using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.IO;
using System.Reflection;

namespace TaskKiller
{
    class Program
    {
        static void Main()
        {
            var tracer = new Tracer();

            tracer.Initialize();
            
            Console.WriteLine("Hello user! What do you want me to do?\n 1- Show all processes\n 2- Kill process\n 3- Exit");                                
            
            while (true)
            {
                Console.WriteLine("Enter your command:");
                string input = GetCommand();
                if (input == "3") break;

                if (input == "1")
                {
                    tracer.Info("The list of processes was shown");

                    foreach (Process process in Process.GetProcesses())
                    {
                        Console.WriteLine($"Name: {process.ProcessName}");
                    }
                }

                else if (input == "2")
                {                    
                    tracer.Info("Deleting procces");

                    Process[] AllProcesses = Process.GetProcesses();

                    Console.WriteLine("Please, enter the name of the process:");
                    string ProcName = Console.ReadLine();

                    Console.WriteLine("Input the timelimit for process living, in minutes:");
                    double LifeTime = double.Parse(Console.ReadLine());

                    Process TargetProc = AllProcesses.First(a => a.ProcessName == ProcName);

                    if ((DateTime.Now - TargetProc.StartTime).TotalMinutes >= LifeTime)
                    {
                        Console.WriteLine("You're wellcome ^_^");
                        TargetProc.Kill();                       

                        tracer.Info($"{TargetProc.ProcessName} was killed");
                        
                    }
                    else
                    {
                        Console.WriteLine("This process doesn't exist enough time to be closed. Do you want to set smart timer? Y/N");
                        string answer = Console.ReadLine().ToUpper();
                        if (answer == "Y")
                        {                            
                            tracer.Info("Smart timer is started");

                            Console.WriteLine("Input the value, in minutes, how often the timer will check the process:");
                            int timerSet = int.Parse(Console.ReadLine());
                            int interval = 60000 * timerSet;
                            Console.WriteLine("How long, in minutes, should the process live:");
                            int lifeLimit = int.Parse(Console.ReadLine());

                            var target = new ObservableProcess()
                            {
                                LifeLimit = lifeLimit,
                                Target = TargetProc
                               
                            };

                            var callBack = new TimerCallback(_ =>
                            {
                                Console.WriteLine($"{DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss")} {TargetProc.ProcessName} is still running...");

                                if (target.ShouldBeKilled)
                                {                                 
                                    tracer.Info($"The process {TargetProc.ProcessName} was killed in {lifeLimit} minutes with checking every {timerSet} minutes");
                                    
                                    target.Kill(); Environment.Exit(0);
                                }                                
                            });

                            Timer timer = new Timer(callBack, null, interval, interval);                            
                        }
                    }
                   
                }
            }

            tracer.Close();
        }

        static string GetCommand()
        {
            var validCommand = new string[]
            {
                "1", "2", "3"
            };

            while (true)
            {
                var input = Console.ReadLine();

                if (validCommand.Contains(input))
                {
                    return input;
                }

                Console.WriteLine($"Incorect command: {input}");
            }
        }       
        
    }
}



