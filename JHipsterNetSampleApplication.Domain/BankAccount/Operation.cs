
namespace JHipsterNetSampleApplication.Domain.BankAccount {
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Operation {
        [Key]
        public string Id { get; set; }

        [Required]
        public DateTime? Date { get; set; }

        public string Description { get; set; }

        [Required]
        public decimal? Amount { get; set; }
    }
}
