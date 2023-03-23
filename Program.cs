using System;
using System.IO;
using System.Net.NetworkInformation;
using System.Threading;

namespace internet_service_checker
{
    class Program
    {
        static void Main(string[] args)
        {
            // Set the console title
            Console.Title = "Internet Service Checker";

            // Display the welcome message and menu options
            Console.WriteLine("===Welcome to the Internet Service Checker===");
            Console.WriteLine();
            Console.WriteLine("===MENU===");
            Console.WriteLine();
            Console.WriteLine("1. Initialize Checker");
            Console.WriteLine("2. About This Program");
            Console.WriteLine("3. Exit");
            Console.WriteLine();
            Console.Write("Please enter a number: ");
            string ans = Console.ReadLine();

            // Process user input using a switch statement
            switch (ans)
            {
                case "1":
                    CheckService();
                    break;
                case "2":
                    // Display information about the program and return to the main menu
                    Console.WriteLine("This program checks internet service by sending ping requests to a target IP address.");
                    Console.WriteLine("The program will continue sending ping requests until you choose to exit.");
                    Console.WriteLine();
                    Console.WriteLine("Press any key to return to the main menu.");
                    Console.ReadKey();
                    Main(args);
                    break;
                case "3":
                    // Exit the program
                    Environment.Exit(0);
                    break;
                default:
                    // Display an error message for invalid input and return to the main menu
                    Console.WriteLine("Please enter a valid number.");
                    Main(args);
                    break;
            }
        }

        static void CheckService()
        {
            var userHomeDirectory = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            // Get the current date to use in the log file name
            var currentDate = DateTime.Now.ToString("ddd M\\-dd\\-yy");
            // Initialize a variable to keep track of the number of ping attempts
            int pingNum = 0;
            // Set the file name for the log file
            string fileName = Path.Combine(userHomeDirectory, $"internet-service-log-{currentDate}.txt");
            // Set a message to display when exiting to the main menu
            string exitMessage = "Press ESC to exit to main menu.";
            // Set the target IP address for the ping requests (Google DNS server)
            string targetIpAddress = "8.8.8.8";

            // If a log file with the same name already exists, delete it
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }

            // Display a message indicating that the program is checking internet service
            Console.WriteLine();
            Console.WriteLine("===STARTED CHECKING INTERNET SERVICE===");
            Console.WriteLine();
            // Display the file name for the log file
            Console.WriteLine($"Log file created in current user's home directory: {fileName}");
            // Create a StreamWriter object to write to the log file
            using (StreamWriter recorder = new StreamWriter(fileName))
            {
                // Write a header to the log file with the current date
                recorder.WriteLine($"===INTERNET SERVICE LOG FOR {currentDate}===");
                recorder.WriteLine();

                // Create a Ping object to send ping requests
                var pingSender = new Ping();

                // Set the ping options to prevent fragmentation
                var pingOptions = new PingOptions
                {
                    DontFragment = true
                };

                // Set the buffer size and timeout for the ping requests
                var buffer = new byte[32];
                var timeout = 1000;

                // Keep sending ping requests until the user chooses to exit
                while (true)
                {
                    // Increment the ping attempt number
                    pingNum++;

                    // Send a ping request to the target IP address
                    var pingReply = pingSender.Send(targetIpAddress, timeout, buffer, pingOptions);

                    // Write the result of the ping request to the log file
                    recorder.WriteLine($"Ping attempt {pingNum} at {DateTime.Now}: {pingReply.Status}");
                    recorder.Flush(); // Flush the buffer to ensure data is written to the file

                    // Display the result of the ping request to the console
                    Console.WriteLine($"Ping attempt {pingNum} at {DateTime.Now}: {pingReply.Status}");

                    // Check if the user has pressed the ESC key to exit to the main menu
                    if (Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape)
                    {
                        // Display a message indicating that the program is exiting to the main menu
                        Console.WriteLine();
                        Console.WriteLine(exitMessage);
                        Console.WriteLine();

                        // Return to the main menu
                        Main(new string[] { });
                    }

                    // Wait for 5 minutes before sending the next ping request
                    Thread.Sleep(300000);
                }
            }
        }
    }
}