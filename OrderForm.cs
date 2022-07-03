using System;
using System.Collections.Generic;

namespace IMJunior
{
    class OrderForm
    {
        public string ShowForm(IReadOnlyList<PaymentSystem> paymentSystems)
        {
            Console.Write("Мы принимаем: ");

            foreach (PaymentSystem paymentSystem in paymentSystems)
            {
                Console.Write($"{paymentSystem.GetName()}; ");
            }

            Console.WriteLine();

            //симуляция веб интерфейса
            Console.WriteLine("Какой системой вы хотите совершить оплату?");
            return Console.ReadLine();
        }
    }
}
