﻿
using System;
using System.Collections.Generic;
using System.IO;
using NLog;

class Program
{
    // Setting up NLog logger
    private static NLog.Logger logger = LogManager.LoadConfiguration(Directory.GetCurrentDirectory() + "\\nlog.config").GetCurrentClassLogger();

    static void Main(string[] args)
    {
        try
        {
            //scrubbing the moviefile
            string scrubbedFile = FileScrubber.ScrubMovies("movies.csv");
            logger.Info(scrubbedFile);
            MovieFile movieFile = new MovieFile(scrubbedFile);

            // Main loop for user interaction
            bool exit = false;
            while (!exit)
            {
                Console.WriteLine("Movie Display:");
                Console.WriteLine("Add Movie:");
                Console.WriteLine("1) Add Movie");
                Console.WriteLine("2) Display All Movies");
                Console.WriteLine("Enter to quit");
                Console.Write("Enter your choice: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        AddMovie(movieFile);
                        break;
                    case "2":
                        DisplayAllMovies(movieFile);
                        break;
                    case "":
                        exit = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice. Please try again.");
                        break;
                }
            }
        }
        catch (Exception ex)
        {
            logger.Error(ex.Message);
        }
        finally
        {
            logger.Info("Program ended");
        }
    }

    // Method to add a new movie
    static void AddMovie(MovieFile movieFile)
    {
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "|INFO|MediaLibrary.Program|User choice: \"1\"");
        Console.Write("Enter movie title: ");
        string title = Console.ReadLine();

        List<string> genres = new List<string>();
        while (true)
        {
            Console.Write("Enter genre (or done to quit): ");
            string genre = Console.ReadLine();
            if (genre.ToLower() == "done")
                break;
            genres.Add(genre);
        }

        Console.Write("Enter movie director: ");
        string director = Console.ReadLine();

        Console.Write("Enter running time (h:m:s): ");
        string runtimeInput = Console.ReadLine();
        TimeSpan runtime = TimeSpan.Parse(runtimeInput);

        // Creating a Movie object with user-provided details
        Movie movie = new Movie
        {
            title = title,
            director = director,
            runningTime = runtime,
            genres = genres
        };

        // Adding the movie to the movie file
        movieFile.AddMovie(movie);
        Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.ffff") + "|INFO|MediaLibrary.MovieFile|Media id " + movie.mediaId + " added");
    }

    // Method to display all movies
    static void DisplayAllMovies(MovieFile movieFile)
    {
        Console.WriteLine("All Movies:");
        foreach (var movie in movieFile.Movies)
        {
            Console.WriteLine(movie.Display());
        }
    }
}