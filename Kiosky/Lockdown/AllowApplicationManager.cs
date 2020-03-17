using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Kiosky.Lockdown
{
    class AllowApplicationManager : IApplicationManager
    {
        SortedSet<string> _allowedPrograms;
        string[] alwaysAllowedPrograms = new string[] { "WindowsInternal.ComposableShell.Experiences.TextInput.InputApp" };
        public void LoadPrograms(string[] programs)
        {
            //TODO: Handle loading a second time?
            foreach(string p in programs)
                _allowedPrograms.Add(p);

            foreach (string p in alwaysAllowedPrograms)
                _allowedPrograms.Add(p);
        }
        /// <summary>
        /// Closes all programs that are not allowed
        /// </summary>
        public void CloseNotAllowedPrograms()
        {
            var currentProcess = Process.GetCurrentProcess();
            foreach (Process p in Process.GetProcesses())
            {
                //Verify if the process is on the allowed list
                if (!_allowedPrograms.Contains(p.ProcessName))
                {
                    //Check if its this process and if it has a window
                    if (p.Id != currentProcess.Id && !p.MainWindowHandle.Equals((IntPtr)0) && p.Responding)
                    {

                        //Check if the window is visisble?

                        try
                        {
                            p.Kill();
                        }
                        catch (Exception e)
                        {
                            Logger.DefaultLogger.LogWarning(String.Format("Failed to kill {0}", p.MainModule.ModuleName));
                        }
                    }
                }
            }
        }

        ~AllowApplicationManager()
        {

        }
    }
}
