using System;

namespace JHipsterNetSampleApplication.Domain.Identity {
    public interface IAuditedEntity {
        string CreatedBy { get; set; }

        DateTime CreatedDate { get; set; }

        string LastModifiedBy { get; set; }

        DateTime LastModifiedDate { get; set; }
    }
}
