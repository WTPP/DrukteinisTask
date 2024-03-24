using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.Builders
{
    public sealed class BasketBuilder
    {
        private readonly BasketModel _basket;

        public BasketBuilder() => _basket = new BasketModel();

        public static implicit operator BasketModel(BasketBuilder builder) => builder._basket;

        public BasketBuilder SetBasketSum(double basketSum)
        {
            _basket.BasketSum = basketSum;
            return this;
        }
    }
}
