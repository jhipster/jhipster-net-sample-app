using JHipsterNetSampleApplication.Models.Interfaces;
using System.ComponentModel.DataAnnotations.Schema;

namespace JHipsterNetSampleApplication.Models {
    [Table("operation_label")]
    public class OperationLabel : IJoinEntity<Operation>, IJoinEntity<Label> {
        public long OperationId { get; set; }
        public Operation Operation { get; set; }
        Operation IJoinEntity<Operation>.Navigation {
            get => Operation;
            set => Operation = value;
        }

        public long LabelId { get; set; }
        public Label Label { get; set; }            
        Label IJoinEntity<Label>.Navigation {
            get => Label;
            set => Label = value;
        }
    }
}
