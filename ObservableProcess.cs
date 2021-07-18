using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace TaskKiller
{
   
        class ObservableProcess
        {
            public DateTime Start;

            public ObservableProcess()
            {
                Start = DateTime.Now;
            }

            public bool ShouldBeKilled
            {
                get
                {
                    var deltaTime = DateTime.Now - Start;
                    return deltaTime.TotalMinutes > LifeLimit;
                }
            }

            public Process Target { get; set; }
            public int LifeLimit { get; set; }

            public void Kill()
            {
                try
                {
                    Target.Kill();
                }
                catch
                {
                    Console.WriteLine("is dead");
                }
            }
        }
}
