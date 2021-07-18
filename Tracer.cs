using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace TaskKiller
{        
         public class Tracer
        {
            private string logPath = "log.txt";
            private StreamWriter stream;

            public void Initialize()
            {
                if (stream != null)
                {
                    stream.Dispose();
                }

                stream = new StreamWriter(logPath, true);

                Info();
                Info("Tracer is ready");
            }

            public void Close()
            {
                Info("Tracer is closing");

                stream.Flush();
                stream.Close();
                stream.Dispose();
                stream = null;
            }

            public void Info(string message = "")
            {
                Log("Info", message);
            }

            public void Warning(string message = "")
            {
                Log("Warning", message);
            }

            private void Log(string level, string message)
            {
                if (string.IsNullOrEmpty(message))
                {
                    stream.WriteLine();
                }
                else
                {
                    var format = "{0} [{1}]: {2}";
                    var time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    var finalLine = string.Format(format, time, level, message);

                    stream.WriteLine(finalLine);
                }
            }
        }   
}
