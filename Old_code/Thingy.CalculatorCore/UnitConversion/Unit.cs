namespace Thingy.CalculatorCore.UnitConversion
{
    /// <summary>
    /// Unit conversion base type
    /// </summary>
    public class Unit
    {
        /// <summary>
        /// Unit name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Conversion ratio compared to the base unit
        /// </summary>
        public double Ratio { get; set; }

        /// <summary>
        /// Action to do
        /// </summary>
        public Actions Action { get; set; }

        /// <summary>
        /// Offset value to add or subtract
        /// </summary>
        public double Offset { get; set; }

        /// <summary>
        /// Creates a new instance of unit
        /// </summary>
        public Unit() { }

        /// <summary>
        /// Creates a new instance of unit
        /// </summary>
        /// <param name="Name">Unit name</param>
        /// <param name="Ratio">Conversion ratio compared to the base unit</param>
        /// <param name="Action">Action to do</param>
        /// <param name="Offset">Offset value to add or subtract</param>
        public Unit(string Name, double Ratio = 1, Actions Action = Actions.None, double Offset = 0)
        {
            this.Name = Name;
            this.Ratio = Ratio;
            this.Action = Action;
            this.Offset = Offset;
        }
    }

    /// <summary>
    /// Unit Converter actions
    /// </summary>
    public enum Actions
    {
        None,
        Add,
        Multiply,
        Divide,
        Subtract
    }

}
