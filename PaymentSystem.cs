using System;

namespace IMJunior
{
    abstract class PaymentSystem
    {
        public void Open()
        {
            ShowOpenSystemText();
        }

        public void HandlePayment()
        {
            ShowHandlePaymentText();
        }

        public abstract string GetName();

        protected abstract void ShowOpenSystemText();

        protected abstract void ShowHandlePaymentText();
    }
}
