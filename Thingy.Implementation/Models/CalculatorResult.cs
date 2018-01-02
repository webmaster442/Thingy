namespace Thingy.Implementation.Models
{
    public class CalculatorResult
    {
        public Status Status { get; set; } 
        public string Content { get; set; }

        public CalculatorResult()
        {
            Status = Status.NoResult;
            Content = null;
        }

        public CalculatorResult(Status status, string content)
        {
            Status = status;
            Content = content;
        }
    }

    public enum Status
    {
        ResultOk,
        ResultError,
        NoResult
    }
}
