using InstagramPrint;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPrint.InstagramApi
{
    public class DownloadedMedia : IDisposable, IComparable
    {
        public DownloadedMedia(string id, string photoUrl, InstagramUser user,
            DateTime createdTime, List<InstagramUser> likes, string link, string location, List<string> tags, List<Comment> comments)
        {
            this.id = id;
            this.photo = new DataImage(photoUrl);
            this.user = user;
            this.createdTime = createdTime;
            this.likes = likes;
            this.link = link;
            this.locationName = location;
            this.tags = tags;
            this.comments = comments;
        }

        private string id;
        public string Id
        {
            get { return id; }
        }

        private DataImage photo;
        public DataImage Photo
        {
            get { return photo; }
        }

        private InstagramUser user;
        public InstagramUser User
        {
            get { return user; }
        }

        public string UserName
        { get { return user.Name != null ? user.Name : user.FullName; } }

        private DateTime createdTime;
        public DateTime CreatedTime
        {
            get { return createdTime; }
        }

        public string StringCreatedTime
        { get { return createdTime.ToString("dd MMM HH:mm"); } }

        private List<InstagramUser> likes;
        public List<InstagramUser> Likes
        {
            get { return likes; }
        }

        private string link;
        public string Link
        {
            get { return link; }
        }

        private string locationName;
        public string LocationName
        {
            get { return locationName; }
        }

        private List<string> tags;
        public List<string> Tags
        {
            get { return tags; }
        }

        private List<Comment> comments;
        public List<Comment> Comments
        {
            get { return comments; }
        }

        public struct Comment
        {
            public string Id;
            public string Text;
            public string CreatedTime;
            public InstagramUser User;

            public Comment(string id, string text, string created_time, InstagramUser user)
            {
                this.Id = id;
                this.Text = text;
                this.CreatedTime = created_time;
                this.User = user;
            }
 
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public int CompareTo(object obj)
        {
            return id.CompareTo((obj as DownloadedMedia).id);
        }
    }
}
