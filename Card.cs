using System;

namespace IMJunior
{
    class Card : PaymentSystem
    {
        private readonly string Name = "Card";

        public override string GetName()
        {
            return Name;
        }

        protected override void ShowOpenSystemText()
        {
            Console.WriteLine($"Вызов API банка эмитера карты {Name}...");
        }

        protected override void ShowHandlePaymentText()
        {
            Console.WriteLine($"Проверка платежа через {Name}...");
        }
    }
}
