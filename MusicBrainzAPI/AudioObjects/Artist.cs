using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI.AudioObjects
{
    /// <summary>
    /// This class is the "Artist" of MusicBrainz
    /// </summary>
    public class Artist
    {
        /// <summary>
        /// The artist name
        /// </summary>
        public string ArtistName { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public readonly Guid ID;
        
        /// <summary>
        /// 
        /// </summary>
        public IList<Album> Albums { get; set; }

        public Artist(Guid id = new Guid())
        {
            ID = id;
            Albums = new List<Album>();
        }
    }
}