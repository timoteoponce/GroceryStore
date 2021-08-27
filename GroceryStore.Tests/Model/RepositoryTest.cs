using GroceryStore.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace GroceryStore.Tests
{
    [TestClass]
    public class RepositoryTest
    {
        [TestMethod]
        public void TestSaveCustomer()
        {
            var repo = new Repository();
            repo.DeleteDatabase();
            var c = new Customer { Id = 1, FirstName = "Kevin", LastName = "Arnold", Age = 16 };            
            repo.Save(c);
            Assert.AreEqual(c, repo.GetCustomers()[0]);
        }

        [TestMethod]
        public void TestSaveMultipleCustomers()
        {
            var repo = new Repository();
            repo.DeleteDatabase();
            for (int i = 0; i < 100; i++)
            {
                var c = new Customer { Id = i, FirstName = $"Kevin-{i}", LastName = "Arnold", Age = 16 + i };
                repo.Save(c);
            }
            Assert.AreEqual(100, repo.GetCustomers().Count);
        }

    }
}
