using System.ComponentModel.DataAnnotations;

namespace Thingy.Db.Entity.MediaLibary
{
    public enum StringOperator : int
    {
        [Display(Name = "Contains, Ignore Case")]
        ContainsIgnoreCase = 0,
        [Display(Name = "Contains")]
        Contains = 1,
        [Display(Name = "Exact Match, Ignore Case")]
        ExactmatchIgnoreCase = 2,
        [Display(Name = "Exact Match")]
        Exactmatch = 3
    }

    public class StringQuery
    {
        public StringOperator Operator { get; set; }
        public string Value { get; set; }

        public StringQuery()
        {
            Operator = StringOperator.ContainsIgnoreCase;
            Value = null;
        }

        public bool HasValue
        {
            get { return !string.IsNullOrEmpty(Value); }
        }
    }
}
