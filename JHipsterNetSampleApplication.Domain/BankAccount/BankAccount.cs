
namespace JHipsterNetSampleApplication.Domain.BankAccount {
    using JHipsterNetSampleApplication.Domain.Identity;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class BankAccount {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal? Balance { get; set; }

        public User User { get; set; }

        public virtual ICollection<Operation> Operations { get; set; }
    }
}
