using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicBrainzAPI;
using MusicBrainzAPI.Domain;

namespace MusicBrainz_API_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Recording recording = new Recording("Atreyu", "The Curse", "Right side of the bed");

            Console.WriteLine(recording.XmlDocument.OuterXml);
        }
    }
}
