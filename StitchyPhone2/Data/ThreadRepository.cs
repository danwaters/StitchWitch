using System.Collections.Generic;
using System.IO;
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

        public static ThreadRepository Instance()
        {
            return instance ?? (instance = ReadThreadsFromDatabase());
        }

        private static ThreadRepository ReadThreadsFromDatabase()
        {
            var repository = new ThreadRepository();
            using (var connection = new SQLiteConnection(Path.GetFullPath(PathToThreadData), SQLiteOpenFlags.ReadOnly))
            {
                repository.Threads = connection.Query<Thread>("SELECT * FROM Threads");
            }
            return repository;
        }
    }
}
