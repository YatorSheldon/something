using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace multipleAccounts
{
    public abstract class Transaction
    {
        
        protected decimal _amount;
        protected bool _success;
        protected bool _executed;
        protected bool _reversed;
        protected DateTime _dateStamp;

        // Read-only properties for the protected fields
        public bool Success
        {
            get { return _success; }
        }

        public bool Executed
        {
            get { return _executed; }
        }

        public bool Reversed
        {
            get { return _reversed; }
        }

        public DateTime DateStamp
        {
            get { return _dateStamp; }
        }

        // Constructor that sets the initial amount
        public Transaction(decimal amount)
        {
            _amount = amount;
            _success = false;
            _executed = false;
            _reversed = false;
            _dateStamp = DateTime.Now; // Set the initial timestamp when transaction is created
        }

        // Abstract method declarations 
        public abstract void Execute();
        public abstract void Rollback();

        // Virtual method that can be overridden by derived classes
        public virtual void Print()
        {
            Console.WriteLine($"Transaction amount: {_amount}");
            Console.WriteLine($"Executed: {_executed}");
            Console.WriteLine($"Success: {_success}");
            Console.WriteLine($"Reversed: {_reversed}");
            Console.WriteLine($"Date/Time: {_dateStamp}");
        }
    }
}
