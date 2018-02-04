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


        public bool IsMatch(int? other)
        {
            if (other == null && Value == null) return true;
            if (other == null || Value == null) return false;

            switch (Operator)
            {
                case IntOperator.Equals:
                    return Value.Value == other.Value;
                case IntOperator.Greater:
                    return Value.Value > other.Value;
                case IntOperator.GreaterOrEqual:
                    return Value.Value >= other.Value;
                case IntOperator.Less:
                    return Value.Value < other.Value;
                case IntOperator.LessOrEqual:
                    return Value.Value <= other.Value;
                default:
                    return false;
            }
        }
    }
}
