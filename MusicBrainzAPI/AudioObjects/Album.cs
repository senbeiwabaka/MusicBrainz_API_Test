using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI.AudioObjects
{
    public enum Status
	{
        Official,
        None
	}

    public enum ReleaseType
	{
        Album,
        Compilation
	}

    public class Album
    {
        public string Title { get; set; }
        public Status Status { get; set; }
        public DateTime ReleasedDate { get; set; }
        public string Country { get; set; }
        public string ArtistName { get; set; }
        public List<ReleaseType> ReleaseTypes { get; set; }
        public string Format { get; set; }
        public byte TrackCount { get; set; }

        public Album()
        {
            Title = string.Empty;
            Status = Status.None;
            ReleasedDate = DateTime.Now;

        }
    }
}
