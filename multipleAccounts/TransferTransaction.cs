using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multipleAccounts
{
    public class TransferTransaction : Transaction
    {
        private Account _fromAccount;
        private Account _toAccount;
        private WithdrawTransaction _withdrawTransaction;
        private DepositTransaction _depositTransaction;

        // Constructor that calls the base class constructor
        public TransferTransaction(Account fromAccount, Account toAccount, decimal amount) : base(amount)
        {
            _fromAccount = fromAccount;
            _toAccount = toAccount;
            _withdrawTransaction = new WithdrawTransaction(fromAccount, amount);
            _depositTransaction = new DepositTransaction(toAccount, amount);
        }

        // Override the Execute method
        public override void Execute()
        {
            if (_executed)
            {
                throw new InvalidOperationException("Transaction has already been executed.");
            }

            // First withdraw from the source account
            _withdrawTransaction.Execute();

            if (_withdrawTransaction.Success)
            {
                // If withdrawal is successful, deposit into the destination account
                _depositTransaction.Execute();
                _success = _depositTransaction.Success;
            }

            _executed = true;
            _dateStamp = DateTime.Now; // Update timestamp when executed

            if (!_success)
            {
                throw new InvalidOperationException("Transfer failed.");
            }
        }

        // Override the Rollback method
        public override void Rollback()
        {
           

            if (_reversed)
            {
                throw new InvalidOperationException("Transaction has already been reversed.");
            }

            // Rollback the withdrawal
            if (_withdrawTransaction.Executed && !_withdrawTransaction.Reversed)
            {
                _withdrawTransaction.Rollback();
            }
            // Rollback the deposit first
            if (_depositTransaction.Executed && !_depositTransaction.Reversed)
            {
                _depositTransaction.Rollback();
            }



            _reversed = _withdrawTransaction.Success || _depositTransaction.Success;
            _dateStamp = DateTime.Now; // Update timestamp when reversed
        }

        // Optionally override the Print method if specific behavior is needed
        public override void Print()
        {
            base.Print();
            Console.WriteLine($"Transfer from account {_fromAccount.AccountNumber} to {_toAccount.AccountNumber}");
        }
    }
}
