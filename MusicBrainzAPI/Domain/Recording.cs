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
        public XmlDocument XmlDocument { get; private set; }
        public IList<string> Generes { get; set; }

        public Recording(string artist = "", string album = "", string songTitle = "")
            : this(Configuration.BaseURL, "test", "test", "test") { }

        public Recording(string baseURL = "", string artist = "", string album = "", string songTitle = "")
        {
            WebRequest request = WebRequest.Create(baseURL + Configuration.Recording + "\"" + songTitle + "\" AND artist:" + artist + " AND release:" + album);

            request.Method = "GET";

            HttpWebResponse response = request.GetResponse() as HttpWebResponse;

            if (response.StatusCode== HttpStatusCode.OK)
            {
                using (Stream stream = response.GetResponseStream())
                {
                    XmlDocument = new XmlDocument();
                    XmlDocument.Load(stream);
                }
            }
        }
    }
}
