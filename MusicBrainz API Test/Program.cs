using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MusicBrainzAPI;
using MusicBrainzAPI.Domain;
using MusicBrainzAPI.AudioObjects;

namespace MusicBrainz_API_Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Recording recording = new Recording();

            System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

            sw.Start();

            recording.SearchGet(string.Empty, "\"right side of the bed\"");

            sw.Stop();

            Console.WriteLine("Elapsed time in milliseconds: {0} ; For a total count of {1}", sw.ElapsedMilliseconds, recording.Songs.Count);

            foreach (Song song in recording.Songs)
            {
                Console.WriteLine(song.ToString());

                Console.WriteLine("--------------------------------------");

                foreach (Album album in song.Albums)
                {
                    Console.WriteLine("\t" + album.ToString());
                }
            }

            Console.ReadLine();
        }
    }
}
