using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Script.Serialization;

namespace artistlyrics
{
    internal class Program 
    {
        /// <summary>
        /// API which takes input as Artist and Title to get the lyrics
        /// </summary>
        private const string uri = "https://api.lyrics.ovh/v1/{0}/{1}";
        /// <summary>
        /// API which takes input as Artist and gets all his song titles
        /// </summary>
        private const string Format = "https://api.lyrics.ovh/suggest/{0}";

        
        static void Main(string[] args)
        {
            string artist;
            bool confirmed = false;
            

            Console.WriteLine("Provide Artist Name:");
            artist = Console.ReadLine();
            Console.Write("Do you want to see all titles list choosen for this artist? [y/n] ");
            ConsoleKey response = Console.ReadKey(false).Key;
            confirmed = response == ConsoleKey.Y ? true : false;
            if (response != ConsoleKey.Enter)
                Console.WriteLine("Fetching Details.........");
            //Console.WriteLine("Provide Artist Name:");
            // artist = Console.ReadLine();
            //CallingAPI(artist, confirmed);

            CallingAPIString(artist, confirmed);
            
            Console.WriteLine("Press any key to exit");
            Console.ReadLine();
        }


        /// <summary>
        /// used Class Objects to retails data for any other purpose
        /// </summary>
        /// <param name="artist"></param>

