using System.Collections.Generic;
namespace Domain.Entities
{
    public class CustomerDemographic
    {
        public CustomerDemographic()
        {
            Customers = new HashSet<Customer>();
        }

        public string CustomerTypeId { get; set; }
        public string CustomerDesc { get; set; }

        public ICollection<Customer> Customers { get; set; }
    }
}
