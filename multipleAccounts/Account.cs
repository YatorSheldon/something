using System;

namespace multipleAccounts
{
    public class Account
    {
        // Private attributes
        private decimal _balance;
        private string _name;
        private string _accountNumber;

        // Constructor to initialize the account
        public Account(string name, string accountNumber, decimal balance)
        {
            _name = name ?? throw new ArgumentNullException(nameof(name), "Name cannot be null");
            _accountNumber = accountNumber ?? throw new ArgumentNullException(nameof(accountNumber), "Account number cannot be null");
            _balance = balance;
        }

        
        public decimal Balance
        {
            get { return _balance; }
        }

        
        public string Name
        {
            get { return _name; }
            private set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _name = value;
                }
                else
                {
                    throw new ArgumentException("Name cannot be empty.");
                }
            }
        }

        
        public string AccountNumber
        {
            get { return _accountNumber; }
            private set
            {
                if (!string.IsNullOrWhiteSpace(value))
                {
                    _accountNumber = value;
                }
                else
                {
                    throw new ArgumentException("Account number cannot be empty.");
                }
            }
        }

        // Deposit method (modifies the balance)
        public bool Deposit(decimal amount)
        {
            if (amount > 0)
            {
                _balance += amount;
                return true;
            }
            return false;
        }

        // Withdraw method (modifies the balance)
        public bool Withdraw(decimal amount)
        {
            if (amount > 0 && amount <= _balance)
            {
                _balance -= amount;
                return true;
            }
            Console.WriteLine(amount <= 0 ? "Withdrawal amount must be positive" : "Insufficient funds.");
            return false;
        }

        // Print method to display account details
        public void Print()
        {
            Console.WriteLine($"Account Name: {Name}");
            Console.WriteLine($"Account Number: {AccountNumber}");
            Console.WriteLine($"Account Balance: {Balance:c}");
        }
    }
}
