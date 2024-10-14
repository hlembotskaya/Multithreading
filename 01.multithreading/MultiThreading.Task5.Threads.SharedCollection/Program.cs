/*
 * 5. Write a program which creates two threads and a shared collection:
 * the first one should add 10 elements into the collection and the second should print all elements
 * in the collection after each adding.
 * Use Thread, ThreadPool or Task classes for thread creation and any kind of synchronization constructions.
 */
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MultiThreading.Task5.Threads.SharedCollection
{
    class Program
    {
        private static List<string> sharedCollection;

        private static AutoResetEvent addAutoResetEvent = new AutoResetEvent(true);
        private static AutoResetEvent printAutoResetEvent = new AutoResetEvent(false);
        
        
        static void Main(string[] args)
        {
            Console.WriteLine("5. Write a program which creates two threads and a shared collection:");
            Console.WriteLine("the first one should add 10 elements into the collection and the second should print all elements in the collection after each adding.");
            Console.WriteLine("Use Thread, ThreadPool or Task classes for thread creation and   .");
            Console.WriteLine();

            sharedCollection = new List<string>();

            Thread threadAdd = new Thread(Add);
            Thread threadPrint = new Thread(Print);


            threadAdd.Start();
            threadPrint.Start();

            Console.ReadLine();
        }

        static void Add(object obj)
        {
            for (int i = 0; i < 10; i++) 
            {
                addAutoResetEvent.WaitOne();
                sharedCollection.Add($"{sharedCollection.Count + 1}");
                printAutoResetEvent.Set();
            }
        }

        static void Print(object obj)
        {
            for (int i = 0; i < 10; i++)
            {
                printAutoResetEvent.WaitOne();
                Console.WriteLine($"{sharedCollection.Last()}");
                addAutoResetEvent.Set();
                Thread.Sleep(1000);
            }
        }
    }
}
