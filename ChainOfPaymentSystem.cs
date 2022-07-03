using System;
using System.Collections.Generic;
using System.Linq;

namespace IMJunior
{
    class ChainOfPaymentSystem
    {
        private const string ErrorMessage = "Платежная система не найдена";
        private readonly IEnumerable<PaymentSystem> _paymentSystems;

        public IReadOnlyList<PaymentSystem> PaymentSystems => _paymentSystems.ToList();

        public ChainOfPaymentSystem(IEnumerable<PaymentSystem> paymentSystems)
        {
            _paymentSystems = paymentSystems;
        }

        public static ChainOfPaymentSystem Create(params PaymentSystem[] paymentSystems)
        {
            return new ChainOfPaymentSystem(paymentSystems);
        }

        public PaymentSystem GetPaymentSystem(string systemId)
        {
            foreach (PaymentSystem paymentSystem in _paymentSystems)
            {
                if (systemId.ToLower() == paymentSystem.GetName().ToLower())
                    return paymentSystem;
            }

            Console.WriteLine(ErrorMessage);
            return null;
        }
    }
}
