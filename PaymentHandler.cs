using System;

namespace IMJunior
{
    class PaymentHandler
    {
        public void ShowPaymentResult(PaymentSystem paymentSystem)
        {
            Console.WriteLine($"Вы оплатили с помощью {paymentSystem.GetName()}");

            paymentSystem.HandlePayment();

            Console.WriteLine("Оплата прошла успешно!");
        }
    }
}
