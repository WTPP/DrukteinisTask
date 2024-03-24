using Models.Abstract;
using Models.Builders;
using Models.Enums;
using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models
{
    [Table("Product")]
    public class ProductModel : BaseModel
    {
        public ProductType ProductTypeId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public ProductModel() => RecDate = EditDate = DateTime.Now; 

        public static ProductBuilder CreateBuilder() => new ProductBuilder();
    }
}
