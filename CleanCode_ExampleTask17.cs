using System;

namespace CleanCodeTask7
{
    class CleanCode_ExampleTask17
    {
        private static float _chance;
        private static int _hourlyRate;
        private static Random _random = new Random();

        public static void CreateNewObject()
        {
            //Создание объекта на карте
        }

        public static void GenerateChance()
        {
            _chance = _random.Next(0, 101);
        }

        public static int CalculateSalary(int hoursWorked)
        {
            return _hourlyRate * hoursWorked;
        }
    }
}
