using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Web;
using System.IO;

namespace fanfiction.net_story_downloader
{
    class Program
    {
        

        static void Main(string[] args)
        {
            string url;
            int baseChapter;
            int finalChapter;

            if (args.Length == 4)
            {
                url = args[0];
                baseChapter = Int32.Parse(args[1]);
                string title = args[3];

                if (args[2] != "f")
                {
                    finalChapter = Int32.Parse(args[2]);
                    if (finalChapter <= baseChapter)
                    {
                        finalChapter = -1;
                    }

                }
                else
                {
                    finalChapter = -1;
                }

                downloadFick(url, baseChapter, finalChapter, title);

            }
            else
            {
                Console.WriteLine("Los argumentos para usar el programa son: \n ffsd.exe url cap_Inicial cap_final titulo \n para bajar todo el fick, simplemente colocar url 0 f.  titulo ");
            }

        }

        static void loadIni()
        {

        }

        static void downloadFick(string url, int inicial, int final, string nmr)
        {
            Console.WriteLine("descargando la historia \n " + url + " \n desde el capítulo " + inicial.ToString() + " hasta el capítulo " + final.ToString() + ". ");
            string storyTitle = nmr;

            while (true)
            {
                if (!Directory.Exists(nmr))
                {
                    Directory.CreateDirectory(nmr);

                    break;
                }
                else
                {
                    nmr += ".new";

                }
            }

            storyTitle = nmr;

            if (final > 0)
            {
                Console.WriteLine("descargando hasta el cap especificado ");

                for (int i = inicial; i <= final; i++)
                {
                    downloadChapter(url, i, storyTitle, true);


                }

            }
            else
            {
                Console.WriteLine("descargando hasta el final ");
                bool fin = false;
                int current = inicial;

                while (!fin)
                {
                    fin = downloadChapter(url, current, storyTitle, false);
                    current++;

                }
            }
        }

        
        static  bool downloadChapter(string storyurl, int chapt, string storyTitle, bool direct)
        {
            WebClient client = new WebClient();
            client.Encoding = Encoding.UTF8;

            string urlToDownload = storyurl;
            if (urlToDownload.EndsWith("/"))
            {
                urlToDownload += chapt.ToString();
            }
            else
            {
                urlToDownload += "/" + chapt.ToString();
            }

            Console.WriteLine("bajando el cap " + chapt.ToString());

            if (direct)
            {
                client.DownloadFile(new Uri(urlToDownload), storyTitle + "\\" + storyTitle + " chapter " + chapt.ToString() + ".html");
                return (false);



            }
            else
            {
                string content = client.DownloadString(urlToDownload);
                
                if (!content.StartsWith("<!DOCTYPE"))
                {
                    Console.WriteLine("se llegó al cap final ");

                    return (true);

                }

                FileStream fs = new FileStream(storyTitle + "\\" + storyTitle + " " + chapt.ToString() + ".html", FileMode.OpenOrCreate);
                StreamWriter  textin = new StreamWriter(fs, Encoding.UTF8);
                textin.Write(content);
                textin.Flush();
                textin.Close();
                fs.Close();


                return (false);
            }
            

            return (true);

        }

    }
}
