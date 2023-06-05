using Application.Common.Mappings;
using Domain.Entities;
using System;

namespace Application.Customers
{
    public class CustomerViewModel : IMapFrom<Customer>
    {
        public string CustomerId { get; set; }
        public string CompanyName { get; set; }
        public string ContactName { get; set; }
        public string ContactTitle { get; set; }
        public string Address { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }

        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string MOdifiedBy { get; set; }
        public DateTime? ModifiedDate { get; set; }
    }
}