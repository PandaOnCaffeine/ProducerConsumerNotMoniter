using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ProducerConsumerNotMoniter
{
    internal class Program
    {
        static SemaphoreSlim semaphore = new SemaphoreSlim(0);
        static Queue<int> buffer = new Queue<int>();
        static object _lock = new object();

        static void Main(string[] args)
        {
            Thread ProducerThread = new Thread(() => Producer());
            Thread ConsumerThread = new Thread(() => Consumer());
            ProducerThread.Start();
            ConsumerThread.Start();

            ProducerThread.Join();
            ConsumerThread.Join();
        }
        static void Producer()
        {
            while (true)
            {
                lock (_lock)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (buffer.Count < 4)
                        {

                            // Simulate producing an item

                            buffer.Enqueue(i + 1);
                            semaphore.Release();

                            Console.WriteLine($"Produceret: {i + 1}");
                        }
                        else
                        {
                            Console.WriteLine($"Producer fik ikke lov til at producere: {buffer.Count}");
                        }
                        Thread.Sleep(200);

                    }
                }
                Thread.Sleep(200);
            }
        }

        static void Consumer()
        {
            while (true)
            {
                lock (_lock)
                {
                    for (int i = 0; i < 10; i++)
                    {
                        if (buffer.Count > 0)
                        {

                            semaphore.Wait();

                            int item;

                            item = buffer.Dequeue();


                            // Simulate consuming an item
                            Console.WriteLine($"Consumed: {item}");
                        }
                        else
                        {
                            Console.WriteLine($"Consumer fik ikke lov til at consumere: {buffer.Count}");

                        }
                        Thread.Sleep(200);
                    }
                }
                Thread.Sleep(200);
            }
        }
    }
}

