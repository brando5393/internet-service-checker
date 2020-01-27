using System;
using System.IO;
using System.Net.NetworkInformation;
//using System.Timers;

namespace internet_service_checker
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Internet Service Checker";
            

            Console.Write("===Welcome to the Internet Service Checker===");
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("===MENU===");
            Console.WriteLine("");
            Console.WriteLine("1. Initialize Checker");
            Console.WriteLine("2. About This Program");
            Console.WriteLine("3. Exit");
            Console.WriteLine("");
            Console.Write("Please enter a number: ");
            string ans = Console.ReadLine();

            switch (ans)
            {
                case "1":
                    // Console.Write("selected option 1");
                    checkService();
                    break;

                case "2":
                    Console.Write("selected option 2");
                    // method call here
                    break;

                case "3":
                    Environment.Exit(0);
                    break;

                default:
                    Console.Write("Please enter a valid number.");
                    break;   
            }
        }
        public static void checkService()
        {
            var currentDate = DateTime.Now.ToString("ddd M\\-dd\\-yy");
            var currentTime = DateTime.Now.ToString("hh:mm tt");
            int pingNum = 0;
            string fileName = @"C:\internet-service-log " + currentDate + ".txt";
            string exitMessage = "Press ESC to exit to main menu.";
            string successMessage = "#PING " + pingNum + ": Status Return = SUCCESSFULL / Time Recorded = [" + currentTime + "]";
            string failMessage = "#PING " + pingNum + ": Status Return = FAILURE / Time Recorded = [" + currentTime + "]";
            
            // Check if a file with the same name exists if so, delete it
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            // Create log file
            Console.WriteLine("");
            Console.WriteLine("===STARTED CHECKING INTERNET SERVICE===");
            Console.WriteLine("");
            Console.WriteLine("Log file created in C drive");
            using (StreamWriter recorder = new StreamWriter(fileName))
            {
                recorder.WriteLine("===INTERNET SERVICE LOG FOR " + currentDate + "===");
                recorder.WriteLine("");
                Ping serviceChecker = new Ping();
                
                while (!(Console.KeyAvailable && Console.ReadKey(true).Key == ConsoleKey.Escape))
                {
                    // create timer to send ping to google every 5 minutes
                    var mainTimer = new System.Timers.Timer();
                    mainTimer.Interval = 300000;
                    mainTimer.Elapsed += onTimedEvent;
                    mainTimer.AutoReset = true;
                    mainTimer.Enabled = true;
                      void onTimedEvent(object source, System.Timers.ElapsedEventArgs elapsedEventArgs)
                    {
                        try
                        {
                            Console.WriteLine("!Sending ping attempt to google.com");
                            PingReply reply = serviceChecker.Send("172.217.1.132", 1000);
                            if (reply != null)
                            {
                                pingNum++;
                                Console.WriteLine(successMessage);
                                recorder.WriteLine(successMessage);
                                Console.WriteLine(exitMessage);
                            }
                            else
                            {
                                pingNum++;
                                Console.WriteLine(failMessage);
                                recorder.WriteLine(failMessage);
                                Console.WriteLine(exitMessage);
                            }
                        }
                        catch
                        {
                            Console.WriteLine("!ERROR ENCOUNTERED WHILE ATTEMPTING TO CONTACT GOOGLE SERVER");
                            Console.WriteLine("!PRESS ANY KEY TO RETURN TO THE MAIN MENU");
                            Console.ReadKey();
                            Environment.Exit(0);

                        }
                        
                       

                    }
                    
                    
                    
                }
                
                
                
                    
                
            }
            
        }
    }
}
