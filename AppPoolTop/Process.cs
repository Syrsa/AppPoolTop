using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace ConsoleApplication5
{
    class Process
    {
        string _instanceName;
        string _appPoolName;
        PerformanceCounter _cpuCounter;
        int _pid;
        double _nextValue = 0;

        public Process(string instance, int pid)
        {
            _instanceName = instance;
            _pid = pid;
            _cpuCounter = new PerformanceCounter("Process", "% Processor Time", _instanceName, true);
        }

        public int pid
        {
            get
            {
                return _pid;
            }
        }

        public string instanceName 
        {
            get
            {
                return _instanceName;
            }
        }

        public PerformanceCounter cpuCounter
        {
            get
            {
                return _cpuCounter;
            }
        }

        public string appPoolName
        {
            get
            {
                return _appPoolName;
            }
            set
            {
                _appPoolName = value;
            }
        }

        public void step()
        {
            _nextValue = cpuCounter.NextValue();
        }

        public double nextValue
        {
            get
            {
                return _nextValue;
            }
        }
    }
}
