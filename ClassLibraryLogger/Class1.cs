using System;
using System.IO;

namespace ClassLibraryLogger
{
    public class MyLog
    {
        private int filrcount;
        private string path = @"C:\LogDir";
        const int filelimit = 5;
        

        public MyLog() 
        {
            DirectoryInfo dirInfo = new DirectoryInfo(path);
            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }
            filrcount = 0; 
        }

        public MyLog(string newpath)
        {
            path = newpath;

            DirectoryInfo dirInfo = new DirectoryInfo(newpath);

            if (!dirInfo.Exists)
            {
                dirInfo.Create();
            }

            filrcount = 0;
        }

        public void Logging(string type,string msg)
        {
            if (filrcount == 0)
            {
                filrcount++;
               
            }

            using (FileStream fstream = new FileStream($@"{path}\Log-{filrcount}.txt", FileMode.OpenOrCreate))
            {
                if (fstream.Length > 200000000)
                {
                    filrcount++;
                }
                
            }

            if (filrcount > filelimit)
            {
                filrcount = filrcount % filelimit;
            }

            

            using (FileStream fstream = new FileStream($@"{path}\Log-{filrcount}.txt", FileMode.OpenOrCreate))
            {
                string text = $"{type}: MSG - {msg}";
                byte[] array = System.Text.Encoding.Default.GetBytes(text);
                fstream.Write(array, 0, array.Length);

            }
                Console.WriteLine($"{type} msg logged!");
        }

        public void Log(string infomsg)
        {
            Logging("INFO", infomsg);
        }


        public void LogException(string errmsg)
        {
            Logging("ERROR", errmsg);
        }


    }
}
