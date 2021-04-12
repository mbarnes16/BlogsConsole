using System;
using System.IO;
namespace BlogsConsole
{
    public class Display
    {
        public void menu() 
        {
        Console.WriteLine("1) Display all blogs");
        Console.WriteLine("2) Add blog");
        Console.WriteLine("3) Create post");
        Console.WriteLine("4) Display posts");
        Console.WriteLine("5) Exit program");
        }
        public void numberOfBlogs (int numberBlogs) {
            if (numberBlogs == 1)
                Console.WriteLine($"1 blog in the database:");
            else if (numberBlogs > 1)
                Console.WriteLine($"{numberBlogs} blogs in the database:");
            else
                Console.WriteLine($"0 blogs in the database.");
        }
        
    }
}