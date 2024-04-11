using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace HomeWork2_2
{
    public class Program
    {
        static void Main(string[] args)
        {
            // Считывание JSON файла
            string jsonString = File.ReadAllText("JSON_sample_1.json");
            var deals = JsonSerializer.Deserialize<List<Deal>>(jsonString);

            // Вызов метода GetNumbersOfDeals
            var numbersOfDeals = GetNumbersOfDeals(deals);
            Console.WriteLine($"[*] Количество найденных значений: {numbersOfDeals.Count()}");
            Console.WriteLine($"[*] Идентификаторы:");
            foreach (var num in numbersOfDeals)
            {
                Console.WriteLine($"\t∟ Номер: {num},");
            }

            // Вызов метода GetSumsByMonth
            var sumsByMonth = GetSumsByMonth(deals);
            Console.WriteLine("\n[*] Пары месяц-сумма:");
            foreach (var sum in sumsByMonth)
            {
                Console.WriteLine($"\t∟ {sum.Month:MMMM}: {sum.Sum}");
            }
        }

        public class Deal
        {
            public int Sum { get; set; }
            public string Id { get; set; }
            public DateTime Date { get; set; }
        }

        // Метод GetNumbersOfDeals
        static IList<string> GetNumbersOfDeals(IEnumerable<Deal> deals)
        {
            // Фильтрация по сумме (не меньше 100 руб)
            // Далее взятие 5 сделок с ранней датой
            var filter = deals
                        .Where(deal => deal.Sum >= 100)
                        .OrderBy(deal => deal.Date)
                        .Take(5)
                        .OrderByDescending(deal => deal.Sum)
                        .Select(deal => deal.Id)
                        .ToList();

            // Возвращение Id в отсортированном по сумме по убыванию виде
            return filter;
        }

        // Синтаксический сахар для объявления класса
        record SumByMonth(DateTime Month, int Sum);

        // Метод GetSumsByMonth
        static IList<SumByMonth> GetSumsByMonth(IEnumerable<Deal> deals)
        {
            // Группировка по месяцу сделки
            // Далее возвращение суммы сделок за каждый месяц
            var sum = deals
                    .GroupBy(deal => new DateTime(deal.Date.Year, deal.Date.Month, 1))
                    .OrderBy(group => group.Key)
                    .Select(group => new SumByMonth(group.Key, group.Sum(deal => deal.Sum)))
                    .ToList();

            // Возвращение суммы
            return sum;
        }
    }
}