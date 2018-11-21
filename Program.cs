using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Dapper;
using Npgsql;

namespace npgsql_test
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                var tracks = Enumerable.Range(0, 30).Select(i => new TrackDto { Name = "track " + i }).ToList();
                Insert(tracks);

                var currentProcess = System.Diagnostics.Process.GetCurrentProcess();
                long totalBytesOfMemoryUsed = currentProcess.WorkingSet64;
                Console.WriteLine(totalBytesOfMemoryUsed / 1000000);

                Thread.Sleep(5); // If this number is considerably bigger (e.g. 500), we don't see the memory increase
            }
        }

        private static void Insert(List<TrackDto> tracks)
        {
            using (var connection = new NpgsqlConnection("Server=localhost;Database=postgres;Uid=postgres;Password=npgsql_tests"))
            {
                connection.Open();
                connection.TypeMapper.MapComposite<TrackDto>("track");
            }
        }
    }

    class TrackDto
    {
        public string Name { get; set; }
    }
}
