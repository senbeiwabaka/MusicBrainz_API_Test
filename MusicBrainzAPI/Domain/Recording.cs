using MusicBrainzAPI.AudioObjects;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Xml;

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
                        case "release-group":

                            break;
                        case "date":
                            //album.ReleasedDate = DateTime.Parse(release.InnerText);
                            break;
                        case "country":
                            album.Country = releaseElement.InnerText;
                            break;
                        case "medium-list":
                            break;
                        default:
                            break;
                    }
                }

                if (!string.IsNullOrEmpty(release.ChildNodes[1].InnerText))
                {
                    
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
