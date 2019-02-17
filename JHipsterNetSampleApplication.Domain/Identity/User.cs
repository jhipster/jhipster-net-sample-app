
namespace JHipsterNetSampleApplication.Domain.Identity {
    using Microsoft.AspNetCore.Identity;
    using System;

    public class User : IdentityUser {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string LangKey { get; set; }
        public string ImageUrl { get; set; }
        public string ResetKey { get; set; }
        public DateTime? ResetDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string LastModifiedBy { get; set; }
        public DateTime LastModifiedDate { get; set; }
    }
}
