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
        public Album Album { get; set; }

    }
}
