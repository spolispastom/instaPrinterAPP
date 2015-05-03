
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using InstagramPatterns;
using System.Threading;
using System.Collections.Specialized;

namespace InstagramPatterns.InstagramApi
{
    public class InstagramImageDownloader : IDisposable
    {
        
        #region//CLIENT INFO
        protected static string CLIENTID = "fc770cb0f6034b85b5a4e0ba77c5cc28";
        protected static string CLIENTSECRET = "abe6d25713884b35bf69ec3d8bc6f7ac";
        protected static string REDIRECTURI = "http://instasamara.ru";
        protected static string CODE = "34881bf709db4e169204f783104d8a06";

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


        public bool RunFromShortcode(string shortcode)
        {
            if (state == InstagramImageLoederState.Running)
                return false;

            this.tag = shortcode;

            DownloadThread = new Thread(DownloadShotrcodeMetod);
            DownloadThread.Start();
            state = InstagramImageLoederState.Running;
            return true;
        }

        public bool RunFromUser()
        { return RunFromUser(tag); }

        public bool RunFromUser(string user)
        {
            if (state == InstagramImageLoederState.Running)
                return false;

            this.tag = user;
            
            if (importedMediaID != null && importedMediaID.Count > 0)
                importedMediaID.Clear();
            
            DownloadThread = new Thread(DownloadUserMetod);
            DownloadThread.Start();
            state = InstagramImageLoederState.Running;
            return true;
        }

        public bool RunFromTag()
        { return RunFromTag(tag); }

        public bool RunFromTag(string tag)
        { return RunFromTag(tag, new List<string>()); }

        public bool RunFromTag(string tag, ICollection<string> importedMediaIDCollection)
        {
            if (state == InstagramImageLoederState.Running)
                return false;

            this.tag = tag;

            importedMediaID = new StringCollection();
            foreach (string id in importedMediaIDCollection)
                importedMediaID.Add(id);

            DownloadThread = new Thread(DownloadTagMetod);
            DownloadThread.Start();
            state = InstagramImageLoederState.Running;
            return true;
        }


        #region Download metods

        private static DownloadedMedia DatumToDownloadedMediaConvert(global::InstagramPatterns.InstagramApi.MediasJsonTypes.Datum media)
        {
            List<InstagramUser> likes = new List<InstagramUser>();
            foreach (var v in media.Likes.Data)
            { likes.Add(new InstagramUser(v.FullName, v.Username, v.Id, new DataImage(v.ProfilePicture))); }

            string locationName = media.Location != null ? media.Location.Name : "";

            List<string> tags = new List<string>();
            foreach (var t in media.Tags)
            { tags.Add(t); }

            List<InstagramPatterns.InstagramApi.DownloadedMedia.Comment> comments = new List<InstagramPatterns.InstagramApi.DownloadedMedia.Comment>();
            foreach (var c in media.Comments.Data)
            { comments.Add(new InstagramPatterns.InstagramApi.DownloadedMedia.Comment(c.Id, c.Text, c.CreatedTime, new InstagramUser(c.From.FullName, c.From.Username, c.From.Id, new DataImage(c.From.ProfilePicture)))); }

            DateTime createdTime = TimeZoneInfo.ConvertTimeFromUtc(media.CreatedTime, TimeZoneInfo.Local);

            return new DownloadedMedia(media.Id, media.Images.StandardResolution.Url, 
                new InstagramUser(media.User.FullName, media.User.Username, media.User.Id, 
                new DataImage(media.User.ProfilePicture)),
                createdTime, likes, media.Link, locationName, tags, comments);
        }

