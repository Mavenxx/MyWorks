using System;
using System.Collections.Generic; // Работа с List
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO; // Работа с файлами
using System.Xml.Linq;

namespace ClassWork
{
    class Program
    {
        static void Main(string[] args) {

            var factories = GetFactories();
            var units = GetUnits();
            var tanks = GetTanks();

            Console.WriteLine($"Количество резервуаров: {tanks.Length}, установок: {units.Length}");

            var foundUnit = FindUnit(units, tanks, "Резервуар 2");
            var factory = FindFactory(factories, foundUnit);

            Console.WriteLine($"\nРезервуар 2 принадлежит установке {foundUnit.Name} и заводу {factory.Name}");

            // -- #5 Вывести общую сумму загрузки всех резервуаров.
            var totalVolume = GetTotalVolume(tanks);
            Console.WriteLine($"\nОбщий объем резервуаров: {totalVolume}");

            // -- #4 Осуществить вывод в консоль всех резервуаров, включая имена цеха и фабрики, в которых они числятся.
            PrintAllTanks(tanks, units, factories);

            // -- #2 Создать экземпляры объектов из таблицы. Предусмотреть создание объектов
            // с помощью new, прямо в коде определив значения из приложенных таблиц.

            // Создание экземпляров объектов из таблицы "Заводы"
            List<Factory> Factories = new List<Factory>
            {
                new Factory() { Id=1, Name="НПЗ№1", Description="Первый нефтеперерабатывающий завод" },
                new Factory() { Id=2, Name="НПЗ№2", Description="Второй нефтеперерабатывающий завод" }
            };

            // Создание экземпляров объектов из таблицы "Установки"
            List<Unit> Units = new List<Unit>
            {
                new Unit() { Id=1, Name="ГФУ-2", Description="Газофракционирующая установка", FactoryId=1 },
                new Unit() { Id=2, Name="АВТ-6", Description="Атмосферно-вакуумная трубчатка", FactoryId=1 },
                new Unit() { Id=3, Name="АВТ-10", Description="Атмосферно-вакуумная трубчатка", FactoryId=2 }
            };

            // Создание экземпляров объектов из таблицы "Резервуары"
            List<Tank> Tanks = new List<Tank>
            {
                new Tank() { Id=1, Name="Резервуар 1", Description="Надземный - вертикальный", Volume=1500, MaxVolume=2000, UnitId=1 },
                new Tank() { Id=2, Name="Резервуар 2", Description="Надземный - горизонтальный", Volume=2500, MaxVolume=3000, UnitId=1 },
                new Tank() { Id=3, Name="Дополнительный резервуар 24", Description="Надземный - горизонтальный", Volume=3000, MaxVolume=3000, UnitId=2 },
                new Tank() { Id=4, Name="Резервуар 35", Description="Надземный - вертикальный", Volume=3000, MaxVolume=3000, UnitId=2 },
                new Tank() { Id=5, Name="Резервуар 47", Description="Подземный - двустенный", Volume=4000, MaxVolume=5000, UnitId=2 },
                new Tank() { Id=6, Name="Резервуар 256", Description="Подводный", Volume=500, MaxVolume=500, UnitId=3 }
            };

            // -- #6 Осуществить возможность поиска по наименованию в коллекции, например через ввод в консоли.
            int choice;

            Console.WriteLine("\nКакой объект в коллекции вы хотите найти?");
            Console.WriteLine("\t[1] Найти завод");
            Console.WriteLine("\t[2] Найти установку");
            Console.WriteLine("\t[3] Найти резервуар");
            Console.WriteLine("\t[0] Выход");
            Console.Write("\nТекущий выбор: ");

            if (int.TryParse(Console.ReadLine(), out choice)) {
                switch (choice) {
                    case 0:
                        break;

                    // Поиск завода
                    case 1:
                        Console.Write("\nВведите имя завода: ");
                        string name = Console.ReadLine();
                        var result = Factories.Find(x => x.Name == name);
                        if (result != null) {
                            Console.WriteLine($"\nНазвание завода: {result.Name}");
                            Console.WriteLine($"Номер завода: {result.Id}");
                            Console.WriteLine($"Описание завода: {result.Description}");
                        }
                        else {
                            Console.WriteLine($"\nПо данному имени ничего не найдено!");
                        }
                        Console.Write("\nНажмите любую клавишу для выхода.");
                        Console.ReadKey();
                        break;

                    // Поиск установки
                    case 2:
                        Console.Write("\n[*] Введите имя установки: ");
                        string name1 = Console.ReadLine();
                        var result1 = Units.Find(x => x.Name == name1);
                        if (result1 != null) {
                            Console.WriteLine($"\nНазвание установки: {result1.Name}");
                            Console.WriteLine($"Номер установки: {result1.Id}");
                            Console.WriteLine($"Описание установки: {result1.Description}");
                            Console.WriteLine($"Номер завода: {result1.FactoryId}");
                        }
                        else {
                            Console.WriteLine($"\nПо данному имени ничего не найдено!");
                        }
                        Console.Write("\nНажмите любую клавишу для выхода.");
                        Console.ReadKey();
                        break;

                    // Поиск резервуара
                    case 3:
                        Console.Write("\n[*] Введите имя резервуара: ");
                        string name2 = Console.ReadLine();
                        var result2 = Tanks.Find(x => x.Name == name2);
                        if (result2 != null) {
                            Console.WriteLine($"\nНазвание резервуара: {result2.Name}");
                            Console.WriteLine($"Номер резервуара: {result2.Id}");
                            Console.WriteLine($"Описание резервуара: {result2.Description}");
                            Console.WriteLine($"Объем резервуара: {result2.Volume}");
                            Console.WriteLine($"Максимальный объем резервуара: {result2.MaxVolume}");
                            Console.WriteLine($"Номер установки: {result2.UnitId}");
                        }
                        else {
                            Console.WriteLine($"\nПо данному имени ничего не найдено!");
                        }
                        Console.Write("\nНажмите любую клавишу для выхода!");
                        Console.ReadKey();
                        break;

                    default:
                        Console.Write("\nНеверный выбор!");
                        Console.ReadKey();
                        break;
                }
            }
        }

