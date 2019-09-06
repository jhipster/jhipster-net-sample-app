using JHipsterNetSampleApplication.Models;
using System;

namespace JHipsterNetSampleApplication.Test.Setup {
    public class AssociatedEntityFactories {
        public static BankAccount getDefaultAssociatedBankAccount()
        {
            var bankAccount = new BankAccount {
                Name = "AAAAAAAAAA",
                Balance = new decimal(1.0)
            };

            return bankAccount;
        }

        public static BankAccount getUpdatedAssociatedBankAccount()
        {
            var updatedBankAccount = new BankAccount {
                Name = "BBBBBBBBBB",
                Balance = new decimal(2.0)
            };

            return updatedBankAccount;
        }

        public static Operation getDefaultAssociatedOperation()
        {
            var operation = new Operation {
                Date = DateTime.UnixEpoch,
                Description = "AAAAAAAAAA",
                Amount = new decimal(1.0)
            };

            return operation;
        }

        public static Operation getUpdatedAssociatedOperation()
        {
            var updatedOperation = new Operation {
                Date = DateTime.Now,
                Description = "BBBBBBBBBB",
                Amount = new decimal(2.0)
            };

            return updatedOperation;
        }

        public static Label getDefaultAssociatedLabel()
        {
            var label = new Label {
                Name = "AAAAAAAAAA"
            };

            return label;
        }

        public static Label getUpdatedAssociatedLabel()
        {
            var updatedLabel = new Label {
                Name = "BBBBBBBBBB"
            };

            return updatedLabel;
        }
    }
}
