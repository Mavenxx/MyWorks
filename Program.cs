using System;
using System.Collections.Generic; // Работа с List
using System.Linq;
using System.Text.Json;
using System.Text;
using System.Threading.Tasks;
using System.IO; // Работа с файлами
using System.Xml.Linq;
using static HomeWork2.Program;

namespace HomeWork2
{
    public class Program
    {
        static void Main(string[] args)
        {
            Factory[] factories = GetFactories();
            Unit[] units = GetUnits();
            Tank[] tanks = GetTanks();

            Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

            var searchUnit = FindUnit(units, tanks, "Резервуар 2");
            var searchFactory = FindFactory(factories, searchUnit);

            Console.WriteLine($"\nРезервуар 2 принадлежит установке {searchUnit.Name} и заводу {searchFactory.Name}");

            // Вывод общей суммы загрузки всех резервуаров
            var totalVolume = GetTotalVolume(tanks);
            Console.WriteLine($"\nОбщий объем резервуаров: {totalVolume}");

            // Вывод в консоль всех резервуаров, включая имена цеха и фабрики, в которых они числятся
            PrintAllTanks(tanks, units, factories);

            // Поиск по наименованию в коллекции
            bool exit = false;
            while (!exit) {
                
                int choice;

                Console.WriteLine("\n[?] Выберите интересующий вас запрос:");
                Console.WriteLine("\t[1] Найти завод");
                Console.WriteLine("\t[2] Найти установку");
                Console.WriteLine("\t[3] Найти резервуар");
                Console.WriteLine("\t[4] Очистить консоль");
                Console.WriteLine("\t[0] Выход");
                Console.Write("\nТекущий выбор: ");

                if (int.TryParse(Console.ReadLine(), out choice))
                {
                    switch (choice)
                    {
                        case 0:
                            exit = true;
                            break;

                        // Поиск завода
                        case 1:
                            Console.Write("\n[*] Введите имя завода: ");
                            string name = Console.ReadLine();
                            var result = factories.FirstOrDefault(factory => factory.Name == name);
                            if (result != null)
                            {
                                Console.WriteLine("\n[!] Результаты поиска:");
                                Console.WriteLine($"\t∟ Название завода: \t{result.Name}");
                                Console.WriteLine($"\t∟ Номер завода: \t{result.Id}");
                                Console.WriteLine($"\t∟ Описание завода: \t{result.Description}");
                            }
                            else
                            {
                                Console.WriteLine($"\nПо данному имени ничего не найдено!\n");
                            }
                            Console.Write("\nНажмите любую клавишу для выхода.\n");
                            Console.ReadKey();
                            break;

                        // Поиск установки
                        case 2:
                            Console.Write("\n[*] Введите имя установки: ");
                            string name1 = Console.ReadLine();
                            var result1 = units.FirstOrDefault(unit => unit.Name == name1);
                            if (result1 != null)
                            {
                                Console.WriteLine("\n[!] Результаты поиска:");
                                Console.WriteLine($"\t∟ Название установки: \t{result1.Name}");
                                Console.WriteLine($"\t∟ Номер установки: \t{result1.Id}");
                                Console.WriteLine($"\t∟ Описание установки: \t{result1.Description}");
                                Console.WriteLine($"\t∟ Номер завода: \t{result1.FactoryId}");
                            }
                            else
                            {
                                Console.WriteLine($"\nПо данному имени ничего не найдено!\n");
                            }
                            Console.Write("\nНажмите любую клавишу для выхода.\n");
                            Console.ReadKey();
                            break;

                        // Поиск резервуара
                        case 3:
                            Console.Write("\n[*] Введите имя резервуара: ");
                            string name2 = Console.ReadLine();
                            var result2 = tanks.FirstOrDefault(tank => tank.Name == name2);
                            if (result2 != null)
                            {
                                Console.WriteLine("\n[!] Результаты поиска:");
                                Console.WriteLine($"\t∟ Название резервуара: \t{result2.Name}");
                                Console.WriteLine($"\t∟ Номер резервуара: \t{result2.Id}");
                                Console.WriteLine($"\t∟ Описание резервуара: \t{result2.Description}");
                                Console.WriteLine($"\t∟ Объем резервуара: \t{result2.Volume}");
                                Console.WriteLine($"\t∟ Максимальный объём: \t{result2.MaxVolume}");
                                Console.WriteLine($"\t∟ Номер установки: \t{result2.UnitId}");
                            }
                            else
                            {
                                Console.WriteLine($"\nПо данному имени ничего не найдено!\n");
                            }
                            Console.Write("\nНажмите любую клавишу для выхода!\n");
                            Console.ReadKey();
                            break;

                        case 4:
                            Console.Clear();
                            Console.Write("Консоль очищена\n");
                            break;

                        default:
                            Console.Write("\nНеверный выбор!\n");
                            Console.ReadKey();
                            break;
                    }
                }
            }

            // Сохранение объектов в JSON-файлы
            JsonUnload(factories.ToList(), "factories_new.json");
            JsonUnload(units.ToList(), "units_new.json");
            JsonUnload(tanks.ToList(), "tanks_new.json");
        }

