using Northwind.Context; // To use NorthwindDataContext
using Northwind.EntityModels;

namespace Northwind.UnitTest
{
    public class EntityModelTests
    {
        [Fact]
        public void DatabaseConnectionTest()
        {
            using NorthwindDataContext db = new();
            Assert.True(db.Database.CanConnect());
        }

        [Fact]
        public void CategoryCountTest()
        {
            using NorthwindDataContext db = new();
            int expected = 8;
            int actual = db.Categories.Count();
        }

        [Fact]
        public void ProductIdIsChaiTest()
        {
            using NorthwindDataContext db = new();

            string expected = "Chai";
            Product? product = db.Products.Find(keyValues: 1);
            string actual = product?.ProductName ?? String.Empty;
            Assert.Equal(expected, actual);
        }
    }
}