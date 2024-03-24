using Models.Abstract;
using Models.Builders;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Basket")]
    public class BasketModel : BaseModel
    {
        public double BasketSum { get; set; }

        public BasketModel() => RecDate = EditDate = DateTime.Now;

        public static BasketBuilder CreateBuilder() => new BasketBuilder();
    }
}
