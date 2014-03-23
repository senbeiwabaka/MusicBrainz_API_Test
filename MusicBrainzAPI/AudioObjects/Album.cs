using System;
using System.Collections.Generic;

namespace MusicBrainzAPI.AudioObjects
{
    /// <summary>
    /// The album release type such as album or EP
    /// </summary>
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

    /// <summary>
    /// The status of the album such as official or promotion
    /// </summary>
    public enum Status
    {
        Bootleg,
        Official,
        Pseudo_Release,
        Promotion,
        None
    }

    /// <summary>
    /// This class is the "Release" of MusicBrainz
    /// </summary>
    public class Album
    {
        public readonly Guid ID;

        public Album(Guid id = new Guid())
        {
            Status = Status.None;
            ReleasedDate = DateTime.Now;
            ID = id;
            Artists = new List<Artist>();
            PrimaryType = ReleaseType.None;
            SecondaryType = ReleaseType.None;
            Tracks = new List<Track>();
        }

        public IList<Artist> Artists { get; set; }

        public string Country { get; set; }

        public string Format { get; set; }

        public string RecordLabel { get; set; }

        public DateTime ReleasedDate { get; set; }

        public Status Status { get; set; }

        public string Title { get; set; }

        public byte TrackCount { get; set; }

        public string AmazonASIN { get; set; }

        public ReleaseType PrimaryType { get; set; }

        public ReleaseType SecondaryType { get; set; }

        public int TrackPosition { get; set; }

        public IList<Track> Tracks { get; set; }

        public override string ToString()
        {
            return "Album Title: " + Title + " ; Country: " + Country + " ; Status: " + Status.ToString();
        }
    }
}