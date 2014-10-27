using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace CreditCardValidation.Tests
{
    class InstrumentsRunner
    {
        static string[] GetInstrumentsOutput()
        {
            const string cmd = "/usr/bin/instruments";

            var startInfo = new ProcessStartInfo
            {
                FileName = cmd,
                Arguments = "-s devices",
                RedirectStandardOutput = true,
                UseShellExecute = false
            };

            Process proc = new Process();
            proc.StartInfo = startInfo;
            proc.Start();
            var result = proc.StandardOutput.ReadToEnd();
            proc.WaitForExit();

            var lines = result.Split('\n');
            return lines;
        }

        public Simulator[] GetListOfSimulators()
        {
            var simulators = new List<Simulator>();
            var lines = GetInstrumentsOutput();

            foreach (var line in lines)
            {
                var sim = new Simulator(line);
                if (sim.IsValid())
                {
                    simulators.Add(sim);
                }
            }

            return simulators.ToArray();
        }
    }
    
}
