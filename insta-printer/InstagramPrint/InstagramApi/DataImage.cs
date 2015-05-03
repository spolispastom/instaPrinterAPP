using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPrint.InstagramApi
{
    public struct DataImage
    {
        public DataImage(string url)
            : this(new Uri(url)) { }

        public DataImage(Uri uri) 
        {
            HttpClient http = new HttpClient();
            data = http.GetByteArrayAsync(uri).Result;
            http.Dispose();
        }

        public DataImage(byte[] data)
        {
            this.data = data;
        }

        private byte[] data;

        public byte[] GetData()
        {
            return data;
        }

        public MemoryStream GetStream()
        {
            return new MemoryStream(data);
        }
    }
}
