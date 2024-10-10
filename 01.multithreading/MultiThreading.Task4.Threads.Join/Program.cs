/*
 * 4.	Write a program which recursively creates 10 threads.
 * Each thread should be with the same body and receive a state with integer number, decrement it,
 * print and pass as a state into the newly created thread.
 * Use Thread class for this task and Join for waiting threads.
 * 
 * Implement all of the following options:
 * - a) Use Thread class for this task and Join for waiting threads.
 * - b) ThreadPool class for this task and Semaphore for waiting threads.
 */

using System;
using System.Threading;

namespace MultiThreading.Task4.Threads.Join
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("4.	Write a program which recursively creates 10 threads.");
            Console.WriteLine("Each thread should be with the same body and receive a state with integer number, decrement it, print and pass as a state into the newly created thread.");
            Console.WriteLine("Implement all of the following options:");
            Console.WriteLine();
            Console.WriteLine("- a) Use Thread class for this task and Join for waiting threads.");
            Console.WriteLine("- b) ThreadPool class for this task and Semaphore for waiting threads.");

            Console.WriteLine();

            int threadsState = 10;

            //a
            Console.WriteLine("CreateThreads(threadsState): ");
            CreateThreads(threadsState);


            //b
            Console.WriteLine("CreateThreadPool(threadsState): ");
            CreateThreadPool(threadsState);

            Console.ReadLine();
        }

        // a) Use Thread class for this task and Join for waiting threads.
        static void CreateThreads(int state)
        {
            if (state > 0)
            {
                Thread thread = new Thread(() => ThreadBody(state));
                thread.Start();
                thread.Join();
            }
        }

        static void ThreadBody(int state)
        {
            Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}, State: {state}");
            state--;

            CreateThreads(state);
        }


        //b) ThreadPool class for this task and Semaphore for waiting threads.

        private static Semaphore semaphore = new Semaphore(3, 3);

        static void CreateThreadPool(int state)
        {
            if (state > 0)
            {
                ThreadPool.QueueUserWorkItem(ThreadBody, state);
            }
        }

        static void ThreadBody(object stateObj)
        {
            int state = (int)stateObj;

            semaphore.WaitOne(); 

            try
            {
                Console.WriteLine($"Thread: {Thread.CurrentThread.ManagedThreadId}, State: {state}");
                state--;
                CreateThreadPool(state);
            }
            finally
            {
                semaphore.Release();
            }
        }

    }
}
