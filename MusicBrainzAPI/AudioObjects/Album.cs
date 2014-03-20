using System;
using System.Collections.Generic;

namespace MusicBrainzAPI.AudioObjects
{
    public enum ReleaseType
    {
        Album,
        Audiobook,
        Compilation,
        EP,
        Interview,
        Live,
        Nat,
        Other,
        Promotion,
        Remix,
        Single,
        Soundtrack,
        Spokenword,
        None
    }

    public enum Status
    {
        Bootleg,
        Official,
        Pseudo_Release,
        Promotion,
        None
    }

    public class Album
    {
        public readonly Guid ID;

        public Album(Guid id = new Guid())
        {
            Status = Status.None;
            ReleasedDate = DateTime.Now;
            ID = id;
            ReleaseTypes = new List<ReleaseType>();
            ListofAlbumGenres = new List<string>();
            Artists = new List<Artist>();
        }

        public IList<Artist> Artists { get; set; }

        public string Country { get; set; }

        public string Format { get; set; }

        public IList<string> ListofAlbumGenres { get; set; }

        public string RecordLabel { get; set; }

        public DateTime ReleasedDate { get; set; }

        public IList<ReleaseType> ReleaseTypes { get; set; }

        public Status Status { get; set; }

        public string Title { get; set; }

        public byte TrackCount { get; set; }
    }
}