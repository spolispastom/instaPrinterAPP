using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPrint.InstagramApi
{
    public class InstagramUser
    {
        public InstagramUser(string full_name, string username, string id, DataImage profilePicture)
        {
            FullName = full_name;
            Name = username;
            ProfilePicture = profilePicture;
            Id = id;
        }

        public readonly string Id;
        public readonly string FullName;
        public readonly string Name;
        public DataImage ProfilePicture;

    }
}