        // Метод для выгрузки всех объектов в JSON (после завершения программы)
        public static void JsonUnload<T>(IEnumerable<T> items, string path)
        {
            var op = new JsonSerializerOptions { WriteIndented = true };
            string jsonString = JsonSerializer.Serialize(items, op);
            File.WriteAllText(path, jsonString);
        }

        // Директория файлов "\bin\Debug"
        // Метод для возвращения массива заводов
        public static Factory[] GetFactories()
        {
            string jsonString = File.ReadAllText("factories.json");

            // Синтаксис запросов
            var factories = from factory in JsonSerializer.Deserialize<List<Factory>>(jsonString) select factory;

            // Синтаксис методов
            /*var factories = JsonSerializer.Deserialize<List<Factory>>(jsonString).Select(factory => factory);*/

            return factories.ToArray();
        }

        // Метод для возвращения массива установок
        public static Unit[] GetUnits()
        {
            string jsonString = File.ReadAllText("units.json");

            // Синтаксис запросов
            /*var units = from unit in JsonSerializer.Deserialize<List<Unit>>(jsonString) select unit;*/

            // Синтаксис методов
            var units = JsonSerializer.Deserialize<List<Unit>>(jsonString).Select(unit => unit);

            return units.ToArray();
        }

        // Метод для возвращения массива резервуаров
        public static Tank[] GetTanks()
        {
            string jsonString = File.ReadAllText("tanks.json");

            // Синтаксис запросов
            /*var tanks = from tank in JsonSerializer.Deserialize<List<Tank>>(jsonString) select tank;*/

            // Синтаксис методов
            var tanks = JsonSerializer.Deserialize<List<Tank>>(jsonString).Select(tank => tank);

            return tanks.ToArray();
        }

        // Метод для возвращения установки (Unit), которой принадлежит резервуар (Tank)
        public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName)
        {
            // Синтаксис запросов
            var searchUnit = (from tank in tanks where tank.Name == tankName join unit in units on tank.UnitId equals unit.Id select unit).FirstOrDefault();

            // Синтаксис методов
            /*var searchUnit = tanks.Where(tank => tank.Name == tankName).Join(units, tank => tank.UnitId, unit => unit.Id, (tank, unit) => unit).FirstOrDefault();*/

            return searchUnit;
        }

        // Метод для возвращения объекта завода, соответствующий установке
        public static Factory FindFactory(Factory[] factories, Unit unit)
        {
            // Синтаксис запросов
            /*var searchFactory = (from factory in factories where unit.Id == factory.Id select factory).FirstOrDefault();*/

            // Синтаксис методов
            var searchFactory = factories.FirstOrDefault(factory => unit.Id == factory.Id);

            return searchFactory;
        }

