using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SQLite;
using Stitcher;

namespace StitchWitch.Data
{
    public class ThreadRepository
    {
        private static ThreadRepository instance;
        private const string PathToThreadData = "Data/ReferenceData/Threads.sqlite";

        public List<Thread> Threads { get; private set; }

        protected ThreadRepository()
        {
            Threads = new List<Thread>();
        }

        public Thread ByColor(Color color)
        {
            if (color.IsTransparent) throw new Exception("There is no such thing as a transparent thread.");

            return Threads.Single(t => t.HexCode == color.HexCode);
        }

        public static ThreadRepository Instance()
        {
            return instance ?? (instance = ReadThreadsFromDatabase());
        }

        private static ThreadRepository ReadThreadsFromDatabase()
        {
            var repository = new ThreadRepository();
            using (var connection = new SQLiteConnection(Path.GetFullPath(PathToThreadData), SQLiteOpenFlags.ReadOnly))
            {
                //repository.Threads = connection.Query<Thread>("SELECT * FROM Threads WHERE DMC = 'Blanc' OR DMC = '162'");

                repository.Threads = connection.Query<Thread>("SELECT * FROM Threads WHERE DMC = 'White' OR DMC = '166' OR DMC = '334' OR DMC = '327'");
            }
            return repository;
        }
    }
}
