using JHipsterNetSampleApplication.Models.ManyToManyTools;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace JHipsterNetSampleApplication.Models {
    [Table("operation")]
    public class Operation {
        public Operation()
        {
            Labels = new JoinCollectionFacade<Label, Operation, OperationLabel>(this, OperationLabels);
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long Id { get; set; }

        [Required] [Column("nhi_date")] public DateTime Date { get; set; }
        
        [Column("description")] public string Description { get; set; }

        [Required]
        [Column("amount", TypeName = "decimal(10,2)")]
        public decimal Amount { get; set; }

        //public int BankAccountId { get; set; }
        //        [JsonIgnore]
        public BankAccount BankAccount { get; set; }

        private ICollection<OperationLabel> OperationLabels { get; } = new List<OperationLabel>();

        [NotMapped]
        public ICollection<Label> Labels { get; }

        public override bool Equals(object obj)
        {
            if (this == obj) return true;
            if (obj == null || GetType() != obj.GetType()) return false;
            var operation = obj as Operation;
            if (operation?.Id == null || operation?.Id == 0 || Id == 0) return false;
            return EqualityComparer<long>.Default.Equals(Id, operation.Id);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Id);
        }

        public override string ToString()
        {
            return "Operation{" +
                   $"ID='{Id}'" +
                   $", Date='{Date}'" +
                   $", Description='{Description}'" +
                   $", Amount='{Amount}'" +
                   "}";
        }
    }
}
