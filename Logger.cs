using System;
using System.IO;

namespace Lesson
{
    interface ILogger
    {
        void WriteError(string message);
    }

    class Program
    {
        private const string Message = "ErrorMessage";

        static void Main(string[] args)
        {
            Pathfinder pathfinderToFile = new Pathfinder(new FileLogWritter());
            pathfinderToFile.Find(Message);

            Pathfinder pathfinderToConsole = new Pathfinder(new ConsoleLogWritter());
            pathfinderToConsole.Find(Message);

            Pathfinder pathfinderToSecureFile = new Pathfinder(new SecureFileLogWritter());
            pathfinderToSecureFile.Find(Message);

            Pathfinder pathfinderToSecureConsole = new Pathfinder(new SecureConsoleLogWritter());
            pathfinderToSecureConsole.Find(Message);

            Pathfinder pathfinderToConsoleAndToSecureFile = new Pathfinder(new ConsoleLogWritter(new SecureFileLogWritter()));
            pathfinderToConsoleAndToSecureFile.Find(Message);
        }
    }

    class ConsoleLogWritter : ILogger
    {
        private ILogger _logger;

        public ConsoleLogWritter()
        {
        }

        public ConsoleLogWritter(ILogger logger)
        {
            _logger = logger;
        }

        public virtual void WriteError(string message)
        {
            Console.WriteLine(message);

            if (_logger != null)
                _logger.WriteError(message);
        }
    }

    class SecureConsoleLogWritter : ConsoleLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class FileLogWritter : ILogger
    {
        public virtual void WriteError(string message)
        {
            File.WriteAllText("log.txt", message);
        }
    }

    class SecureFileLogWritter : FileLogWritter
    {
        public override void WriteError(string message)
        {
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                base.WriteError(message);
            }
        }
    }

    class Pathfinder : ILogger
    {
        private ILogger _logger;

        public Pathfinder(ILogger logger)
        {
            _logger = logger;
        }

        public void Find(string message)
        {
            WriteError(message);
        }

        public void WriteError(string message)
        {
            _logger.WriteError(message);
        }
    }
}