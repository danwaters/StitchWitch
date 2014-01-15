namespace StitchWitch.Data
{
    public class ThreadRepository
    {
        private static ThreadRepository instance;
        private readonly string pathToData = "ReferenceData/Threads.sqlite";

        protected ThreadRepository()
        {
            
        }

        public static ThreadRepository Instance()
        {
            if (instance == null)
            {
                instance = ReadThreadsFromDatabase();
            }
        }

        private static ThreadRepository ReadThreadsFromDatabase()
        {

        }
    }
}
