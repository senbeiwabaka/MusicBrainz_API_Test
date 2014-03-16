using MusicBrainzAPI.AudioObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Xml;

namespace MusicBrainzAPI.Domain
{
    public class Recording : BaseDomainObject
    {
        private XmlDocument XmlDocument { get; set; }

        public IList<Song> Songs { get; set; }
        
        public Recording()
        {
            Songs = new List<Song>();
        }

        public bool Get(string baseURL = "", string artist = "", string album = "", string songTitle = "")
        {
            try
            {
                if (string.IsNullOrEmpty(baseURL))
                {
                    baseURL = "http://musicbrainz.org/ws/2/";
                }
                WebRequest request = WebRequest.Create(baseURL + Configuration.Recording + "\"" + songTitle + "\" AND artist:" + artist + " AND release:" + album);

                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        XmlDocument = new XmlDocument();
                        XmlDocument.Load(stream);

                        Setup();
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        private void Setup()
        {
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(XmlDocument.NameTable);
            foreach (XmlNode recording in XmlDocument.GetElementsByTagName("recording"))
            {
                // song setup
                Song song = new Song(Convert.ToInt32(recording.Attributes["ext:score"].Value),Guid.Parse(recording.Attributes["id"].Value));

                song.SongTitle = recording.ChildNodes[0].InnerText;

                song.SongLength = Convert.ToInt64(recording.ChildNodes[1].InnerText);

                // artist setup
                Artist songArtist = new Artist(Guid.Parse(recording.ChildNodes[2].ChildNodes[0].ChildNodes[0].Attributes["id"].Value)) 
                { 
                    ArtistName = recording.ChildNodes[2].ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText 
                };

                var something = recording.SelectSingleNode("/release*");
                Console.WriteLine(something);

                // album(s) setup
                foreach (XmlNode release in recording.ChildNodes[3])
                {
                    Album album = new Album(Guid.Parse(release.Attributes["id"].Value))
                    {
                        Title = release.ChildNodes[0].InnerText,
                        Status = (Status)Enum.Parse(typeof(Status), release.ChildNodes[1].InnerText)
                    };

                    album.Artists.Add(songArtist);

                    song.Albums.Add(album);
                }

                song.Artists.Add(songArtist);

                // song tag setup
                foreach (XmlNode tag in recording.ChildNodes[4])
                {
                    song.ListofSongGenres.Add(tag.InnerText);
                }
            }
        }
    }
}
