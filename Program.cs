using System;
using NLog.Web;
using System.IO;
using System.Linq;

namespace BlogsConsole
{
    class Program
    {
        // create static instance of Logger
        private static NLog.Logger logger = NLogBuilder.ConfigureNLog(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();
        static void Main(string[] args)
        {
                  logger.Info("Program started");

            try
            {
                Display display = new Display();
                string menuChoice;
                var db = new BloggingContext();

                do {
                    // display menu
                    display.menu();
                    menuChoice = Console.ReadLine();
                    Console.WriteLine("");
                    var query = db.Blogs.OrderBy(b => b.BlogId);
                    var postQuery = db.Posts.OrderBy(p => p.PostId);
                    string input;
                    int blogIdChoice;
                    bool repeat;
            
                    switch(menuChoice) 
                    {
                        case "1":
                            int maxBlogNumber = db.Blogs.Max(b => b.BlogId);
                            display.numberOfBlogs(maxBlogNumber);

                            foreach (var item in query)
                            {
                                Console.WriteLine($"{item.Name}");
                            }
                            break;

                        case "2":
                            // Create and save a new Blog
                            string name;
                            do {    
                                Console.Write("Enter a name for a new Blog: ");
                                name = Console.ReadLine();
                                if (name == "") 
                                {
                                    Console.WriteLine("The blog name cannot be blank.\n");
                                }
                            } while (name == "");

                            var blog = new Blog { Name = name };

                            db.AddBlog(blog);
                            logger.Info("Blog added - {name}", name);
                            break;

                        case "3":
                            
                            if (query == null) 
                            {
                                Console.WriteLine("There are no blogs.");
                                Console.WriteLine("Choose option 2 to add a blog.\n");
                            }
                            
                            else {
                                
                                do {
                                    // prompt user to select blog 
                                    Console.WriteLine("Select a blog to create a post in");
                                    
                                    // if query is not null
                                    foreach (var item in query)
                                    {
                                        Console.WriteLine($"{item.BlogId}) {item.Name}");

                                    }
                                    input = Console.ReadLine();
                                    try {
                                        blogIdChoice = int.Parse(input);
                                        repeat = false;
                                    }
                                    // error: not an integer
                                    catch {
                                        Console.WriteLine("\nInvalid choice.");
                                        Console.WriteLine("Please enter the number of one of the blogs.\n");
                                        blogIdChoice = -99;
                                        repeat = true;
                                    }
                                    // error: integer is out of range
                                    maxBlogNumber = db.Blogs.Max(b => b.BlogId);
                                    if ((blogIdChoice < 1 || blogIdChoice > maxBlogNumber) && blogIdChoice != -99) { 
                                        Console.WriteLine("\nInvalid choice.");
                                        Console.WriteLine("Please enter the number of one of the blogs.\n");
                                        repeat = true;
                                    }
                                } while (repeat == true); // repeat loop if either error occurs (not an integer or out of range)
                                Post post = new Post();
                                post.BlogId = blogIdChoice;
                                Console.WriteLine("Enter the title of the post: ");
                                post.Title = Console.ReadLine();
                                Console.WriteLine("Enter the content of the post: ");
                                post.Content = Console.ReadLine();
                                db.AddPost(post);
                                logger.Info("Post added - {name}", post.Title);
                            }

                            break;

                        case "4":
                             if(db.Blogs.Count() != 0)
                    {
                        int blogId = 0; 
                        Console.WriteLine("Select the blog's posts to display:");
                        var blogs = db.Blogs.OrderBy(b => b.BlogId);
                        Console.WriteLine("0) Posts from all blogs");
                        foreach(var item in blogs)
                        { 
                            Console.WriteLine(item.BlogId + ")" + " Posts from " + item.Name); 
                        }
                        try
                        {
                            blogId = Int32.Parse(Console.ReadLine());
                            if(blogId > db.Blogs.Count())
                            { 
                                logger.Error("There are no Blogs saved with that Id"); 
                            }
                            else
                            {
                                if(blogId == 0)
                                {
                                    Console.WriteLine(db.Posts.Count() + " post(s) returned");
                                    foreach(var item in db.Posts)
                                    { 
                                        Console.WriteLine("Blog: " + item.Blog.Name + "\nTitle: " + item.Title + "\nContent: " + item.Content); 
                                    }
                                }
                                else
                                {  
                                    var posts = db.Posts.Where(p => p.BlogId == blogId);
                                    Console.WriteLine(posts.Count() + " post(s) returned");
                                    foreach(var item in posts)
                                    { 
                                        Console.WriteLine("Blog: " + item.Blog.Name + "\nTitle: " + item.Title + "\nContent: " + item.Content); 
                                    }
                                }
                            }
                        }catch(FormatException)
                        { 
                            logger.Error("Invalid Blog Id"); 
                        }
                    }
                    else
                    { 
                        Console.WriteLine(db.Blogs.Count() + " Blogs returned"); 
                    }
                    break;
        
                        case "5":
                            logger.Info("Program Ended");
                            Environment.Exit(0);
                            break;

                        default:
                            display.menu();
                            break;
                    }
                } while(menuChoice != "5");
            }
            
            catch (Exception ex)
            {
                logger.Error(ex.Message);
            }
            logger.Info("Program ended");
        }
    }
}