        // -- #3 Реализовать функции, возвращающие результат согласно комментариям.
        // Используйте циклы, условные операторы и оператор switch.

        // Директория файлов "ClassWork\ClassWork\bin\Debug"
        // Метод для возвращения массива заводов, согласно приложенным таблицам
        public static Factory[] GetFactories() {
            // Создание массива из 3 элементов
            Factory[] factories = new Factory[2];
            for (int i = 0; i < 2; i++) {
                string file = File.ReadLines("Factory.txt").ElementAt(i);
                string[] lines = file.Split(',');
                factories[i] = new Factory(int.Parse(lines[0]), lines[1], lines[2]);
            }
            return factories;
        }

        // Метод для возвращения массива установок, согласно приложенным таблицам
        public static Unit[] GetUnits() {
            // Создание массива из 4 элементов
            Unit[] units = new Unit[3];
            for (int i = 0; i < 3; i++) {
                string file = File.ReadLines("Unit.txt").ElementAt(i);
                string[] lines = file.Split(',');
                units[i] = new Unit(int.Parse(lines[0]), lines[1], lines[2], int.Parse(lines[3]));
            }
            return units;
        }

        // Метод для возвращения массива резервуаров, согласно приложенным таблицам
        public static Tank[] GetTanks() {
            // Создание массива из 7 элементов
            Tank[] tanks = new Tank[6];
            for (int i = 0; i < 6; i++) {
                string file = File.ReadLines("Tank.txt").ElementAt(i);
                string[] lines = file.Split(',');
                tanks[i] = new Tank(int.Parse(lines[0]), lines[1], lines[2], int.Parse(lines[3]), int.Parse(lines[4]), int.Parse(lines[5]));
            }
            return tanks;
        }

        // Метод для возвращения установки (Unit), которой принадлежит резервуар (Tank)
        public static Unit FindUnit(Unit[] units, Tank[] tanks, string tankName) {
            // Идем по списку резервуаров
            foreach (var tank in tanks) {
                // Если найден, ищем установку
                if (tank.Name == tankName) {
                    // Идем по списку установок
                    foreach (var unit in units) {
                        // Если объект найден, возвращаем его
                        if (tank.UnitId == unit.Id) {
                            return unit;
                        }
                    }
                }
            }
            // Возвращаем значение "null" если резервуар или установка по имени не найдены
            return null;
        }

        // Метод для возвращения объекта завода, соответствующий установке
        public static Factory FindFactory(Factory[] factories, Unit unit) {
            // Идем по списку заводов
            foreach (var factory in factories) {
                // Если объект найден, возвращаем его
                if (unit.Id == factory.Id) {
                    return factory;
                }
            }
            // Возвращаем значение "null" если завод по имени не найдены
            return null;
        }

        // Метод для возвращения суммарного объема резервуаров в массиве
        public static int GetTotalVolume(Tank[] units) {
            return GetTanks().Sum(item => item.Volume);
        }

        // Метод для вывода в консоль всех резервуаров, включая имена цеха и фабрики, в которых они числятся
        public static void PrintAllTanks(Tank[] tanks, Unit[] units, Factory[] factories) {
            // Идем по списку резервуаров
            foreach (var tank in tanks) {
                // Идем по списку установок
                foreach (var unit in units) {
                    // Если айди есть в таблице, идем дальше
                    if (tank.UnitId == unit.Id) {
                        // Идем по списку заводов
                        foreach (var factory in factories) {
                            // Если айди есть в таблице, идем дальше
                            if (unit.FactoryId == factory.Id) {
                                Console.WriteLine($"\n-- Резервуар номер: {tank.Id} --");
                                Console.WriteLine($"Название: {tank.Name}");
                                Console.WriteLine($"Описание: {tank.Description}");
                                Console.WriteLine($"Объём: {tank.Volume}");
                                Console.WriteLine($"Максимальный объём: {tank.MaxVolume}");
                                Console.WriteLine($"Название завода: {factory.Description}");
                                Console.WriteLine($"Имя цеха: {factory.Name}");
                                break;
                            }
                        }
                        break;
                    }
                }
            }
        }



        // -- #1 Создать классы, описывающие структуру каждой из приведённых ниже таблиц.

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

            // Конструктор с параметрами
            public Factory(int id, string name, string description)
            {
                Id = id;
                Name = name;
                Description = description;
            }
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

            // Конструктор с параметрами
            public Unit(int id, string name, string description, int factoryid)
            {
                Id = id;
                Name = name;
                Description = description;
                FactoryId = factoryid;
            }
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

            // Конструктор с параметрами
            public Tank(int id, string name, string description, int volume, int maxvolume, int unitid)
            {
                Id = id;
                Name = name;
                Description = description;
                Volume = volume;
                MaxVolume = maxvolume;
                UnitId = unitid;
            }
        }
    }
}
