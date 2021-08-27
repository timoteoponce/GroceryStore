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

    }
}
