using Models.Abstract;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Transaction")]
    public class TransactionModel : BaseModel
    {
        public int BasketId { get; set; }

        public int ProductId { get; set; }

        public double Quantity { get; set; }

        public double Sum { get; set; }

        public bool? IsLotteryWin { get; set; }

        public TransactionModel() => RecDate = EditDate = DateTime.Now;
    }
}
