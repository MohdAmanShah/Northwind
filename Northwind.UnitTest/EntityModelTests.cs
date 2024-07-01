using Northwind.Context; // To use NorthwindContext
using Northwind.EntityModels;

namespace Northwind.UnitTest
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectionTest()
        {
            using NorthwindContext db = new();
            Assert.True(db.Database.CanConnect());
        }

        [Fact]
        public void CategoryCountTest()
        {
            using NorthwindContext db = new();
            int expected = 8;
            int actual = db.Categories.Count();
        }

        [Fact]
        public void ProductIdIsChaiTest()
        {
            using NorthwindContext db = new();

            string expected = "Chai";
            Product? product = db.Products.Find(keyValues: 1);
            string actual = product?.ProductName ?? String.Empty;
            Assert.Equal(expected, actual);
        }
    }
}