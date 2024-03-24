using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Abstract
{
    public abstract class BaseModel : IBaseModel
    {
        [Index]
        public int Id { get; set; }
        public DateTime? RecDate { get; set; }
        public DateTime? EditDate { get; set; }
    }
}
