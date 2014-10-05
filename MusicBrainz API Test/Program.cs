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

            recording.SearchGet(string.Empty, "\"right side of the bed\" AND artist:atreyu AND release:the curse");

            int i = 0;

            long time = 0;

            while (i < 10)
            {
                System.Diagnostics.Stopwatch sw = new System.Diagnostics.Stopwatch();

                sw.Start();

                Recording r = new Recording();

                r.SearchGet(string.Empty, "\"right side of the bed\" AND artist:atreyu AND release:the curse");

                sw.Stop();

                time += sw.ElapsedMilliseconds;

                ++i;
            }

            Console.WriteLine("Elapsed time in milliseconds: {0} ; For a total count of {1}", time / i, recording.Songs.Count);

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
