using System;

namespace Models.Abstract
{
    public interface IBaseModel
    {
        int Id { get; set; }
        DateTime? RecDate { get; set; }
        DateTime? EditDate { get; set; }
    }
}