        private static void CallingAPI(string artist, bool showTitle)
        {
            string title = "Adventure of a Lifetime";
            string strUrl2 = string.Format(uri, artist, title);
            string strUrl = string.Format(Format, artist);
            StreamReader reader = null;
            WebRequest webRequest = null;
            HttpWebResponse responseObj = null;

            WebRequest webRequest2 = null;
            HttpWebResponse responseObj2 = null;
            StreamReader reader2 = null;
            StringBuilder allLyrics = new StringBuilder();
            char[] chars = { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
            try
            {
                webRequest = (WebRequest)WebRequest.Create(strUrl);
                webRequest.Method = "GET";

                responseObj = (HttpWebResponse)webRequest.GetResponse();
                using (Stream streamRep = responseObj.GetResponseStream())
                {
                    reader = new StreamReader(streamRep);
                    string str = reader.ReadToEnd();
                    JObject json = JObject.Parse(str);
                    ICollection<Songs> songs = new List<Songs>();

                    dynamic data = JsonConvert.DeserializeObject<dynamic>(str);

                    Songs songs1 = new Songs();

                    songs1.MyName = artist;
                    ICollection<Lyrics> collyrics = new List<Lyrics>();

                    Console.WriteLine("Songs List by Artist:" + artist);
                    Console.WriteLine();
                    foreach (var item in data.data)
                    {

                        Lyrics lyrics = new Lyrics();

                        lyrics.MyTitle = item.title;
                        strUrl2 = string.Format(uri, artist, item.title);
                        try
                        {

                            webRequest2 = (WebRequest)WebRequest.Create(strUrl2);
                            webRequest2.Method = "GET";

                            responseObj2 = (HttpWebResponse)webRequest2.GetResponse();
                            using (Stream streamRep2 = responseObj2.GetResponseStream())
                            {
                                reader2 = new StreamReader(streamRep2);
                                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(reader2.ReadToEnd());
                                lyrics.MyLyric = data2.lyrics;
                                WordsInSong wordsInLyric = new WordsInSong();
                                string wordsintext = data2.lyrics;

                                IList<WordsInSong> enumerable = wordsintext.Split(chars).GroupBy(x => x).Select(y => new WordsInSong { UniqueWord = y.Key, CountOfOccurance = y.Count() }).ToList();
                                //ICollection<WordsInSong> wordsInSongs=new List<WordsInSong>();
                                //wordsInSongs = enumerable;

                                lyrics.WordsInLyric = enumerable;

                                if (showTitle)
                                {
                                    Console.WriteLine(lyrics.MyTitle);
                                }
                                //Console.WriteLine(enumerable);

                                allLyrics.Append(data2.lyrics);

                                collyrics.Add(lyrics);


                            }




                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                            reader2.Close();
                            reader2.Dispose();
                            responseObj2.Close();
                        }


                    }
                    string strAllLyrics = allLyrics.ToString();
                    IList<WordsInSong> lstAllLyrics = strAllLyrics.Split(chars).GroupBy(x => x).Select(y => new WordsInSong { UniqueWord = y.Key, CountOfOccurance = y.Count() }).OrderByDescending(z => z.CountOfOccurance).ToList();
                    Console.WriteLine(string.Format("Word     ||     Occurance"));
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine();
                    foreach (var item in lstAllLyrics)
                    {

                        Console.WriteLine(string.Format("{0}     ||     {1}", item.UniqueWord, item.CountOfOccurance));
                        Console.WriteLine("--------------------------------------------------------");


                    }
                    songs1.allSongs = collyrics;


                    songs.Add(songs1);

                }

            }
            finally
            {


                reader.Close();
                responseObj.Close();

            }
        }

        /// <summary>
        /// used only strings and Linq
        /// </summary>
        /// <param name="artist"></param>
        private static void CallingAPIString(string artist, bool showTitle)
        {
            string title = "";
            string strUrl2 = string.Format(uri, artist, title);
            string strUrl = string.Format(Format, artist);
            StreamReader reader = null;
            WebRequest webRequest = null;
            HttpWebResponse responseObj = null;

            WebRequest webRequest2 = null;
            HttpWebResponse responseObj2 = null;
            StreamReader reader2 = null;
            StringBuilder allLyrics = new StringBuilder();
            char[] chars = { ' ', '.', ',', ';', ':', '?', '\n', '\r' };
            try
            {
                webRequest = (WebRequest)WebRequest.Create(strUrl);
                webRequest.Method = "GET";

                responseObj = (HttpWebResponse)webRequest.GetResponse();
                using (Stream streamRep = responseObj.GetResponseStream())
                {
                    reader = new StreamReader(streamRep);
                    string str = reader.ReadToEnd();
                    JObject json = JObject.Parse(str);


                    dynamic data = JsonConvert.DeserializeObject<dynamic>(str);


                    Console.WriteLine();
                    Console.WriteLine("Songs List by Artist:" + artist);
                    Console.WriteLine();
                    foreach (var item in data.data)
                    {


                        strUrl2 = string.Format(uri, artist, item.title);
                        try
                        {

                            webRequest2 = (WebRequest)WebRequest.Create(strUrl2);
                            webRequest2.Method = "GET";

                            responseObj2 = (HttpWebResponse)webRequest2.GetResponse();
                            using (Stream streamRep2 = responseObj2.GetResponseStream())
                            {
                                reader2 = new StreamReader(streamRep2);
                                dynamic data2 = JsonConvert.DeserializeObject<dynamic>(reader2.ReadToEnd());
                                string wordsintext = data2.lyrics;
                                // IList<WordsInSong> enumerable = wordsintext.Split(chars).GroupBy(x => x).Select(y => new WordsInSong { UniqueWord = y.Key, CountOfOccurance = y.Count() }).ToList();



                                if (showTitle)
                                {
                                    Console.WriteLine(item.title);
                                }
                                //Console.WriteLine(enumerable);

                                allLyrics.Append(data2.lyrics);




                            }




                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {

                            reader2.Close();
                            reader2.Dispose();
                            responseObj2.Close();
                        }


                    }
                    string strAllLyrics = allLyrics.ToString();
                    IList<WordsInSong> lstAllLyrics = strAllLyrics.Split(chars).GroupBy(x => x).Select(y => new WordsInSong { UniqueWord = y.Key, CountOfOccurance = y.Count() }).OrderByDescending(z => z.CountOfOccurance).ToList();
                    Console.WriteLine(string.Format("Word     ||     Occurance"));
                    Console.WriteLine("--------------------------------------------------------");
                    Console.WriteLine();
                    foreach (var item in lstAllLyrics)
                    {

                        Console.WriteLine(string.Format("{0}     ||     {1}", item.UniqueWord, item.CountOfOccurance));
                        Console.WriteLine("--------------------------------------------------------");


                    }


                }

            }
            catch (Exception ex) { }
            finally
            {


                reader.Close();
                reader.Dispose();
                responseObj.Close();

            }
        }
    }
}