        private static DownloadedMedia ShortcodeToDownloadedMediaConvert(global::InstagramPatterns.InstagramApi.Shortcode shortcode )
        {
            List<InstagramUser> likes = new List<InstagramUser>();
            foreach (var v in shortcode.Data.Likes.Data)
            { likes.Add(new InstagramUser(v.FullName, v.Username, v.Id, new DataImage(v.ProfilePicture))); }

            string locationName = shortcode.Data.Location != null ? shortcode.Data.Location.Name : "";
            
            List<string> tags = new List<string>();
            foreach (var t in shortcode.Data.Tags)
            { tags.Add(t.ToString()); }

            List<InstagramPatterns.InstagramApi.DownloadedMedia.Comment> comments = new List<InstagramPatterns.InstagramApi.DownloadedMedia.Comment>();
            foreach (var c in shortcode.Data.Comments.Data)
            { comments.Add(new InstagramPatterns.InstagramApi.DownloadedMedia.Comment(c.Id, c.Text, c.CreatedTime, new InstagramUser(c.From.FullName, c.From.Username, c.From.Id, new DataImage(c.From.ProfilePicture)))); }

            DateTime createdTime = TimeZoneInfo.ConvertTimeFromUtc(shortcode.Data.CreatedTime, TimeZoneInfo.Local);

            return new DownloadedMedia(shortcode.Data.Id, shortcode.Data.Images.StandardResolution.Url,
                new InstagramUser(shortcode.Data.User.FullName, shortcode.Data.User.Username, shortcode.Data.User.Id,
                new DataImage(shortcode.Data.User.ProfilePicture)),
                createdTime, likes, shortcode.Data.Link, locationName, tags, comments);
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

        public static string GetUserIDByName(string UserName)
        {
            string uri = string.Format("https://api.instagram.com/v1/users/search?q={0}&client_id={1}", UserName, CLIENTID);
            try
            {
                HttpClient client = new HttpClient();
                string response = client.GetStringAsync(uri).Result;
                User medias = (User)JsonConvert.DeserializeObject(response, typeof(User));
                if (medias.Data.Count() > 0)
                    return medias.Data.First().Id;
                else return "";
            }
            catch
            { return ""; }
        }

        public static Medias GetUserMedias(string userID, string max_tag_id = "1")
        {

            string uri = string.Format("https://api.instagram.com/v1/users/{0}/media/recent?client_id={1}&max_tag_id={2}", userID, CLIENTID, max_tag_id);
            return GetTagMediasFronURL(uri);
        }

        public static Medias GetUserMedias(string userID)
        {
            string uri = string.Format("https://api.instagram.com/v1/users/{0}/media/recent?client_id={1}", userID, CLIENTID);;
            return GetTagMediasFronURL(uri);
        }

        public static Shortcode GetPhotoByShortcode(string shortcode)
        {
            string uri = string.Format("https://api.instagram.com/v1/media/shortcode/{0}?client_id={1}", shortcode, CLIENTID);
            return GetShortcodePhotoFronURL(uri);
        }

        public static Shortcode GetShortcodePhotoFronURL(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                string response = client.GetStringAsync(uri).Result;
                //string response = @"{""meta"":{""code"":200},""data"":{""attribution"":null,""tags"":[],""type"":""image"",""location"":{""latitude"":53.197715854,""longitude"":50.183663176},""comments"":{""count"":0,""data"":[]},""filter"":""Amaro"",""created_time"":""1424716723"",""link"":""https:\/\/instagram.com\/p\/zdDu8ChXSI\/"",""likes"":{""count"":43,""data"":[{""username"":""svetik_nest"",""profile_picture"":""https:\/\/igcdn-photos-a-a.akamaihd.net\/hphotos-ak-xap1\/t51.2885-19\/926814_303758656452152_957114340_a.jpg"",""id"":""1409466793"",""full_name"":""Svetlana""},{""username"":""lapuzenok"",""profile_picture"":""https:\/\/igcdn-photos-f-a.akamaihd.net\/hphotos-ak-xap1\/t51.2885-19\/10724037_1456501817927741_38356487_a.jpg"",""id"":""1099090211"",""full_name"":""\u042f\u0440\u043e\u0441\u043b\u0430\u0432 \u041b\u0430\u043f\u0443\u0437\u0438\u043d""},{""username"":""slav_ko"",""profile_picture"":""https:\/\/instagramimages-a.akamaihd.net\/profiles\/profile_472293052_75sq_1374601273.jpg"",""id"":""472293052"",""full_name"":""\u0421\u043b\u0430\u0432\u0430 \u0427\u0435\u0440\u043d\u044b\u0448\u0435\u0432""},{""username"":""kurlik_murlik"",""profile_picture"":""https:\/\/igcdn-photos-b-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/11032842_1590230317886465_1575754248_a.jpg"",""id"":""285767470"",""full_name"":""\u041c\u0430\u0440\u0443\u0441\u0435\u043d\u044c\u043a\u0430""}]},""images"":{""low_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/s306x306\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":306,""height"":306},""thumbnail"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/s150x150\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":150,""height"":150},""standard_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":640,""height"":640}},""users_in_photo"":[],""caption"":{""created_time"":""1424716723"",""text"":""\u270c\ufe0f\u0441 \u043c\u043e\u0438\u043c\u0438 @ivanovaevgenia @katerina_pea"",""from"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""},""id"":""926913519663019582""},""user_has_liked"":false,""id"":""926913518010463368_418040348"",""user"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""}}}";
                //string response = @"{""meta"":{""code"":200},""data"":{""attribution"":null,""tags"":[],""type"":""image"",""location"":{""latitude"":53.230980369,""longitude"":50.271875836},""comments"":{""count"":3,""data"":[{""created_time"":""1425485784"",""text"":""\u041a\u0440\u0430\u0441\u043e\u0442\u0430 \ud83d\ude48"",""from"":{""username"":""only_lolita"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xfa1\/t51.2885-19\/11017634_1552466208337262_1917941697_a.jpg"",""id"":""391913907"",""full_name"":""""},""id"":""933364867422058000""},{""created_time"":""1425485839"",""text"":""@only_lolita \u0434\u0430\u0430\u0430\u0430) \u043e\u0447\u0435\u043d\u044c \u043a\u0440\u0430\u0441\u0438\u0432\u044b\u0435 \ud83d\ude0a"",""from"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""},""id"":""933365326597682742""},{""created_time"":""1425597029"",""text"":""@zhenechka_korotkova \u044f, \u043a\u0430\u043a \u0431\u0443\u0434\u0442\u043e, \u0433\u0440\u0438\u0431\u043e\u0432 \u043d\u0430\u0435\u043b\u0441\u044f..."",""from"":{""username"":""k163uf"",""profile_picture"":""https:\/\/instagramimages-a.akamaihd.net\/profiles\/profile_971126228_75sq_1389556537.jpg"",""id"":""971126228"",""full_name"":""Kirill Vlasenko""},""id"":""934298056298689608""}]},""filter"":""Lo-fi"",""created_time"":""1425484280"",""link"":""https:\/\/instagram.com\/p\/zz7u3QBXQ2\/"",""likes"":{""count"":48,""data"":[{""username"":""ksenivaa"",""profile_picture"":""https:\/\/igcdn-photos-f-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10995070_694110277367037_281436496_a.jpg"",""id"":""488593016"",""full_name"":""kseniva""},{""username"":""k163uf"",""profile_picture"":""https:\/\/instagramimages-a.akamaihd.net\/profiles\/profile_971126228_75sq_1389556537.jpg"",""id"":""971126228"",""full_name"":""Kirill Vlasenko""},{""username"":""innkou"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10986033_212459855591270_1460092380_a.jpg"",""id"":""357155542"",""full_name"":""\u0418\u043d\u043d\u0430""},{""username"":""alexandra_996"",""profile_picture"":""https:\/\/igcdn-photos-h-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10995058_1545418032384567_729443549_a.jpg"",""id"":""263465967"",""full_name"":""\u0410\u043b\u0435\u043a\u0441\u0430\u043d\u0434\u0440\u0430""}]},""images"":{""low_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/s306x306\/e15\/10483455_794007190694048_1594023115_n.jpg"",""width"":306,""height"":306},""thumbnail"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/s150x150\/e15\/10483455_794007190694048_1594023115_n.jpg"",""width"":150,""height"":150},""standard_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/e15\/10483455_794007190694048_1594023115_n.jpg"",""width"":640,""height"":640}},""users_in_photo"":[],""caption"":{""created_time"":""1425484280"",""text"":""\u0421\u043f\u0430\u0441\u0438\u0431\u043e \u043b\u044e\u0431\u0438\u043c\u044b\u0439 @maksalasheev \u2764\ufe0f\u2764\ufe0f\u2764\ufe0f\u043e\u043d\u0438 \u043f\u043e\u0442\u0440\u044f\u0441\u043d\u044b\ud83d\ude0d"",""from"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""},""id"":""933352356232656504""},""user_has_liked"":false,""id"":""933352252960502838_418040348"",""user"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""}}}";
                //string response = @"{""meta"":{""code"":200},""data"":{""attribution"":null,""tags"":[],""type"":""image"",""location"":{""latitude"":53.27301,""name"":""\u041c\u0435\u043b\u044c\u043d\u0438\u0446\u0430"",""longitude"":50.26828,""id"":502229763},""comments"":{""count"":0,""data"":[]},""filter"":""Normal"",""created_time"":""1410677244"",""link"":""https:\/\/instagram.com\/p\/s6pjXKBXUF\/"",""likes"":{""count"":40,""data"":[{""username"":""bazhenovatanja"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10623696_1530084723877550_563258713_a.jpg"",""id"":""1134415082"",""full_name"":""\u0422\u0430\u043d\u044e\u0448\u043a\u0430 \u0411\u0430\u0436\u0435\u043d\u043e\u0432\u0430""},{""username"":""lapuzenok"",""profile_picture"":""https:\/\/igcdn-photos-f-a.akamaihd.net\/hphotos-ak-xap1\/t51.2885-19\/10724037_1456501817927741_38356487_a.jpg"",""id"":""1099090211"",""full_name"":""\u042f\u0440\u043e\u0441\u043b\u0430\u0432 \u041b\u0430\u043f\u0443\u0437\u0438\u043d""},{""username"":""nadyashka148"",""profile_picture"":""https:\/\/igcdn-photos-a-a.akamaihd.net\/hphotos-ak-xfp1\/t51.2885-19\/10706941_1390981291191744_96414865_a.jpg"",""id"":""1192405458"",""full_name"":""\u041d\u0430\u0434\u0435\u0436\u0434\u0430""},{""username"":""mariya_55555"",""profile_picture"":""https:\/\/instagramimages-a.akamaihd.net\/profiles\/profile_1190144526_75sq_1395155778.jpg"",""id"":""1190144526"",""full_name"":""\u041c\u0430\u0440\u0438\u044f""}]},""images"":{""low_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/s306x306\/e15\/10661283_296235247234589_1296153937_n.jpg"",""width"":306,""height"":306},""thumbnail"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/s150x150\/e15\/10661283_296235247234589_1296153937_n.jpg"",""width"":150,""height"":150},""standard_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xap1\/t51.2885-15\/e15\/10661283_296235247234589_1296153937_n.jpg"",""width"":640,""height"":640}},""users_in_photo"":[],""caption"":null,""user_has_liked"":false,""id"":""809141833019258117_418040348"",""user"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""}}}";
                Shortcode medias = (Shortcode)JsonConvert.DeserializeObject(response, typeof(Shortcode));
                return medias;
            }
            catch
            { return null; }
        }

