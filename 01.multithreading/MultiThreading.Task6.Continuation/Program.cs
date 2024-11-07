/*
*  Create a Task and attach continuations to it according to the following criteria:
   a.    Continuation task should be executed regardless of the result of the parent task.
   b.    Continuation task should be executed when the parent task finished without success.
   c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation
   d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled
   Demonstrate the work of the each case with console utility.
*/
using System;
using System.Threading.Tasks;
using System.Threading;

namespace MultiThreading.Task6.Continuation
{
    class Program
    {
        static void Main(string[] args)
        {
            {
                Console.WriteLine("Create a Task and attach continuations to it according to the following criteria:");
                Console.WriteLine("a.    Continuation task should be executed regardless of the result of the parent task.");
                Console.WriteLine("b.    Continuation task should be executed when the parent task finished without success.");
                Console.WriteLine("c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.");
                Console.WriteLine("d.    Continuation task should be executed outside of the thread pool when the parent task would be cancelled.");
                Console.WriteLine("Demonstrate the work of the each case with console utility.");
                Console.WriteLine();


                //"a.    Continuation task should be executed regardless of the result of the parent task.
                Task parentTask = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Parent Task started.");
                    Thread.Sleep(1000); 
                    Console.WriteLine("Parent Task completed.");
                });

                parentTask.ContinueWith(t =>
                {
                    Console.WriteLine("Continuation Task executed regardless of the parent task result.");
                }, TaskContinuationOptions.None);



                //b.    Continuation task should be executed when the parent task finished without success.
                Task parentTaskFailure = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Parent Task Failure started.");
                    Thread.Sleep(1000); 
                    throw new InvalidOperationException("Parent task encountered an error.");
                });

                parentTaskFailure.ContinueWith(t =>
                {
                    Console.WriteLine("Continuation Task executed after parent failed.");
                }, TaskContinuationOptions.OnlyOnFaulted | TaskContinuationOptions.ExecuteSynchronously);

                //c.    Continuation task should be executed when the parent task would be finished with fail and parent task thread should be reused for continuation.
                var cts = new CancellationTokenSource();
                Task parentTaskCanceled = Task.Factory.StartNew(() =>
                {
                    Console.WriteLine("Parent Task Canceled started.");
                    Thread.Sleep(1000); 
                    cts.Token.ThrowIfCancellationRequested();
                    Console.WriteLine("Parent Task Canceled completed.");
                }, cts.Token);

                parentTaskCanceled.ContinueWith(t =>
                {
                    Console.WriteLine("Continuation Task executed after parent was canceled.");
                }, TaskContinuationOptions.OnlyOnCanceled | TaskContinuationOptions.LongRunning);

                cts.Cancel();

                // Wait for all tasks to finish
                try
                {
                    Task.WaitAll(parentTask, parentTaskFailure, parentTaskCanceled);
                }
                catch (AggregateException ex)
                {
                    foreach (var innerEx in ex.InnerExceptions)
                    {
                        Console.WriteLine($"Exception: {innerEx.Message}");
                    }
                }
                Console.WriteLine("All tasks and continuations processed.");
            }
        }
    }
}
