using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace multipleAccounts
{
    public class Bank
    {
        private List<Account> _accounts; // List to hold bank accounts
        private List<Transaction> _transactions; // List to hold transactions

        public List<Transaction> Transactions => _transactions;


        // Constructor to initialize the accounts and transactions list
        public Bank()
        {
            _accounts = new List<Account>();
            _transactions = new List<Transaction>();
        }

        // Method to add an account to the bank
        public void AddAccount(Account account)
        {
            if (account != null)
            {
                _accounts.Add(account);
            }
        }

        // Method to get an account by name
        public Account? GetAccount(string name)
        {
            return _accounts.FirstOrDefault(account => account.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        // Method to execute any type of transaction (deposit, withdrawal, transfer)
        public void ExecuteTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                _transactions.Add(transaction); // Add the transaction to the list
                try
                {
                    transaction.Execute(); // Execute the transaction
                    Console.WriteLine("Transaction executed successfully.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Transaction failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Transaction is null, cannot execute.");
            }
        }

        // New method to rollback a transaction
        public void RollbackTransaction(Transaction transaction)
        {
            if (transaction != null)
            {
                try
                {
                    transaction.Rollback(); // Attempt to rollback the transaction
                    Console.WriteLine("Transaction rollback successful.");
                }
                catch (InvalidOperationException ex)
                {
                    Console.WriteLine($"Rollback failed: {ex.Message}");
                }
            }
            else
            {
                Console.WriteLine("Transaction is null, cannot rollback.");
            }
        }

        // Optional: Method to print the transaction history
        public void PrintTransactionHistory()
        {
            if (_transactions.Count == 0)
            {
                Console.WriteLine("No transactions available.");
                return;
            }

            Console.WriteLine("Transaction History:");
            for (int i = 0; i < _transactions.Count; i++)
            {
                Console.WriteLine($"Transaction {i + 1}:");
                _transactions[i].Print(); // Call the Print method on each transaction
                Console.WriteLine("------------------------");
            }
        }

    }
}
