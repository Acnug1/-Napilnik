using System;

namespace IMJunior
{
    class YandexMoney : PaymentSystem
    {
        private readonly string Name = "YandexMoney";

        public override string GetName()
        {
            return Name;
        }

        protected override void ShowOpenSystemText()
        {
            Console.WriteLine($"Вызов API {Name}...");
        }

        protected override void ShowHandlePaymentText()
        {
            Console.WriteLine($"Проверка платежа через {Name}...");
        }
    }
}
