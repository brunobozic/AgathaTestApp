using System;

namespace CountTo100
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            for (int i = 0; i <= 100; i++)
            {
                int sentinel = 100;

                if (i == 0)
                {
                    Console.Write(sentinel);
                }
                else if (i > 0)
                {
                    Console.Write(sentinel - i);
                }
            }
        }
    }
}