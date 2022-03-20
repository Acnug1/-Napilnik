using System;
using System.Collections.Generic;
using System.Linq;

namespace Napilnik
{
    public class Good
    {
        private readonly string _name;

        public string Name => _name;

        public Good(string name)
        {
            _name = name;
        }
    }

    public class GoodInStock : IReadOnlyGoodInStock
    {
        public GoodInStock(Good good, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            Good = good;
            Count = count;
        }

        public Good Good { get; }
        public int Count { get; private set; }

        public void Merge(GoodInStock newGoodInStock)
        {
            if (newGoodInStock.Good != Good)
                throw new InvalidOperationException();

            Count += newGoodInStock.Count;
        }

        public void Take(GoodInStock selectedGoodInStock)
        {
            if (selectedGoodInStock.Good != Good)
                throw new InvalidOperationException();

            Count -= selectedGoodInStock.Count;

            if (Count < 0)
                throw new ArgumentOutOfRangeException(nameof(Count));
        }
    }

    public interface IReadOnlyGoodInStock
    {
        public Good Good { get; }
        public int Count { get; }
    }

    public class Warehouse
    {
        private readonly List<GoodInStock> _goodsInStock;

        public Warehouse()
        {
            _goodsInStock = new List<GoodInStock>();
        }

        public IReadOnlyList<IReadOnlyGoodInStock> GoodsInStock => _goodsInStock;

        public void Delive(Good good, int count)
        {
            var newGoodInStock = new GoodInStock(good, count);

            int goodInStockIndex = _goodsInStock.FindIndex(goodInStock => goodInStock.Good == good);

            if (goodInStockIndex == -1)
                _goodsInStock.Add(newGoodInStock);
            else
                _goodsInStock[goodInStockIndex].Merge(newGoodInStock);
        }

        public void ShowGoodsInStock()
        {
            foreach (GoodInStock goodInStock in _goodsInStock)
                Console.WriteLine($"На складе имеется {goodInStock.Good.Name}, в количестве {goodInStock.Count} шт.");
        }

        public void Take(IReadOnlyList<IReadOnlyGoodInCart> goodsInCart)
        {
            foreach (IReadOnlyGoodInCart goodInCart in goodsInCart)
            {
                int goodInStockIndex = _goodsInStock.FindIndex(goodInStock => goodInStock.Good == goodInCart.Good &&
                goodInStock.Count >= goodInCart.Count); 

                if (goodInStockIndex == -1)
                    throw new InvalidOperationException();
                else
                {
                    var selectedGoodInStock = new GoodInStock(goodInCart.Good, goodInCart.Count);
                    _goodsInStock[goodInStockIndex].Take(selectedGoodInStock);

                    if (_goodsInStock[goodInStockIndex].Count == 0)
                        _goodsInStock.RemoveAt(goodInStockIndex);
                }
            }
        }
    }

    public class Shop
    {
        private readonly Warehouse _warehouse;

        public Shop(Warehouse warehouse)
        {
            _warehouse = warehouse;
        }

        public Cart Cart()
        {
            return new Cart(_warehouse);
        }
    }

    public class GoodInCart : IReadOnlyGoodInCart
    {
        public GoodInCart(Good good, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            Good = good;
            Count = count;
        }

        public Good Good { get; }
        public int Count { get; private set; }

        public void Merge(GoodInCart newGoodInCart)
        {
            if (newGoodInCart.Good != Good)
                throw new InvalidOperationException();

            Count += newGoodInCart.Count;
        }
    }

    public interface IReadOnlyGoodInCart
    {
        public Good Good { get; }
        public int Count { get; }
    }

    public class Cart
    {
        private const string Paylink = "Платежная ссылка";
        private readonly Warehouse _warehouse;
        private readonly List<GoodInCart> _goodsInCart;
        private int _currentCount;

        public Cart(Warehouse warehouse)
        {
            _warehouse = warehouse;
            _goodsInCart = new List<GoodInCart>();
        }

        public IReadOnlyList<IReadOnlyGoodInCart> GoodsInCart => _goodsInCart;

        public void Add(Good good, int count)
        {
            if (count < 1)
                throw new ArgumentOutOfRangeException(nameof(count));

            int goodInCartIndex = _goodsInCart.FindIndex(goodInCart => goodInCart.Good == good);

            if (goodInCartIndex == -1)
                _currentCount = 0;
            else
                _currentCount = _goodsInCart[goodInCartIndex].Count;

            IReadOnlyGoodInStock goodInStock = _warehouse.GoodsInStock.FirstOrDefault
                    (goodInStock => goodInStock.Good == good && goodInStock.Count >= count + _currentCount);

            if (goodInStock == null)
                throw new InvalidOperationException();
            else
            {
                var newGoodInCart = new GoodInCart(good, count);

                if (goodInCartIndex == -1)
                    _goodsInCart.Add(newGoodInCart);
                else
                    _goodsInCart[goodInCartIndex].Merge(newGoodInCart);
            }
        }

        public void ShowGoodsInCart()
        {
            foreach (IReadOnlyGoodInCart goodInCart in _goodsInCart)
                Console.WriteLine($"В корзину добавлен {goodInCart.Good.Name}, в количестве {goodInCart.Count} шт.");
        }

        public string Order()
        {
            _warehouse.Take(GoodsInCart);
            _goodsInCart.Clear();
            return Paylink;
        }
    }
}
