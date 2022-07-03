using System;

namespace IMJunior
{
    class Program
    {
        static void Main(string[] args)
        {
            PaymentSystem qiwi = new QIWI();
            PaymentSystem webMoney = new WebMoney();
            PaymentSystem card = new Card();
            PaymentSystem yandexMoney = new YandexMoney();
            ChainOfPaymentSystem chainOfPaymentSystems = ChainOfPaymentSystem.Create(qiwi, webMoney, card, yandexMoney);

            var orderForm = new OrderForm();
            var paymentHandler = new PaymentHandler();

            string systemId = orderForm.ShowForm(chainOfPaymentSystems.PaymentSystems);

            PaymentSystem selectedPaymentSystem = chainOfPaymentSystems.GetPaymentSystem(systemId);

            if (selectedPaymentSystem != null)
            {
                selectedPaymentSystem.Open();

                paymentHandler.ShowPaymentResult(selectedPaymentSystem);
            }
        }
    }
}
