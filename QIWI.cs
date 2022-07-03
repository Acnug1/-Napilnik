using System;

namespace IMJunior
{
    class QIWI : PaymentSystem
    {
        private readonly string Name = "QIWI";

        public override string GetName()
        {
            return Name;
        }

        protected override void ShowOpenSystemText()
        {
            Console.WriteLine($"Перевод на страницу {Name}...");
        }

        protected override void ShowHandlePaymentText()
        {
            Console.WriteLine($"Проверка платежа через {Name}...");
        }
    }
}
