using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace artistlyrics
{
    internal class Songs
    {
        private string myName;

        public string MyName
        {
            get { return myName; }
            set { myName = value; }
        }

        public ICollection<Lyrics> allSongs { get; set; }

        public WordsInSong WordsInSong { get; set; }

    }

    internal class Lyrics
    {
        private string Title;

        public string MyTitle
        {
            get { return Title; }
            set { Title = value; }
        }

        private string Lyric;

        public string MyLyric
        {
            get { return Lyric; }
            set { Lyric = value; }
        }

        public ICollection<WordsInSong> WordsInLyric { get; set; }

        //public void getUniqueWordsCount()
        //{
            
        //        var myuniquewordscount= MyLyric.Split(' ').GroupBy(x => x).Select(y => new WordsInSong { UniqueWord = y.Key, CountOfOccurance = y.Count() }).OrderBy(z => z.CountOfOccurance);
        //        
            
        //}


    }

    internal class WordsInSong
    {
        public string UniqueWord { get; set; }
        public int CountOfOccurance { get; set; }

        
    }
}
