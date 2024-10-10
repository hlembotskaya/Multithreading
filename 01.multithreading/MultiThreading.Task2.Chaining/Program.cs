/*
 * 2.	Write a program, which creates a chain of four Tasks.
 * First Task – creates an array of 10 random integer.
 * Second Task – multiplies this array with another random integer.
 * Third Task – sorts this array by ascending.
 * Fourth Task – calculates the average value. All this tasks should print the values to console.
 */
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MultiThreading.Task2.Chaining
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine(".Net Mentoring Program. MultiThreading V1 ");
            Console.WriteLine("2.	Write a program, which creates a chain of four Tasks.");
            Console.WriteLine("First Task – creates an array of 10 random integer.");
            Console.WriteLine("Second Task – multiplies this array with another random integer.");
            Console.WriteLine("Third Task – sorts this array by ascending.");
            Console.WriteLine("Fourth Task – calculates the average value. All this tasks should print the values to console");
            Console.WriteLine();

            Random rnd = new Random();

            //1st Task creates an array of 10 random integers.
            Task<int[]> task1 = Task.Run(()=>
            {
                int[] randomArray = new int[10];
                Console.WriteLine("Random Array: ");
                for (int i = 0; i < randomArray.Length; i++)
                {
                    randomArray[i] = rnd.Next(1, 101);
                    Console.WriteLine($"{randomArray[i]}");
                }

                return randomArray;
            });

            //2nd Task multiplies this array with another random integer.
            Task<int[]> task2 = task1.ContinueWith(prevTask => 
            {
                int[] array = prevTask.Result;
                int coefficient = rnd.Next(1, 101);
                Console.WriteLine($"Multiplyed on {coefficient} Array: ");
                for (int i = 0; i < array.Length; i++) 
                {
                    array[i] *= coefficient;
                    Console.WriteLine($"{array[i]}");
                }

                return array;
            });

            //3rd Task sorts this array by ascending.
            Task<int[]> task3 = task2.ContinueWith(prevTask =>
            {
                int[] array = prevTask.Result;
                Array.Sort(array);

                Console.WriteLine("Sorted Array: ");
                for (int i = 0; i < array.Length; i++)
                {
                    Console.WriteLine($"{array[i]}");
                }

                return array;
            });


            //4th Task calculates the average value.
            Task task4 = task3.ContinueWith(prevTask =>
            {
                int[] array = prevTask.Result;
                double average = array.Average();

                Console.WriteLine($"Average value: {average:F2}");
            });

            //All these tasks should print the values to console.


            Console.ReadLine();
        }
    }
}
