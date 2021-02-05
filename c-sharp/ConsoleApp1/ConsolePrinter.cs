using System;

namespace JokeGenerator
{
    public interface IPrinter
    {
        void WriteLine(string value);
    }

    public class ConsolePrinter : IPrinter
    {
        public void WriteLine(string value)
        {
            Console.WriteLine(value);
        }
    }
}
