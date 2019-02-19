
namespace JHipsterNetSampleApplication.Domain.BankAccount {
    using System.ComponentModel.DataAnnotations;

    public class Label {
        [Key]
        public string Id { get; set; }

        [Required]
        [MinLength(3)]
        public string Name { get; set; }
    }
}
