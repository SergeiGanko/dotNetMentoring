namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Foo
    {
        public byte Number { get; } = 1;

        public static string StaticProp { get; set; } = "Static Foo!";

        public string Name { get; set; }

        public string Description { get; set; }

        public double Price { get; set; }

        public int Quantity { get; set; }
    }
}
