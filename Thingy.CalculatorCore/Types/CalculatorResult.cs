namespace Thingy.CalculatorCore
{
    public class CalculatorResult
    {
        public Status Status { get; set; } 
        public string Content { get; set; }
        public object RawObject { get; set; }

        public CalculatorResult()
        {
            Status = Status.NoResult;
            Content = null;
        }

        public CalculatorResult(Status status, string content, object raw)
        {
            Status = status;
            Content = content;
            RawObject = raw;
        }
    }

    public enum Status
    {
        ResultOk,
        ResultError,
        NoResult
    }
}
