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
            Recording recording = new Recording();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            recording.Get(string.Empty, "Atreyu", "The Curse", "Right side of the bed");

            sw.Stop();

            Console.WriteLine("Elapsed time in milliseconds: {0}", sw.ElapsedMilliseconds);

            Console.ReadLine();
        }
    }
}
