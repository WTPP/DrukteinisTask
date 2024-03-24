using Data.Repositories.Abstract;
using Data.Repositories;
using Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Models.DTO;
using DrukteinisTask.Helpers;
using System.Windows.Input;

namespace DrukteinisTask.ViewModel
{
    public class TaxCalculatorViewModel : Framework.ViewModel
    {
        private IGenericRepository<TransactionModel> _transactionRepository;
        private IGenericRepository<ProductModel> _productRepository;
        private ObservableCollection<PurchaseDTO> _transactions;
        private DateTime _fromDate;
        private DateTime _toDate;

        public TaxCalculatorViewModel()
        {
            InitializeRepositories();
            SetDefaultDateRange();
            LoadTransactions();

            FilterCommand = new RelayCommand(FilterTransactions);
        }

        public ICommand FilterCommand { get; }
        private void FilterTransactions(object parameter)
        {
            FilterTransactions();
            NotifyPropertyChanged();
        }

        // Default tax rate, take from setting later
        public double TaxRate
        {
            get { return 8.625; }
        }

        public int TransactionsCount
        {
            get { return Transactions == null ? 0 : Transactions.Count; }
        }

        public double TaxesSum
        {
            get { return TotalSum * TaxRate / 100; }
        }

        public double TotalSum
        {
            get { return Transactions == null ? 0 : Transactions.Sum(transaction => transaction.Sum); }
        }

        public DateTime FromDate
        {
            get { return _fromDate; }
            set { SetProperty(ref _fromDate, value); }
        }

        public DateTime ToDate
        {
            get { return _toDate; }
            set { SetProperty(ref _toDate, value); }
        }

        public ObservableCollection<PurchaseDTO> Transactions
        {
            get { return _transactions; }
            set { SetProperty(ref _transactions, value); }
        }

        private void InitializeRepositories()
        {
            _transactionRepository = new EFGenericRepository<TransactionModel>();
            _productRepository = new EFGenericRepository<ProductModel>();
        }

        private void SetDefaultDateRange()
        {
            FromDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            ToDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.DaysInMonth(DateTime.Now.Year, DateTime.Now.Month));
        }

        private void LoadTransactions()
        {
            try
            {
                var transactions = (from t in _transactionRepository.Get()
                                    join p in _productRepository.Get()
                                    on t.ProductId equals p.Id
                                    select new PurchaseDTO
                                    {
                                        BasketId = t.BasketId,
                                        ProductName = p.Name,
                                        Quantity = t.Quantity,
                                        Sum = t.Sum,
                                        Date = t.RecDate
                                    }).ToList();
                Transactions = new ObservableCollection<PurchaseDTO>(transactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading transactions: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void FilterTransactions()
        {
            try
            {
                var filteredTransactions = (from t in _transactionRepository.Get()
                                            join p in _productRepository.Get()
                                            on t.ProductId equals p.Id
                                            where t.RecDate >= FromDate && t.RecDate <= ToDate
                                            select new PurchaseDTO
                                            {
                                                ProductName = p.Name,
                                                Quantity = t.Quantity,
                                                Sum = t.Sum,
                                                Date = t.RecDate
                                            }).ToList();
                Transactions = new ObservableCollection<PurchaseDTO>(filteredTransactions);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error filtering transactions: " + ex.Message, "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
