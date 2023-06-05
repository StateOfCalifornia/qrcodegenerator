using System;

namespace Domain.Common
{
    public abstract class AuditableEntity
    {
        public string CreatedBy { get; set; }

        public DateTime CreatedDate { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime? ModifiedDate { get; set; }
    }
}