        public static Medias GetTagMediasFronURL(string uri)
        {
            try
            {
                HttpClient client = new HttpClient();
                string response = client.GetStringAsync(uri).Result;
               // string response = @"{""meta"":{""code"":200},""data"":{""attribution"":null,""tags"":[],""type"":""image"",""location"":{""latitude"":53.197715854,""longitude"":50.183663176},""comments"":{""count"":0,""data"":[]},""filter"":""Amaro"",""created_time"":""1424716723"",""link"":""https:\/\/instagram.com\/p\/zdDu8ChXSI\/"",""likes"":{""count"":43,""data"":[{""username"":""svetik_nest"",""profile_picture"":""https:\/\/igcdn-photos-a-a.akamaihd.net\/hphotos-ak-xap1\/t51.2885-19\/926814_303758656452152_957114340_a.jpg"",""id"":""1409466793"",""full_name"":""Svetlana""},{""username"":""lapuzenok"",""profile_picture"":""https:\/\/igcdn-photos-f-a.akamaihd.net\/hphotos-ak-xap1\/t51.2885-19\/10724037_1456501817927741_38356487_a.jpg"",""id"":""1099090211"",""full_name"":""\u042f\u0440\u043e\u0441\u043b\u0430\u0432 \u041b\u0430\u043f\u0443\u0437\u0438\u043d""},{""username"":""slav_ko"",""profile_picture"":""https:\/\/instagramimages-a.akamaihd.net\/profiles\/profile_472293052_75sq_1374601273.jpg"",""id"":""472293052"",""full_name"":""\u0421\u043b\u0430\u0432\u0430 \u0427\u0435\u0440\u043d\u044b\u0448\u0435\u0432""},{""username"":""kurlik_murlik"",""profile_picture"":""https:\/\/igcdn-photos-b-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/11032842_1590230317886465_1575754248_a.jpg"",""id"":""285767470"",""full_name"":""\u041c\u0430\u0440\u0443\u0441\u0435\u043d\u044c\u043a\u0430""}]},""images"":{""low_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/s306x306\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":306,""height"":306},""thumbnail"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/s150x150\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":150,""height"":150},""standard_resolution"":{""url"":""http:\/\/scontent.cdninstagram.com\/hphotos-xfa1\/t51.2885-15\/e15\/10899134_1556519054587797_477074793_n.jpg"",""width"":640,""height"":640}},""users_in_photo"":[],""caption"":{""created_time"":""1424716723"",""text"":""\u270c\ufe0f\u0441 \u043c\u043e\u0438\u043c\u0438 @ivanovaevgenia @katerina_pea"",""from"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""},""id"":""926913519663019582""},""user_has_liked"":false,""id"":""926913518010463368_418040348"",""user"":{""username"":""zhenechka_korotkova"",""profile_picture"":""https:\/\/igcdn-photos-g-a.akamaihd.net\/hphotos-ak-xaf1\/t51.2885-19\/10864888_1518717501746934_1173628749_a.jpg"",""id"":""418040348"",""full_name"":""Zhenechka)""}}}";
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

        private void DownloadShotrcodeMetod()
        {
            ConcurrentQueueDownloadedImage queueDownloadedImage = ConcurrentQueueDownloadedImage.Instance;

            Shortcode shortcode = GetPhotoByShortcode(tag);
          
            try
            {
                DownloadedMedia dm = ShortcodeToDownloadedMediaConvert(shortcode);
                queueDownloadedImage.Enqueue(dm);
                importedMediaID.Add(shortcode.Data.Id);
                OnMediaDownloaded(this, dm);
            }
            catch { }
            
        }

        private void DownloadUserMetod()
        {
            ConcurrentQueueDownloadedImage queueDownloadedImage = ConcurrentQueueDownloadedImage.Instance;

            foreach (var m in queueDownloadedImage)    //Заполняем колекцию найденых фотографий из тех что в очереди.
                importedMediaID.Add(m.Id);      //В большинтве случаев очередь тоже пуста.

            string userID = GetUserIDByName(tag);
            Medias medias = GetUserMedias(userID);
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

                    if ((isFastDowland && isEndLine) || medias.Pagination.NextUrl == null)
                    {
                        medias = GetUserMedias(userID);
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
        
        private void DownloadTagMetod()
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

                    if ((isFastDowland && isEndLine) || medias.Pagination.NextUrl == null)
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

        #endregion

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
