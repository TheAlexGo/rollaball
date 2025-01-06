using System;
using System.Diagnostics;

namespace GitIntegration
{
    public class Utils
    {
        public static string ExecuteGitWithParams(string param)
        {
            var processInfo = new ProcessStartInfo("git")
            {
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                CreateNoWindow = true
            };

            var process = new Process();
            process.StartInfo = processInfo;
            process.StartInfo.FileName = "git";
            process.StartInfo.Arguments = param;
            process.Start();
            process.WaitForExit();

            if (process.ExitCode != 0)
            {
                throw new Exception(process.StandardError.ReadLine());
            }
        
            return process.StandardOutput.ReadLine();
        }
    }
}