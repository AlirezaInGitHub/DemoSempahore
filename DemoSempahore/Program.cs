using System;
using System.Threading;
using System.Threading.Tasks;

namespace DemoSempahore
{
    class Program
    {
        static int X = 0;
        static Semaphore semaphore;

        static void Main(string[] args)
        {
            semaphore = new Semaphore(1, 1);
            Console.WriteLine("Expected result is X= 30000. Let's try:");

            for (int i = 0; i < 10; i++)
            {
                X = 0;
                var t1 = Task.Run(() => Increament(false));
                var t2 = Task.Run(() => Increament(false));
                var t3 = Task.Run(() => Increament(false));

                Task.WaitAll(t1, t2, t3);

                Console.Write($"Without semaphore X= {X}...");

                X = 0;
                t1 = Task.Run(() => Increament(true));
                t2 = Task.Run(() => Increament(true));
                t3 = Task.Run(() => Increament(true));

                Task.WaitAll(t1, t2, t3);

                Console.WriteLine($"With semaphore X= {X}");
            }

            Console.WriteLine("Press any ket to continue...");
            Console.ReadKey();
        }

        static void Increament(bool useSemaphore)
        {
            for (int i = 0; i < 10000; i++)
            {
                if (useSemaphore)
                    semaphore.WaitOne();
                X += 1;
                if (useSemaphore)
                    semaphore.Release();
            }
        }
    }
}
