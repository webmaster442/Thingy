using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity.MediaLibary
{
    public enum IntOperator : int
    {
        [Display(Name = "=")]
        Equals = 0,
        [Display(Name = "<")]
        Less = 1,
        [Display(Name = "<=")]
        LessOrEqual = 2,
        [Display(Name = ">")]
        Greater = 3,
        [Display(Name = ">=")]
        GreaterOrEqual = 4
    }

    public class IntQuery
    {
        public IntOperator Operator { get; set; }
        public int? Value { get; set; }

        public IntQuery()
        {
            Operator = IntOperator.Equals;
            Value = null;
        }

        public bool HasValue
        {
            get { return Value != null; }
        }
    }
}
