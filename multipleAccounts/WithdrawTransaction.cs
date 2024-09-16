using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multipleAccounts
{
    public class WithdrawTransaction : Transaction
    {
        private Account _account;

        // Constructor that calls the base class constructor
        public WithdrawTransaction(Account account, decimal amount) : base(amount)
        {
            _account = account;
        }

        // Override the Execute method
        public override void Execute()
        {
            if (_executed)
            {
                throw new InvalidOperationException("Transaction has already been executed.");
            }

            _success = _account.Withdraw(_amount);
            _executed = true;
            _dateStamp = DateTime.Now; // Update timestamp when executed

            if (!_success)
            {
                throw new InvalidOperationException("Withdrawal failed due to insufficient funds.");
            }
        }

        // Override the Rollback method
        public override void Rollback()
        {
           

            if (_reversed)
            {
                throw new InvalidOperationException("Transaction has already been reversed.");
            }

            
            _reversed = _account.Deposit(_amount);
            _dateStamp = DateTime.Now; // Update timestamp when reversed
        }

        // Optionally override the Print method if specific behavior is needed
        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Withdrawal from account {_account.AccountNumber}");
        }
    }
}
