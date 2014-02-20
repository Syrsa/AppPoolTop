using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Diagnostics;
using System.Threading;
using System.Management;
using System.Text.RegularExpressions;

namespace ConsoleApplication5
{
    class Program
    {
        static List<Process> counters = new List<Process>();
        static void Main(string[] args)
        {

            /// Weee new test.....

            PerformanceCounterCategory cat = new PerformanceCounterCategory("Process");
            string[] instances = cat.GetInstanceNames();
            foreach (string instance in instances)
            {

                using (PerformanceCounter cnt = new PerformanceCounter("Process",
                     "ID Process", instance, true))
                {
                    if (instance.StartsWith("w3wp"))
                    {
                        Process process = new Process(instance, (int)cnt.RawValue);
                        counters.Add(process);

                        Console.WriteLine(instance + ": " + ((int)cnt.RawValue));
                        int val = (int)cnt.RawValue;

                        WqlObjectQuery wqlQuery = new WqlObjectQuery("SELECT * from Win32_Process where ProcessId=" + val);
                        ManagementObjectSearcher searcher = new ManagementObjectSearcher(wqlQuery);

                        foreach (ManagementObject disk in searcher.Get())
                        {
                            Regex re = new Regex("-ap \"(.+)\"");

                            if (disk.GetPropertyValue("CommandLine") != null)
                            {
                                // Should work for IIS6
                                string commandLine = disk.GetPropertyValue("CommandLine").ToString();
                                process.appPoolName = re.Match(commandLine).Value.Substring(5);
                                process.appPoolName = process.appPoolName.Split('"')[0];
                            }
                        }
                    }
                }
            }

            int maxNameLen = 0;
            foreach (Process process in counters)
            {
                if (process.appPoolName.Length > maxNameLen)
                {
                    maxNameLen = process.appPoolName.Length;
                }
            }


            while (!Console.KeyAvailable)
            {
                foreach (Process myAppCpu in counters)
                {
                    myAppCpu.step();
                }

                Console.Clear();

                Console.Write("Name");

                for (int i = 0; i < maxNameLen - 4; i++)
                {
                    Console.Write(" ");
                }
                Console.WriteLine(" CPU");


                for (int i = 0; i < maxNameLen; i++)
                {
                    Console.Write("-");
                }
                Console.WriteLine(" ------");

                foreach (Process myAppCpu in counters.OrderByDescending(x => x.nextValue))
                {
                    Console.Write(myAppCpu.appPoolName);
                    for (int i = 0; i < maxNameLen - myAppCpu.appPoolName.Length ; i++)
			        {
			            Console.Write(" ");
			        }
                    Console.WriteLine(" " + Math.Round(myAppCpu.nextValue, 1));
                }
                Thread.Sleep(3000);
            }
        }
    }
}
