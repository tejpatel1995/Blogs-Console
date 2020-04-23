using NLog;
using BlogsConsole.Models;
using System;
using System.Linq;

namespace BlogsConsole
{
    class MainClass
    {
        private static NLog.Logger logger = NLog.LogManager.GetCurrentClassLogger();
        public static void Main(string[] args)
        {
            logger.Info("Program started");
            try
            {
                string answer;
                do
                {

                    Console.WriteLine("Select action: \n1. Display Blogs\n2. Add Blog\n3. Create Post\n4. Display Posts\nPress any other key to Quit");
                    answer = Console.ReadLine();
                    Console.Clear();
                    if (answer == "1")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.WriteLine("All blogs in the database:");
                        foreach (var item in query)
                        {
                            Console.WriteLine(item.Name);
                        }
                        Console.WriteLine();
                    }
                    if (answer == "2")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.Name);
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        bool valid = true;
                        foreach (var item in query)
                        {
                            if (name.Equals(item.Name))
                            {
                                valid = false;
                            }
                        }
                        if (name.Equals("") || valid == false)
                        {
                            logger.Error($"INVALID: {name} is already blog name or is null.");
                            Console.WriteLine($"INVALID: {name} is an invalid blog name or is null.");
                        }
                        else
                        {
                            var blog = new Blog { Name = name };
                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                        }
                    }
                    if (answer == "3")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Which blog would you like to enter a post in?");
                        int tempMax = 0;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}. {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int blogNum))
                        {
                            foreach (var item in query)
                            {
                                if (tempMax < item.BlogId)
                                {
                                    tempMax = item.BlogId;
                                }
                            }
                            if ((blogNum < tempMax) && (blogNum > 0))
                            {
                                Post post = new Post();
                                Console.WriteLine("Title: ");
                                post.Title = Console.ReadLine();
                                Console.WriteLine("Content: ");
                                post.Content = Console.ReadLine();
                                post.BlogId = blogNum;
                                db.AddPost(post);
                                logger.Info($"Post {post.Title} added in BlogId: {blogNum}");
                            }
                            else
                            {
                                Console.WriteLine("Invalid Blog Number");
                                logger.Error("Invalid Blog Number");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Blog is NaN");
                            logger.Error("Blog is Nan");
                        }
                    }
                    if (answer == "4")
                    {

                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Which blogs posts would you like to view?\n0. Posts from all blogs");
                        int tempMax = 0;
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}. Posts from blog: {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int blogNum))
                        {
                            foreach (var item in query)
                            {
                                if (tempMax < item.BlogId)
                                {
                                    tempMax = item.BlogId;
                                }
                            }
                            if (blogNum == 0)
                            {
                                foreach (var item in db.Posts.OrderBy(p => p.Title))
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                                }
                            }
                            else if ((blogNum < tempMax) && (blogNum > 0))
                            {
                                foreach (var item in db.Posts.Where(p => p.BlogId == blogNum).OrderBy(p => p.Title))
                                {
                                    Console.WriteLine($"Blog: {item.Blog.Name}\nTitle: {item.Title}\nContent: {item.Content}\n");
                                }
                            }
                            else
                            {
                                Console.WriteLine("Invalid Blog Number");
                                logger.Error("Invalid Blog Number");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Blog is NaN");
                            logger.Error("Blog is Nan");
                        }
                    }
                } while (answer == "1" || answer == "2" || answer == "3" || answer == "4");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
