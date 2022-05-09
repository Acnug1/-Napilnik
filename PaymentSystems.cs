using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace Lesson1
{
    public interface IPaymentSystem
    {
        public string GetPayingLink(Order order);
    }

    public interface IHashGetter
    {
        public string GetHash(Order order);
    }

    class Program
    {
        private const string PaymentLinkTemplate1 = "pay.system1.ru/order?amount=12000RUB&hash=";
        private const string PaymentLinkTemplate2 = "order.system2.ru/pay?hash=";
        private const string PaymentLinkTemplate3 = "system3.com/pay?amount=12000&curency=RUB&hash=";

        static void Main(string[] args)
        {
            //Выведите платёжные ссылки для трёх разных систем платежа: 
            //pay.system1.ru/order?amount=12000RUB&hash={MD5 хеш + ID заказа}
            //order.system2.ru/pay?hash={MD5 хеш + ID заказа + сумма заказа}
            //system3.com/pay?amount=12000&curency=RUB&hash={SHA-1 хеш + сумма заказа + ID заказа + секретный ключ от системы}

            Order order = new Order(1, 2);

            PaymentSystem md5Hash = new PaymentSystem(new Md5Hash());
            PaymentSystem sha1Hash = new PaymentSystem(new Sha1Hash());
            PaymentSystem secretKey = new PaymentSystem(new SecreteKey());

            string paymentLink1 = PaymentLinkTemplate1 + md5Hash.GetPayingLink(order) + order.Id;
            string paymentLink2 = PaymentLinkTemplate2 + md5Hash.GetPayingLink(order) + order.Id + order.Amount;
            string paymentLink3 = PaymentLinkTemplate3 + sha1Hash.GetPayingLink(order)
                + order.Amount + order.Id + secretKey.GetPayingLink(order);
        }
    }

    public class Order
    {
        public readonly int Id;
        public readonly int Amount;

        public Order(int id, int amount) => (Id, Amount) = (id, amount);
    }

    public class Md5Hash : IHashGetter, IPaymentSystem
    {
        public string GetHash(Order order)
        {
            MD5 md5 = MD5.Create();
            byte[] hash = md5.ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString() + order.Amount.ToString()));

            return Convert.ToBase64String(hash);
        }

        public string GetPayingLink(Order order)
        {
            return GetHash(order);
        }
    }

    public class Sha1Hash : IHashGetter, IPaymentSystem
    {
        public string GetHash(Order order)
        {
            SHA1 sha1 = SHA1.Create();
            byte[] hash = sha1.ComputeHash(Encoding.UTF8.GetBytes(order.Id.ToString() + order.Amount.ToString()));

            return Convert.ToBase64String(hash);
        }

        public string GetPayingLink(Order order)
        {
            return GetHash(order);
        }
    }

    public class SecreteKey : IPaymentSystem
    {
        private byte[] _encrypted;

        private string GetSecretKey(Order order)
        {
            using (Aes aes = Aes.Create())
            {
                aes.GenerateIV();
                aes.GenerateKey();
                ICryptoTransform encryptor = aes.CreateEncryptor(aes.Key, aes.IV);
                using (var msEncrypt = new MemoryStream())
                {
                    using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (var swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(order.Id.ToString() + order.Amount.ToString());
                        }
                        _encrypted = msEncrypt.ToArray();
                    }
                }
            }

            return Convert.ToBase64String(_encrypted);
        }

        public string GetPayingLink(Order order)
        {
            return GetSecretKey(order);
        }
    }

    public class PaymentSystem : IPaymentSystem
    {
        private IPaymentSystem _paymentSystem;

        public PaymentSystem(IPaymentSystem paymentSystem)
        {
            _paymentSystem = paymentSystem;
        }

        public string GetPayingLink(Order order)
        {
            return _paymentSystem.GetPayingLink(order);
        }
    }
}