        // Метод для возвращения суммарного объема резервуаров в массиве
        public static int GetTotalVolume(Tank[] tanks)
        {
            // Синтаксис запросов
            /*var totalVolume = (from tank in tanks select tank.Volume).Sum();*/

            // Синтаксис методов
            var totalVolume = tanks.Sum(tank => tank.Volume);

            return totalVolume;
        }

        // Метод для вывода в консоль всех резервуаров (cинтаксис запросов)
        public static void PrintAllTanks(Tank[] tanks, Unit[] units, Factory[] factories)
        {
            var tankInfo = from tank in tanks join unit in units on tank.UnitId equals unit.Id join factory in factories on unit.FactoryId equals factory.Id select new
            {
                TankId = tank.Id,
                TankName = tank.Name,
                TankDescription = tank.Description,
                TankVolume = tank.Volume,
                TankMaxVolume = tank.MaxVolume,
                TankFactory = factory.Description,
                TankFactoryName = factory.Name
            };

            foreach (var info in tankInfo)
            {
                Console.WriteLine($"\n[#] Резервуар №{info.TankId}");
                Console.WriteLine($"\t∟ Название: \t{info.TankName}");
                Console.WriteLine($"\t∟ Описание: \t{info.TankDescription}");
                Console.WriteLine($"\t∟ Объём: \t{info.TankVolume}");
                Console.WriteLine($"\t∟ Макс объём: \t{info.TankMaxVolume}");
                Console.WriteLine($"\t∟ Завод: \t{info.TankFactory}");
                Console.WriteLine($"\t∟ Имя цеха: \t{info.TankFactoryName}");
            }
        }

        // Метод для вывода в консоль всех резервуаров (cинтаксис методов)
        /*public static void PrintAllTanks(Tank[] tanks, Unit[] units, Factory[] factories)
        {

            var tankInfo = tanks.Join(units, tank => tank.UnitId, unit => unit.Id, (tank, unit) => new 
            { 
                Tank = tank,
                Unit = unit 
            })
            .Join(factories, joined => joined.Unit.FactoryId, factory => factory.Id, (joined, factory) => new 
            { 
                Tank = joined.Tank,
                Unit = joined.Unit,
                Factory = factory
            });

            foreach (var info in tankInfo)
            {
                Console.WriteLine($"\n[#] Резервуар №{info.Tank.Id}");
                Console.WriteLine($"\t∟ Название: \t{info.Tank.Name}");
                Console.WriteLine($"\t∟ Описание: \t{info.Tank.Description}");
                Console.WriteLine($"\t∟ Объём: \t{info.Tank.Volume}");
                Console.WriteLine($"\t∟ Макс объём: \t{info.Tank.MaxVolume}");
                Console.WriteLine($"\t∟ Завод: \t{info.Factory.Description}");
                Console.WriteLine($"\t∟ Имя цеха: \t{info.Factory.Name}");
            }
        }*/
    }

    // Классы, описывающие структуру таблиц

    /// <summary>
    /// Заводы - Factories
    /// </summary>
    public class Factory
    {
        public int Id { get; set; } // Номер завода
        public string Name { get; set; } // Название завода
        public string Description { get; set; } // Описание завода

        // Конструктор по умолчанию
        public Factory() { }
    }

    /// <summary>
    /// Установки - Units
    /// </summary>
    public class Unit
    {
        public int Id { get; set; } // Номер установки
        public string Name { get; set; } // Название установки
        public string Description { get; set; } // Описание установки
        public int FactoryId { get; set; } // Номер завода

        // Конструктор по умолчанию
        public Unit() { }
    }

    /// <summary>
    /// Резервуары - Tanks
    /// </summary>
    public class Tank
    {
        public int Id { get; set; } // Номер резервуара
        public string Name { get; set; } // Название резервуара
        public string Description { get; set; } // Описание резервуара
        public int Volume { get; set; } // Объем резервуара
        public int MaxVolume { get; set; } // Максимальный объем резервуара
        public int UnitId { get; set; } // Номер установки

        // Конструктор по умолчанию
        public Tank() { }
    }
}
