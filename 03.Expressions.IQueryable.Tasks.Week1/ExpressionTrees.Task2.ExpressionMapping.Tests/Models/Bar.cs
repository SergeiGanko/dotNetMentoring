namespace ExpressionTrees.Task2.ExpressionMapping.Tests.Models
{
    internal class Bar
    {
        public byte Number { get; } = 5;

        public static string StaticProp { get; set; } = "Static Bar...";

        public string Name { get; set; }

        public string ShortDescription { get; set; }

        public int Price { get; set; }

        public long Quantity { get; set; }
    }
}
