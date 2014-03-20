using MusicBrainzAPI.AudioObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

namespace MusicBrainzAPI.Domain
{
    public class Recording : BaseDomainObject
    {
        private XmlDocument _xmlDocument = null;
        private string _artist = null;
        private string _album = null;
        private string _songTitle = null;

        public IList<Song> Songs { get; set; }
        
        public Recording()
        {
            Songs = new List<Song>();
        }

        public bool SearchGet(string baseURL = "", string artist = "", string album = "", string songTitle = "")
        {
            if (string.IsNullOrEmpty(baseURL))
            {
                baseURL = _baseURL;
            }

            _artist = artist;
            _album = album;
            _songTitle = songTitle;

            string value = baseURL + Configuration.Recording + "\"" + songTitle + "\"" +
                    (!string.IsNullOrEmpty(artist) ? " AND artist:" + artist : string.Empty) +
                    (!string.IsNullOrEmpty(album) ? " AND release:" + album : string.Empty);

            try
            {
                WebRequest request = WebRequest.Create(baseURL + Configuration.Recording + "\"" + songTitle + "\"" +
                    (!string.IsNullOrEmpty(artist) ? " AND artist:" + artist : string.Empty) +
                    (!string.IsNullOrEmpty(album) ? " AND release:" + album : string.Empty));

                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        _xmlDocument = new XmlDocument();
                        _xmlDocument.Load(stream);

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
            XmlNamespaceManager namespaceManager = new XmlNamespaceManager(_xmlDocument.NameTable);
            foreach (XmlNode recording in _xmlDocument.GetElementsByTagName("recording"))
            {
                // song setup
                Song song = new Song(Convert.ToInt32(recording.Attributes["ext:score"].Value), Guid.Parse(recording.Attributes["id"].Value))
                {
                    SongTitle = recording.ChildNodes[0].InnerText.Trim(),
                    
                };

                foreach (XmlElement element in recording.ChildNodes)
                {
                    switch (element.Name.Trim().ToLowerInvariant())
                    {
                        case "length":
                            song.SongLength = Convert.ToInt64(element.InnerText);
                            break;
                        case "artist-credit":
                            // artist setup
                            Artist songArtist = ArtistSetup(element);
                            song.Artists.Add(songArtist);
                            break;
                        case "release-list":
                            // album(s) setup
                            foreach (XmlNode release in element)
                            {
                                Album album = new Album(Guid.Parse(release.Attributes["id"].Value))
                                {
                                    Title = release.ChildNodes[0].InnerText.Trim(),
                                    Status = (Status)Enum.Parse(typeof(Status), release.ChildNodes[1].InnerText)
                                };

                                song.Albums.Add(album);
                            }
                            break;
                        case "tag-list":
                            // song tag setup
                            foreach (XmlNode tag in element)
                            {
                                song.ListofSongGenres.Add(tag.InnerText);
                            }
                            break;
                        default:
                            break;
                    }
                }

                Songs.Add(song);
            }
        }

        private static Artist ArtistSetup(XmlElement artist)
        {
            Artist songArtist = new Artist(Guid.Parse(artist.ChildNodes[0].ChildNodes[0].Attributes["id"].Value))
            {
                ArtistName = artist.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText.Trim()
            };
            return songArtist;
        }
    }
}
