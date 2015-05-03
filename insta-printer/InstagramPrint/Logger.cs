using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstagramPrint
{
   public class Logger
    {
        private string currentTag = "";
       

        public void SetCurrentTag(string tag)
        {
            if (currentTag != tag)
            {
                log = new List<LogItem>();
                currentTag = tag;
            }
            if (!File.Exists(tag + "d.txt"))
            {
                try
                {
                    File.Create(tag + "d.txt");
                }
                catch
                {
                    File.Create("linkd.txt");
                }
            }
            else
            {
                StreamReader sr = File.OpenText(tag + "d.txt");
                string id = sr.ReadLine();
                while (id != null)
                {
                    log.Add(new LogItem(id, currentTag, false));
                    id = sr.ReadLine();
                }
                sr.Close();
            }
            if (!File.Exists(tag + "p.txt"))
            {
                try
                {
                    File.Create(tag + "p.txt");
                }
                catch
                {
                    File.Create("linkp.txt");
                }
            }
            else
            {
                StreamReader sr = File.OpenText(tag + "p.txt");
                string id = sr.ReadLine();
                while (id != null)
                {
                    var lf = log.Where(x => x.ID == id);
                    if (lf != null && lf.Count() > 0)
                    {
                        LogItem item = lf.First();
                        if (item != null)
                            item.IsPrinted = true;
                    }
                    id = sr.ReadLine();
                }
                sr.Close();
            }
        }

        public static List<LogItem> log;

        public void SetDowlandedMediaID(string id)
        {
            log.Add(new LogItem(id, currentTag, false));
            StreamWriter sw = File.AppendText(currentTag + "d.txt");
            sw.WriteLine(id);
            sw.Close();

        }
        public void SetPrintedMediaID(string id) 
        {
            log.Where(x => x.ID == id).First().IsPrinted = true;
            StreamWriter sw = File.AppendText(currentTag + "p.txt");
            sw.WriteLine(id);
            sw.Close();
        }

        public ICollection<LogItem> GetLog()
        {
            return log;
        }

        public class LogItem
        {
            public readonly string ID;
            public readonly string Tag;
            public bool IsPrinted;

            public LogItem(string id, string tag, bool isPrinted)
            {
                this.ID = id;
                this.Tag = tag;
                this.IsPrinted = isPrinted;
            }

        }
    }
}
