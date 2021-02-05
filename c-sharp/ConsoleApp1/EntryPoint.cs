using System;

namespace JokeGenerator
{
    public class EntryPoint
    {
        private string[] _results = new string[50];
        private char _key;
        private Tuple<string, string> _names;
        private readonly IPrinter _printer;
        private readonly IJsonFeed _jsonFeed;

        public EntryPoint(IPrinter printer, IJsonFeed jsonFeed)
        {
            _printer = printer;
            _jsonFeed = jsonFeed;
        }

        public void Run(string[] args)
        {
            _printer.WriteLine("Press ? to get instructions.");

            if (Console.ReadLine() != "?") 
                return;

            while (true)
            {
                _printer.WriteLine("Press c to get categories");
                _printer.WriteLine("Press r to get random jokes");
                GetEnteredKey(Console.ReadKey());
                switch (_key)
                {
                    case 'c':
                        GetCategories();
                        PrintResults();
                        break;
                    case 'r':
                    {
                        _printer.WriteLine("Want to use a random name? y/n");
                        GetEnteredKey(Console.ReadKey());
                        if (_key == 'y')
                            GetNames();
                        _printer.WriteLine("Want to specify a category? y/n");
                        if (_key == 'y')
                        {
                            _printer.WriteLine("How many jokes do you want? (1-9)");
                            if (int.TryParse(Console.ReadLine(), out var n))
                            {
                                _printer.WriteLine("Enter a category;");
                                GetRandomJokes(Console.ReadLine(), n);
                                PrintResults();
                            }
                        }
                        else
                        {
                            _printer.WriteLine("How many jokes do you want? (1-9)");
                            if (int.TryParse(Console.ReadLine(), out var n))
                            {
                                GetRandomJokes(null, n);
                                PrintResults();
                            }
                        }

                        break;
                    }
                }
                _names = null;
            }

        }

        private void PrintResults()
        {
            _printer.WriteLine($"[{string.Join(",", _results)}]");
        }

        private void GetEnteredKey(ConsoleKeyInfo consoleKeyInfo)
        {
            switch (consoleKeyInfo.Key)
            {
                case ConsoleKey.C:
                    _key = 'c';
                    break;
                case ConsoleKey.D0:
                    _key = '0';
                    break;
                case ConsoleKey.D1:
                    _key = '1';
                    break;
                case ConsoleKey.D3:
                    _key = '3';
                    break;
                case ConsoleKey.D4:
                    _key = '4';
                    break;
                case ConsoleKey.D5:
                    _key = '5';
                    break;
                case ConsoleKey.D6:
                    _key = '6';
                    break;
                case ConsoleKey.D7:
                    _key = '7';
                    break;
                case ConsoleKey.D8:
                    _key = '8';
                    break;
                case ConsoleKey.D9:
                    _key = '9';
                    break;
                case ConsoleKey.R:
                    _key = 'r';
                    break;
                case ConsoleKey.Y:
                    _key = 'y';
                    break;
            }
        }

        private void GetRandomJokes(string category, int number)
        {
            _results = _jsonFeed.GetRandomJokesAsync(_names?.Item1, _names?.Item2, category);
        }

        private void GetCategories()
        {
            _results = _jsonFeed.GetCategories();
        }

        private void GetNames()
        {
            new JsonFeed("https://www.names.privserv.com/api/");
            dynamic result = _jsonFeed.GetNames();
            _names = Tuple.Create(result.name.ToString(), result.surname.ToString());
        }
    }
}
