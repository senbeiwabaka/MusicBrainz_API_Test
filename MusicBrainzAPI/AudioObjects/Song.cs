using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI.AudioObjects
{
    public class Song
    {
        public IList<string> ListofSongGenres { get; set; }
        public string SongTitle { get; set; }
        public long SongLength { get; set; }
        public IList<Album> Albums { get; set; }
        public IList<Artist> Artists { get; set; }
        public readonly Guid ID;
        public readonly int MatchScore;

        public Song(int matchScore = 0, Guid id = new Guid())
        {
            MatchScore = matchScore;
            ID = id;

            Albums = new List<Album>();
            Artists = new List<Artist>();
            ListofSongGenres = new List<string>();
        }
    }
}
