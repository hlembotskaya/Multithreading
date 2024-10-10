using System;
using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MultiThreading.Task3.MatrixMultiplier.Matrices;
using MultiThreading.Task3.MatrixMultiplier.Multipliers;

namespace MultiThreading.Task3.MatrixMultiplier.Tests
{
    [TestClass]
    public class MultiplierTest
    {
        [TestMethod]
        public void MultiplyMatrix3On3Test()
        {
            TestMatrix3On3(new MatricesMultiplier());
            TestMatrix3On3(new MatricesMultiplierParallel());
        }

        [TestMethod]
        public void ParallelEfficiencyTest()
        {
            // todo: implement a test method to check the size of the matrix which makes parallel multiplication more effective than
            // todo: the regular one

            int matrixSize = 100;
            bool parallelFaster = false;

            var sequentialMultiplier = new MatricesMultiplier();
            var parallelMultiplier = new MatricesMultiplierParallel();
            
            while (!parallelFaster)
            {
                Matrix matrixA = GenerateRandomMatrix(matrixSize, matrixSize);
                Matrix matrixB = GenerateRandomMatrix(matrixSize, matrixSize);

                // Ragular Multiplying time
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();
                sequentialMultiplier.Multiply(matrixA, matrixB);
                stopwatch.Stop();
                long sequentialTime = stopwatch.ElapsedMilliseconds;

                // Parallel Time
                stopwatch.Restart();
                parallelMultiplier.Multiply(matrixA, matrixB);
                stopwatch.Stop();
                long parallelTime = stopwatch.ElapsedMilliseconds;

                Console.WriteLine($"Matrix Size: {matrixSize}x{matrixSize} | Sequential Time: {sequentialTime} ms | Parallel Time: {parallelTime} ms");

                // Comparence 
                if (parallelTime < sequentialTime)
                {
                    parallelFaster = true;
                    Assert.IsTrue(parallelFaster, $"Parallel Multiplying faster for {matrixSize}x{matrixSize}.");
                }
                else
                {
                    matrixSize += 50; 
                }
            }

        }

        #region private methods

        private Matrix GenerateRandomMatrix(int rows, int cols)
        {
            Random random = new Random();
            Matrix matrix = new Matrix(rows, cols);
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < cols; j++)
                {
                    matrix.SetElement(i, j, random.Next(1, 100));
                }
            }
            return matrix;
        }

        void TestMatrix3On3(IMatricesMultiplier matrixMultiplier)
        {
            if (matrixMultiplier == null)
            {
                throw new ArgumentNullException(nameof(matrixMultiplier));
            }

            var m1 = new Matrix(3, 3);
            m1.SetElement(0, 0, 34);
            m1.SetElement(0, 1, 2);
            m1.SetElement(0, 2, 6);

            m1.SetElement(1, 0, 5);
            m1.SetElement(1, 1, 4);
            m1.SetElement(1, 2, 54);

            m1.SetElement(2, 0, 2);
            m1.SetElement(2, 1, 9);
            m1.SetElement(2, 2, 8);

            var m2 = new Matrix(3, 3);
            m2.SetElement(0, 0, 12);
            m2.SetElement(0, 1, 52);
            m2.SetElement(0, 2, 85);

            m2.SetElement(1, 0, 5);
            m2.SetElement(1, 1, 5);
            m2.SetElement(1, 2, 54);

            m2.SetElement(2, 0, 5);
            m2.SetElement(2, 1, 8);
            m2.SetElement(2, 2, 9);

            var multiplied = matrixMultiplier.Multiply(m1, m2);
            Assert.AreEqual(448, multiplied.GetElement(0, 0));
            Assert.AreEqual(1826, multiplied.GetElement(0, 1));
            Assert.AreEqual(3052, multiplied.GetElement(0, 2));

            Assert.AreEqual(350, multiplied.GetElement(1, 0));
            Assert.AreEqual(712, multiplied.GetElement(1, 1));
            Assert.AreEqual(1127, multiplied.GetElement(1, 2));

            Assert.AreEqual(109, multiplied.GetElement(2, 0));
            Assert.AreEqual(213, multiplied.GetElement(2, 1));
            Assert.AreEqual(728, multiplied.GetElement(2, 2));
        }

        #endregion
    }
}
