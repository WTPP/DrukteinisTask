namespace Models.Builders
{
    public sealed class TransactionBuilder
    {
        private readonly TransactionModel _transaction;

        public TransactionBuilder() => _transaction = new TransactionModel();

        public static implicit operator TransactionModel(TransactionBuilder builder) => builder._transaction;

        public TransactionBuilder SetBasketId(int basketId)
        {
            _transaction.BasketId = basketId;
            return this;
        }

        public TransactionBuilder SetProductId(int productId)
        {
            _transaction.ProductId = productId;
            return this;
        }

        public TransactionBuilder SetQuantity(double quantity)
        {
            _transaction.Quantity = quantity;
            return this;
        }

        public TransactionBuilder SetSum(double sum)
        {
            _transaction.Sum = sum;
            return this;
        }

        public TransactionBuilder SetIsLotteryWin(bool isLotteryWin)
        {
            _transaction.IsLotteryWin = isLotteryWin;
            return this;
        }
    }
}
