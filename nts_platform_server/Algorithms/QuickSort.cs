using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using nts_platform_server.Entities;

namespace nts_platform_server.Algorithms
{
    public class QuickSort<T>
    {
        public QuickSort(T[] inputArray)
        {

            Stopwatch stopwatch = new();
            stopwatch.Start();

            IOrderedEnumerable<T> orderedNumbers = from i in inputArray
                                 orderby i
                                 select i;

            //останавливаем счётчик
            stopwatch.Stop();
            //смотрим сколько миллисекунд было затрачено на выполнение

            var resultTime = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                resultTime.Hours,
                                                resultTime.Minutes,
                                                resultTime.Seconds,
                                                resultTime.Milliseconds);

            Console.WriteLine($"Stopwatch Stop LINQ:{resultTime.Ticks} = { elapsedTime}");

            stopwatch = new Stopwatch();
            stopwatch.Start();

            var s = (int[])Convert.ChangeType(inputArray, typeof(int[]));

            int[] sortedArray = Sort(s, 0, inputArray.Length - 1);


            //останавливаем счётчик
            stopwatch.Stop();
            resultTime = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                resultTime.Hours,
                                                resultTime.Minutes,
                                                resultTime.Seconds,
                                                resultTime.Milliseconds);

            Console.WriteLine($"Stopwatch Stop QuickSort:{resultTime.Ticks} = { elapsedTime}");
        }


        private static string _parametr = "";
        private static Type _type = null;
        private static PropertyInfo _propertyInfo = null;
        public QuickSort(List<T> inputArray, string parametr)
        {
            _parametr = parametr;
            _type = typeof(T);
            _propertyInfo = _type!.GetProperty(_parametr);
            //создаем объект
            Stopwatch stopwatch = new();
            //засекаем время начала операции
            stopwatch.Start();
            // Console.WriteLine($"Stopwatch Start:{ stopwatch.ElapsedTicks}");

            var list = inputArray.OrderBy(x => _type.InvokeMember(
                    _parametr
                    , System.Reflection.BindingFlags.GetProperty
                    , null
                    , x
                    , null
                ));//.ToList();
            //останавливаем счётчик
            stopwatch.Stop();
            //смотрим сколько миллисекунд было затрачено на выполнение
            var resultTime = stopwatch.Elapsed;

            string elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                resultTime.Hours,
                                                resultTime.Minutes,
                                                resultTime.Seconds,
                                                resultTime.Milliseconds);
            Console.WriteLine($"Stopwatch Stop LINQ:{resultTime.Ticks} = { elapsedTime}");
            //создаем объект
            stopwatch = new();
            stopwatch.Start();
            List<T> sortedArray = Sort(inputArray, 0, inputArray.Count - 1);
            stopwatch.Stop();
            resultTime = stopwatch.Elapsed;
            elapsedTime = String.Format("{0:00}:{1:00}:{2:00}.{3:000}",
                                                resultTime.Hours,
                                                resultTime.Minutes,
                                                resultTime.Seconds,
                                                resultTime.Milliseconds);
            Console.WriteLine($"Stopwatch Stop QuickSort:{resultTime.Ticks} = { elapsedTime}");
        }


        private static List<T> Sort(List<T> array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex);

            Sort(array, minIndex, pivotIndex - 1);

            Sort(array, pivotIndex + 1, maxIndex);

            return array;
        }

        private static int GetPivotIndex(List<T> array, int minIndex, int maxIndex)
        {
            int pivot = minIndex - 1;

            int right = (int)_propertyInfo.GetValue(array[maxIndex]);

            T tmp;

            for (int i = minIndex; i <= maxIndex; i++)
            {
                //int left = (int)_type!.GetProperty(_parametr).GetValue(array[i]);
               

                if ((int)_propertyInfo.GetValue(array[i]) < right)
                {
                    pivot++;
                     //Swap(array,pivot, i);

                    tmp = array[pivot];
                    array[pivot] = array[i];
                    array[i] = tmp;
                }

            }

            pivot++;
            //Swap(array, pivot, maxIndex);

            tmp = array[pivot];
            array[pivot] = array[maxIndex];
            array[maxIndex] = tmp;

            return pivot;
        }




        private static int[] Sort(int[] array, int minIndex, int maxIndex)
        {
            if (minIndex >= maxIndex)
            {
                return array;
            }

            int pivotIndex = GetPivotIndex(array, minIndex, maxIndex);

            Sort(array, minIndex, pivotIndex - 1);

            Sort(array, pivotIndex + 1, maxIndex);

            return array;
        }

        private static int GetPivotIndex(int[] array, int minIndex, int maxIndex)
        {
            int pivot = minIndex - 1;

            for (int i = minIndex; i <= maxIndex; i++)
            {

                if (array[i] < array[maxIndex])
                {
                    pivot++;
                    Swap(ref array[pivot], ref array[i]);
                }

            }

            pivot++;
            Swap(ref array[pivot], ref array[maxIndex]);

            return pivot;
        }

        private static void Swap(ref int leftItem, ref int rightItem)
        {
            int temp = leftItem;

            leftItem = rightItem;

            rightItem = temp;
        }

        private static List<T> Swap(List<T> list, int indexA, int indexB)
        {
            T tmp = list[indexA];
            list[indexA] = list[indexB];
            list[indexB] = tmp;
            return list;
        }

    }
}

