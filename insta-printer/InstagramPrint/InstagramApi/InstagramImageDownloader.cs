
using InstagramPrint.InstagramApi;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InstagramPrint;
using System.Threading;
using System.Collections.Specialized;

namespace InstagramPrint.InstagramApi
{
    class InstagramImageDownloader : IDisposable
    {
        
        #region//CLIENT INFO
        protected static string CLIENTID = "fc770cb0f6034b85b5a4e0ba77c5cc28";
        protected static string CLIENTSECRET = "abe6d25713884b35bf69ec3d8bc6f7ac";
        protected static string REDIRECTURI = "http://instasamara.ru";
        #endregion

        Thread DownloadThread;
        protected string tag;
        StringCollection importedMediaID = new StringCollection();
        protected bool isFastDowland = true;
        public bool IsFastDowland
        {
            get { return isFastDowland; }
            set { isFastDowland = value; }
        }

        public InstagramImageDownloader(string tag)
        {
            this.tag = tag;
            state = InstagramImageLoederState.Stopped;
            //Run();
        }

        public bool Run()
        { return Run(tag); }

        public bool Run(string tag)
        { return Run(tag, new List<string>()); }

        public bool Run(string tag, ICollection<string> importedMediaIDCollection)
        {
            if (state == InstagramImageLoederState.Running)
                return false;

            this.tag = tag;

            importedMediaID = new StringCollection();
            foreach (string id in importedMediaIDCollection)
                importedMediaID.Add(id);

            DownloadThread = new Thread(DownloadMetod);
            DownloadThread.Start();
            state = InstagramImageLoederState.Running;
            return true;
        }

        private static DownloadedMedia DatumToDownloadedMediaConvert(global::InstagramPrint.InstagramApi.MediasJsonTypes.Datum media)
        {
            List<InstagramUser> likes = new List<InstagramUser>();
            foreach (var v in media.Likes.Data)
            { likes.Add(new InstagramUser(v.FullName, v.Username, v.Id, new DataImage(v.ProfilePicture))); }

            string locationName = media.Location != null ? media.Location.Name : "";

            List<string> tags = new List<string>();
            foreach (var t in media.Tags)
            { tags.Add(t); }

            List<InstagramPrint.InstagramApi.DownloadedMedia.Comment> comments = new List<InstagramPrint.InstagramApi.DownloadedMedia.Comment>();
            foreach (var c in media.Comments.Data)
            { comments.Add(new InstagramPrint.InstagramApi.DownloadedMedia.Comment(c.Id, c.Text, c.CreatedTime, new InstagramUser(c.From.FullName, c.From.Username, c.From.Id, new DataImage(c.From.ProfilePicture)))); }

            DateTime createdTime = TimeZoneInfo.ConvertTimeFromUtc(media.CreatedTime, TimeZoneInfo.Local);

            return new DownloadedMedia(media.Id, media.Images.StandardResolution.Url, 
                new InstagramUser(media.User.FullName, media.User.Username, media.User.Id, 
                new DataImage(media.User.ProfilePicture)),
                createdTime, likes, media.Link, locationName, tags, comments);
        }

        public static DownloadedMedia GetMedia(string tag, string max_tag_id = "1", int index = 0)
        {
            Medias medias = GetTagMedias(tag, max_tag_id);
            if (medias != null)
                return DatumToDownloadedMediaConvert(medias.Data[index]);
            else return null;
        }

        public static Medias GetTagMedias(string tag, string max_tag_id = "1")
        {
            string uri = string.Format("https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}&max_tag_id={2}", tag, CLIENTID, max_tag_id);
            return GetTagMediasFronURL(uri);
        }

        public static Medias GetTagMedias(string tag)
        {
            string uri = string.Format("https://api.instagram.com/v1/tags/{0}/media/recent?client_id={1}", tag, CLIENTID);
            return GetTagMediasFronURL(uri);
        }

        public static Medias GetTagMediasFronURL(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                string response = client.GetStringAsync(uri).Result;
                Medias medias = (Medias)JsonConvert.DeserializeObject(response, typeof(Medias));
                return medias;
            }
            catch
            { return null; }
        }

        private DateTime endLoedTime = new DateTime(1, 1, 1);

        public DateTime EndLoedTime
        {
            get { return endLoedTime; }
            set { endLoedTime = value; }
        }

        private void DownloadMetod()
        {
            ConcurrentQueueDownloadedImage queueDownloadedImage = ConcurrentQueueDownloadedImage.Instance;

            foreach (var m in queueDownloadedImage)    //Заполняем колекцию найденых фотографий из тех что в очереди.
                importedMediaID.Add(m.Id);      //В большинтве случаев очередь тоже пуста.


            Medias medias = GetTagMedias(tag);
            bool isEndLine = false;
            while (true)
            {
                if (medias == null)
                {
                    state = InstagramImageLoederState.NotConnection;
                    return;
                }

                if (medias != null)
                {
                    foreach (var media in medias.Data)
                    {
                        if (media.Type == "image")
                        {
                            if (importedMediaID.Contains(media.Id) || (media.CreatedTime != null && endLoedTime > media.CreatedTime))
                            {
                                isEndLine = true;
                                continue;
                            }
                            try
                            {
                                DownloadedMedia dm = DatumToDownloadedMediaConvert(media);
                                queueDownloadedImage.Enqueue(dm);
                                importedMediaID.Add(media.Id);
                                OnMediaDownloaded(this, dm);
                            }
                            catch { }
                        }
                        else
                        {
                            continue;
                        }
                    }

                    if ((isFastDowland && isEndLine))
                    {
                        medias = GetTagMedias(tag);
                        isEndLine = false;
                    }
                    else
                        medias = GetTagMediasFronURL(medias.Pagination.NextUrl);
                }

                //if (isFirstDownload)
                //{
                //    foreach (var m in queueDownloadedImage)
                //    {
                //        //if (m.p)
                //        //importedMediaID.Add(m.Id);
                //    }
                //    isFirstDownload = false;
                //}
            }
        }

        private InstagramImageLoederState state;

        internal InstagramImageLoederState State
        { get { return state; } }

        public enum InstagramImageLoederState
        {
            Running,
            Stopped,
            NotConnection
        }

        public delegate void MediaDownloadedEventHendler(object sender, DownloadedMedia media);

        public event MediaDownloadedEventHendler MediaDownloaded;

        private void OnMediaDownloaded(object sender, DownloadedMedia media)
        {
            if (MediaDownloaded != null)
                MediaDownloaded(sender, media);
        }

        public void Cancel()
        {
            if (DownloadThread != null)
            {
                DownloadThread.Abort();
                state = InstagramImageLoederState.Stopped;
            }
        }

        public void Dispose()
        { Cancel(); }
    }
}
