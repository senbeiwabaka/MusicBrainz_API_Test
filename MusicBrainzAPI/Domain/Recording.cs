using MusicBrainzAPI.AudioObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;
using System.Xml.Serialization;

namespace MusicBrainzAPI.Domain
{
    public class Recording : BaseDomainObject, IServices
    {
        private XmlDocument _xmlDocument = null;

        public IList<Song> Songs { get; set; }

        public Recording()
        {
            Songs = new List<Song>();
        }

        public bool SearchGet(string baseURL = "", string query = "")
        {
            if (string.IsNullOrEmpty(baseURL))
            {
                baseURL = _baseURL;
            }
            string url = baseURL + Configuration.Recording + query;

            try
            {
                WebRequest request = WebRequest.Create(url);

                request.Method = "GET";

                HttpWebResponse response = request.GetResponse() as HttpWebResponse;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    using (Stream stream = response.GetResponseStream())
                    {
                        _xmlDocument = new XmlDocument();
                        _xmlDocument.Load(stream);

                        SearchSetup();
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }

            return true;
        }

        public bool Get(string baseURL = "", string query = "")
        {
            throw new NotImplementedException();
        }

        #region Helper Methods

        /// <summary>
        /// Sets up the list of songs returned from search
        /// </summary>
        private void SearchSetup()
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
                            song.Artists.Add(ArtistSetup(element));
                            break;
                        case "release-list":
                            // album(s) setup
                            AlbumSetup(song, element);
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

        private static void AlbumSetup(Song song, XmlElement element)
        {
            foreach (XmlNode release in element)
            {
                Album album = new Album(Guid.Parse(release.Attributes["id"].Value));

                foreach (XmlElement releaseElement in release)
                {
                    switch (releaseElement.Name.Trim().ToLowerInvariant())
                    {
                        case "title":
                            album.Title = releaseElement.InnerText.Trim();
                            break;
                        case "status":
                            album.Status = (Status)Enum.Parse(typeof(Status), releaseElement.InnerText);
                            break;
                        case "artist-credit":
                            Artist artist = new Artist(Guid.Parse(releaseElement.ChildNodes[0].ChildNodes[0].Attributes["id"].Value))
                            {
                                ArtistName = releaseElement.ChildNodes[0].ChildNodes[0].ChildNodes[0].ChildNodes[0].Value.Trim()
                            };

                            album.Artists.Add(artist);
                            break;
                        case "release-group":
                            foreach (XmlElement releaseGroup in releaseElement)
                            {
                                switch (releaseGroup.Name.Trim().ToLowerInvariant())
                                {
                                    case "primary-type":
                                        album.PrimaryType = (ReleaseType)Enum.Parse(typeof(ReleaseType), releaseGroup.InnerText.Trim());
                                        break;
                                    case "secondary-type-list":
                                        album.SecondaryType = (ReleaseType)Enum.Parse(typeof(ReleaseType), releaseGroup.ChildNodes[0].InnerText.Trim());
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case "date":
                            try
                            {
                                album.ReleasedDate = DateTime.Parse(release.InnerText);
                            }
                            catch { }
                            break;
                        case "country":
                            album.Country = releaseElement.InnerText.Trim();
                            break;
                        case "medium-list":
                            foreach (XmlElement mediumList in releaseElement.ChildNodes)
                            {
                                if (mediumList.Name.Trim().Equals("track-count", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    album.TrackCount = Convert.ToByte(mediumList.InnerText);
                                }
                                else if (mediumList.Name.Trim().Equals("medium", StringComparison.InvariantCultureIgnoreCase))
                                {
                                    foreach (XmlElement medium in mediumList.ChildNodes)
                                    {
                                        switch (medium.Name.Trim().ToLowerInvariant())
                                        {
                                            case "format":
                                                album.Format = medium.InnerText.Trim();
                                                break;
                                            case "track-list":

                                                XmlReader reader = XmlReader.Create(new StringReader(medium.InnerXml));
                                                XmlSerializer ser = new XmlSerializer(typeof(Track));

                                                Track track = (Track)ser.Deserialize(reader);

                                                album.Tracks.Add(track);

                                                reader.Close();
                                                break;
                                            default:
                                                break;
                                        }
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }

                song.Albums.Add(album);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="artist"></param>
        /// <returns></returns>
        private static Artist ArtistSetup(XmlElement artist)
        {
            Artist songArtist = new Artist(Guid.Parse(artist.ChildNodes[0].ChildNodes[0].Attributes["id"].Value))
            {
                ArtistName = artist.ChildNodes[0].ChildNodes[0].ChildNodes[0].InnerText.Trim()
            };
            return songArtist;
        }

        #endregion
    }
}