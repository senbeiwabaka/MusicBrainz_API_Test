using System;
using System.Collections.Generic;

namespace MusicBrainzAPI.AudioObjects
{
    /// <summary>
    /// This class is the "Recording" of MusicBrainz
    /// </summary>
    public class Song
    {
        /// <summary>
        /// MusicBrainz "Tag-List"
        /// </summary>
        public IList<string> ListofSongGenres { get; set; }

        /// <summary>
        /// The song title
        /// </summary>
        public string SongTitle { get; set; }

        /// <summary>
        /// The song length (in some format?)
        /// </summary>
        public long SongLength { get; set; }

        /// <summary>
        /// The list of albums this song may pertain to
        /// </summary>
        public IList<Album> Albums { get; set; }

        /// <summary>
        /// The list of artists that this song my pertain to
        /// </summary>
        public IList<Artist> Artists { get; set; }

        /// <summary>
        /// The DB ID of the song in MusicBrainz
        /// </summary>
        public readonly Guid ID;

        /// <summary>
        /// The "percent" match this song is to the specified song (used in search)
        /// </summary>
        public readonly int MatchScore;

        public Song(int matchScore = 0, Guid id = new Guid())
        {
            MatchScore = matchScore;
            ID = id;

            Albums = new List<Album>();
            Artists = new List<Artist>();
            ListofSongGenres = new List<string>();
        }

        public override string ToString()
        {
            return "Song Title: " + SongTitle + " ; Song Length: " + SongLength + " ; Match Score: " + MatchScore;
        }
    }
}
