using System;
using System.Globalization;
using System.IO;

namespace StorageApp.ConsoleApp
{
    internal class Program
    {
        static void Main()
        {
            Run();
            Console.ReadKey();
        }
        
        private static void Run()
        {
            var startLoading = DateTime.Now.Millisecond;
            Storage storage = new();
            var endLoading = DateTime.Now.Millisecond;

            var result1 = storage.GetPalletsGroupedByExpiryDateSortedByWeight();
            var result2 = storage.Get3PalletsWithMaxExpiryDateSortedByVolume();

            Console.WriteLine($"Загрузка данных заняла {endLoading - startLoading} мс");

            string task1 = "Задание №1. Сгруппировать паллеты по сроку годности, отсортировать по возрастанию срока годности, в каждой группе отсортировать паллеты по весу";
            string task2 = "Задание №2. 3 паллеты, которые содержат коробки с наибольшим сроком годности, отсортированные по возрастанию объема";
            storage.DisplayResults(result1, task1);
            storage.DisplayResults(result2, task2);
        }
    }
}