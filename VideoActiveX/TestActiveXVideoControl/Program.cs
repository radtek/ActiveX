﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace TakePicture
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }
/*
        //static void Main(string[] args)
        static int Main()
        {
            // Get command line arguments.
            string[] args = Environment.GetCommandLineArgs();

            // If no arguments are passed then return.
            if (args.Length < 2)
            {
                return 0;
            }

            // Get the file name to run.
            string fileToRun = args[1];

            // Compile command line for the file to run.
            var cmdLine = new StringBuilder();

            for (int i = 2; i < args.Length; i++)
            {
                cmdLine.AppendFormat(args.Contains(" ") ? "\"{0}\" " : "{0} ", args[i]);
            }

            // Execute the external file.
            var process = Process.Start(fileToRun, cmdLine.ToString());
            // Wait the process to complete.
            process.WaitForExit();
            return process.ExitCode;
        }
*/



    }
}
