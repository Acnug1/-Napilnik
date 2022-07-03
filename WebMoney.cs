using System;

namespace IMJunior
{
    class WebMoney : PaymentSystem
    {
        private readonly string Name = "WebMoney";

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
