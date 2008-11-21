using System;
using System.Collections.Generic;
using System.Text;
using OSGeo.FDO.Commands;
using System.IO;

namespace FdoToolbox.Core.AppFramework
{
    public abstract class ConsoleApplication : IDisposable
    {
        protected IConsoleCommand _Command;

        public bool Ask(string question)
        {
            Console.WriteLine("{0} [y/n]?", question);
            ConsoleKeyInfo input = Console.ReadKey();
            while (input.Key != ConsoleKey.Y && input.Key != ConsoleKey.N)
            {
                Console.WriteLine("Unknown response. Try again.");
                Console.WriteLine("{0} [y/n]?", question);
            }
            return input.Key == ConsoleKey.Y;
        }

        /// <summary>
        /// Throws an ArgumentException if the given parameter value is empty
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parameter"></param>
        protected static void ThrowIfEmpty(string value, string parameter)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("Missing required parameter: " + parameter);
        }

        /// <summary>
        /// Parse application-specific arguments.
        /// </summary>
        /// <param name="args">The array of commandline arguments</param>
        public abstract void ParseArguments(string[] args);

        /// <summary>
        /// Display usage information for this application
        /// </summary>
        public abstract void ShowUsage();

        /// <summary>
        /// Run the application
        /// </summary>
        /// <param name="args">The array of commandline arguments</param>
        public virtual void Run(string[] args)
        {
            try
            {
                ParseArguments(args);
            }
            catch (ArgumentException ex)
            {
                Console.Error.WriteLine(ex.Message);
                ShowUsage();
                return;
            }

#if DEBUG
            if (_Command != null)
                Console.WriteLine("Silent: {0}\nTest: {1}", _Command.IsSilent, _Command.IsTestOnly);
#endif

            int retCode = (int)CommandStatus.E_OK;
            if (_Command != null)
            {
                try
                {
                    retCode = _Command.Execute();
                }
                catch (Exception ex)
                {
                    Console.Error.WriteLine(ex.ToString());
                    retCode = (int)CommandStatus.E_FAIL_UNKNOWN;
                }
            }
#if DEBUG
            Console.WriteLine("Status: {0}", retCode);
#endif
            System.Environment.ExitCode = retCode;
        }

        /// <summary>
        /// Verifies the file name exists.
        /// </summary>
        /// <param name="fileName"></param>
        /// <returns>The file name.</returns>
        protected string CheckFile(string fileName)
        {
            string file = fileName;
            if (!File.Exists(file))
            {
                if (!Path.IsPathRooted(file))
                    file = Path.Combine(this.AppPath, file);

                if (!File.Exists(file))
                    throw new ArgumentException("Unable to find file: " + fileName);
            }
            return file;
        }

        /// <summary>
        /// Current working directory path of the application
        /// </summary>
        public string AppPath
        {
            get
            {
                return AppDomain.CurrentDomain.BaseDirectory;
            }
        }

        private bool _IsTest;

        /// <summary>
        /// Is this application being run in simulation mode?
        /// </summary>
        public bool IsTestOnly
        {
            get { return _IsTest; }
            set { _IsTest = value; }
        }

        private bool _IsSilent;

        /// <summary>
        /// Is this application running silent? (no console output)
        /// </summary>
        public bool IsSilent
        {
            get { return _IsSilent; }
            set { _IsSilent = value; }
        }


        /// <summary>
        /// Gets an argument value with the given prefix. Arguments follow the
        /// convention [prefix]:[value]
        /// </summary>
        /// <param name="prefix"></param>
        /// <param name="args"></param>
        /// <returns>The argument value if found, otherwise null</returns>
        protected static string GetArgument(string prefix, string[] args)
        {
            if (args.Length == 0)
                return null;

            foreach (string arg in args)
            {
                if (arg.StartsWith(prefix))
                {
                    string argument = arg.Substring(arg.IndexOf(":") + 1);
                    return argument;
                }
            }
            return null;
        }

        /// <summary>
        /// Checks if a given parameter switch was defined
        /// </summary>
        /// <param name="strSwitch"></param>
        /// <returns></returns>
        protected static bool IsSwitchDefined(string strSwitch, string[] args)
        {
            if (args.Length == 0)
                return false;

            foreach (string arg in args)
            {
                if (arg == strSwitch || arg.StartsWith(strSwitch))
                {
                    return true;
                }
            }
            return false;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            
        }
    }
}
