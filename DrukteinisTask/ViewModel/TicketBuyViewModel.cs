using Data.Repositories;
using Data.Repositories.Abstract;
using DrukteinisTask.Helpers;
using Models;
using System.Collections.Generic;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using Models.Builders;
using DrukteinisTask.Views;

namespace DrukteinisTask.ViewModel
{
    public class TicketBuyViewModel : Framework.ViewModel
    {
        public ObservableCollection<ProductModel> Products { get; set; }

        private IGenericRepository<ProductModel> _productRepository;
        private IGenericRepository<BasketModel> _basketRepository;
        private IGenericRepository<TransactionModel> _transactionRepository;

        private ObservableCollection<ProductModel> _basketItems;
        private double _basketTotal;
        private string _basketSummary;
        private string _toWinLotteryText;

        public TicketBuyViewModel()
        {
            InitializeRepositories();
            InitializeProducts();

            ToWinLotteryText = "Hourly, one customer ordering each package type may win the other two packages for free";

            AddToBasketCommand = new RelayCommand(AddToBasket);
            RemoveItemCommand = new RelayCommand(RemoveItem);
            PayCommand = new RelayCommand(Pay);
        }

        private void InitializeRepositories()
        {
            _productRepository = new EFGenericRepository<ProductModel>();
            _basketRepository = new EFGenericRepository<BasketModel>();
            _transactionRepository = new EFGenericRepository<TransactionModel>();
        }

        private void InitializeProducts()
        {
            var productModels = _productRepository.Get().ToList();
            Products = new ObservableCollection<ProductModel>(
                productModels.Select(model => new ProductModel { Name = model.Name, Price = model.Price, Description = model.Description, Id = model.Id, ProductTypeId = model.ProductTypeId })
                );
        }

        public ICommand AddToBasketCommand { get; }
        private void AddToBasket(object parameter)
        {
            if (parameter is ProductModel product)
            {
                if (BasketItems is null)
                    BasketItems = new ObservableCollection<ProductModel>();

                BasketItems.Add(product);
                BasketTotal += product.Price;
                BasketSummary = $"Total price: ${BasketTotal}";

                CheckForWinningCondition();
            }
        }

        public ICommand RemoveItemCommand { get; }
        private void RemoveItem(object parameter)
        {
            if (parameter is ProductModel product)
            {
                BasketItems.Remove(product);
                BasketTotal -= product.Price;
                BasketSummary = $"Total price: ${BasketTotal}";

                CheckForWinningCondition();
            }
        }

        private void CheckForWinningCondition()
        {
            if (BasketItems == null || BasketItems.Count == 0)
            {
                ToWinLotteryText = "Hourly, one customer ordering each package type may win the other two packages for free";
                return;
            }

            var productTypesCounts = Enum.GetValues(typeof(Models.Enums.ProductType))
                                 .Cast<Models.Enums.ProductType>()
                                 .ToDictionary(
                                     s => s,
                                     s => BasketItems.Count(item => item.ProductTypeId == s)
                                 );

            var missingTypes = productTypesCounts.Where(s => s.Value == 0)
                                                 .Select(s => s.Key)
                                                 .ToList();

            switch (missingTypes.Count)
            {
                case 0:
                    ToWinLotteryText = "Pay for a chance to win!";
                    break;
                case 1:
                    ToWinLotteryText = $"To be eligible for the lottery, you are missing the {missingTypes[0]} ticket";
                    break;
                case 2:
                    ToWinLotteryText = $"To be eligible for the lottery, you are missing the {missingTypes[0]} and {missingTypes[1]} tickets";
                    break;
                default:
                    ToWinLotteryText = "";
                    break;
            }
        }

        public ICommand PayCommand { get; }
        private void Pay(object obj)
        {
            if (BasketItems is null || BasketItems.Count == 0)
                return;

            bool wonLottery = false;
            if (EligibleForLottery() && !SomebodyWonThisHour())
                wonLottery = DetermineWinningChance();

            BasketModel newBasket = new BasketBuilder()
                .SetBasketSum(BasketTotal);

            _basketRepository.Create(newBasket);

            if (wonLottery)
            {
                var winningNotificationWindow = new LotteryWinForm();
                winningNotificationWindow.ShowDialog();

                GetTwoTicketsForFree(newBasket);
            }

            CreateTransactions(newBasket);
            ClearBasket();
        }

        private void ClearBasket()
        {
            BasketItems.Clear();
            BasketTotal = 0;
            BasketSummary = string.Empty;
            CheckForWinningCondition();
        }

        private void CreateTransactions(BasketModel newBasket)
        {
            var groupedItems = BasketItems.GroupBy(item => item.Id)
                                                       .Select(group => new
                                                       {
                                                           ProductId = group.Key,
                                                           Quantity = group.Sum(item => 1),
                                                           Price = group.Sum(item => item.Price)
                                                       });

            foreach (var group in groupedItems)
            {
                TransactionModel transaction = new TransactionBuilder()
                    .SetBasketId(newBasket.Id)
                    .SetProductId(group.ProductId)
                    .SetQuantity(group.Quantity)
                    .SetSum(group.Price)
                    .SetIsLotteryWin(false);

                _transactionRepository.Create(transaction);
            }
        }

        private void GetTwoTicketsForFree(BasketModel newBasket)
        {
            var panoramaTransaction = BasketItems.FirstOrDefault(s => s.ProductTypeId == Models.Enums.ProductType.Panorama);
            var stripTransaction = BasketItems.FirstOrDefault(s => s.ProductTypeId == Models.Enums.ProductType.Strip);

            List<ProductModel> productModels = new List<ProductModel>() { panoramaTransaction, stripTransaction };

            foreach (var item in productModels)
            {
                TransactionModel transaction = new TransactionBuilder()
                    .SetBasketId(newBasket.Id)
                    .SetProductId(item.Id)
                    .SetQuantity(1)
                    .SetSum(0)
                    .SetIsLotteryWin(true);

                _transactionRepository.Create(transaction);
                BasketItems.Remove(item);
            }
        }

        private bool EligibleForLottery()
        {
            return BasketItems.GroupBy(item => item.ProductTypeId).Count() >= 3;
        }

        private bool DetermineWinningChance()
        {
            Random random = new Random();
            int chanceToWin = random.Next(1, 11);

            // Adjust the winning probability as needed
            // For example, if you want a 50% chance of winning, you can check if chanceToWin <= 5
            return chanceToWin <= 3; 
        }

        private bool SomebodyWonThisHour()
        {
            DateTime currentTime = DateTime.Now;
            DateTime currentHourStart = new DateTime(currentTime.Year, currentTime.Month, currentTime.Day, currentTime.Hour, 0, 0);

            bool thisHourWinnersExist = _transactionRepository.Get()
                .Any(transaction =>
                    transaction.IsLotteryWin == true &&
                    transaction.RecDate >= currentHourStart &&
                    transaction.RecDate < currentTime);

            return thisHourWinnersExist;
        }

        public ObservableCollection<ProductModel> BasketItems
        {
            get { return _basketItems; }
            set { SetProperty(ref _basketItems, value); }
        }

        
        public string ToWinLotteryText
        {
            get { return _toWinLotteryText; }
            set { SetProperty(ref _toWinLotteryText, value); }
        }

        public string BasketSummary
        {
            get { return _basketSummary; }
            set { SetProperty(ref _basketSummary, value); }
        }

        public double BasketTotal
        {
            get { return _basketTotal; }
            set { SetProperty(ref _basketTotal, value); }
        }
    }
}
