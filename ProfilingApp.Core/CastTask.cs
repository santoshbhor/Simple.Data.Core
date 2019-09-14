using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using ProfilingApp.Resources;
using Simple.Data;


namespace ProfilingApp
{
    public class CastTask : IProfileTask
    {
        private readonly dynamic _db = Database.OpenConnection(Settings.Default.ConnectionString);

        public void Run()
        {
            var watch = Stopwatch.StartNew();

            //Dynamic
            var dposts = _db["Post"].All().ToList();
            Console.WriteLine("Total Posts (dynamic): {0}", dposts.Count);
            watch.Stop();
            Console.WriteLine("(dynamic): {0} ms", watch.Elapsed.Milliseconds);

            //Typed Model test
            List<Post> posts = _db.Posts.All().ToList<Post>();
            Console.WriteLine("Total Posts (Typed Model): {0}", posts.Count);
            watch.Stop();
            Console.WriteLine("(Typed Model): {0} ms", watch.Elapsed.Milliseconds );
        }
    }

    public class Post
    {
        public int ID { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public DateTime Created { get; set; }
    }
}