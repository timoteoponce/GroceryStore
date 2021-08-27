using System;

namespace GroceryStore.Model
{
    public class Customer
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Customer customer &&
                   Id == customer.Id &&
                   FirstName == customer.FirstName &&
                   LastName == customer.LastName &&
                   Age == customer.Age;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id, FirstName, LastName, Age);
        }
    }
}
