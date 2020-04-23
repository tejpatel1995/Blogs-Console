﻿using NLog;
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

                    Console.WriteLine("Select action: \n1. Display Blogs\n2. Add Blog\n3. Create Post\nPress any other key to Quit");
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
                        Console.Write("Enter a name for a new Blog: ");
                        var name = Console.ReadLine();
                        var blog = new Blog { Name = name };
                        db.AddBlog(blog);
                        logger.Info("Blog added - {name}", name);
                    }
                    if (answer == "3")
                    {
                        var db = new BloggingContext();
                        var query = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("Which blog would you like to enter a post in?");
                        foreach (var item in query)
                        {
                            Console.WriteLine($"{item.BlogId}. {item.Name}");
                        }
                        if (int.TryParse(Console.ReadLine(), out int blogNum))
                        {
                            Post post = new Post();
                            Console.WriteLine("Title: ");
                            post.Title = Console.ReadLine();
                            Console.WriteLine("Content: ");
                            post.Content = Console.ReadLine();
                            post.BlogId = blogNum;
                            logger.Info($"Post {post.Title} added in BlogId: {blogNum}");
                        }
                        else
                        {
                            Console.WriteLine("Invalid Blog Number");
                            logger.Error("Invalid Blog Number");
                        }
                    }
                } while (answer == "1" || answer == "2" || answer == "3");
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}
