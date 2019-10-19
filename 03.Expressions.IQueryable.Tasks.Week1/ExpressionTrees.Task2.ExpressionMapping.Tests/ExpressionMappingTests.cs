using System.Collections.Generic;
using ExpressionTrees.Task2.ExpressionMapping.Tests.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ExpressionTrees.Task2.ExpressionMapping.Tests
{
    [TestClass]
    public class ExpressionMappingTests
    {
        [TestMethod]
        public void TestMethod1()
        {
            var mapGenerator = new MappingGenerator();
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var foo = new Foo
            {
                Name = "Foo",
                Description = "Description of Foo",
                Price = 10.99,
                Quantity = 100
            };

            var bar = mapper.Map(foo);

            Assert.AreEqual(foo.Name, bar.Name);
            Assert.AreEqual((int)foo.Price, bar.Price);
            Assert.AreEqual(foo.Quantity, bar.Quantity);
        }

        [TestMethod]
        public void TestMethod2()
        {
            var mapGenerator = new MappingGenerator();
            mapGenerator.AddRule(nameof(Foo.Description), nameof(Bar.ShortDescription));
            var mapper = mapGenerator.Generate<Foo, Bar>();

            var foo = new Foo
            {
                Name = "Foo",
                Description = "Description of Foo",
                Price = 10.99,
                Quantity = 100
            };

            var bar = mapper.Map(foo);

            Assert.AreEqual(foo.Name, bar.Name);
            Assert.AreEqual(foo.Description, bar.ShortDescription);
            Assert.AreEqual((int)foo.Price, bar.Price);
            Assert.AreEqual(foo.Quantity, bar.Quantity);
        }
    }
}
