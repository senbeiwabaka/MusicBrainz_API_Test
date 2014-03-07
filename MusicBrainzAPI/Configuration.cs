using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MusicBrainzAPI
{
    public static class Configuration
    {
        public static string BaseURL = "http://musicbrainz.org/ws/2/";
        public static string Release = "release/?query=";
        public static string Recording = "recording?query=";
    }
}
