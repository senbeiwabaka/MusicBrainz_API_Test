using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;

namespace MusicBrainzAPI.Domain
{
    public class BaseDomainObject
    {
        protected readonly string _baseURL = "http://musicbrainz.org/ws/2/";
    }
}
