﻿using System.Collections.Generic;

namespace Domain.Entities
{
    public class Territory
    {
        public Territory()
        {
            Employees = new HashSet<Employee>();
        }

        public string TerritoryId { get; set; }
        public string TerritoryDescription { get; set; }
        public int RegionId { get; set; }

        public Region Region { get; set; }

        public ICollection<Employee> Employees { get; set; }
    }
}
