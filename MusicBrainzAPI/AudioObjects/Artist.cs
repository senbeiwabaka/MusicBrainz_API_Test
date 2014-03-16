using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI.AudioObjects
{
    public class Artist
    {
        public string ArtistName { get; set; }
        public readonly Guid ID;
        public IList<Album> Albums { get; set; }

        public Artist(Guid id = new Guid())
        {
            ID = id;
            Albums = new List<Album>();
        }
    }
}