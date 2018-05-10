namespace Thingy.Engineering.Domain.LogicMinimizer
{
    internal class ImplicantRelationship
    {
        public Implicant A { get; }
        public Implicant B { get; }

        public ImplicantRelationship(Implicant first, Implicant second)
        {
            A = first;
            B = second;
        }
    }
}
