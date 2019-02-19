
namespace JHipsterNetSampleApplication.Domain.BankAccount {
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public class BankAccountManager {
        IBankAccountStore bankAccountStore;

        public BankAccountManager(IBankAccountStore bankAccountStore)
        {
            this.bankAccountStore = bankAccountStore;
        }
    }
}
