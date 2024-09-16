using multipleAccounts;
using System;


// Enumeration for menu options
public enum MenuOption
{
    AddAccount = 1,
    Withdraw,
    Deposit,
    Print,
    Transfer,
    Rollback,  
    Quit
}


public class BankSystem
{
    public static void Main(string[] args)
    {
        Bank myBank = new Bank();

        MenuOption userOption;

        // Main loop
        do
        {
            userOption = ReadUserOption();

            switch (userOption)
            {
                case MenuOption.AddAccount:
                    DoAddAccount(myBank);
                    break;
                case MenuOption.Withdraw:
                    DoWithdraw(myBank);
                    break;
                case MenuOption.Deposit:
                    DoDeposit(myBank);
                    break;
                case MenuOption.Print:
                    DoPrint(myBank);
                    break;
                case MenuOption.Transfer:
                    DoTransfer(myBank);
                    break;
                case MenuOption.Rollback: // New menu option for rollback
                    DoRollback(myBank);
                    break;
                case MenuOption.Quit:
                    Console.WriteLine("Exiting the system.");
                    break;
            }

        } while (userOption != MenuOption.Quit);
    }


    public static MenuOption ReadUserOption()
    {
        int option;
        bool validInput = false;

        while (!validInput)
        {
            Console.Clear();
            Console.WriteLine("Please choose an option:");
            Console.WriteLine("1. Add New Account");
            Console.WriteLine("2. Withdraw");
            Console.WriteLine("3. Deposit");
            Console.WriteLine("4. Print Account Details");
            Console.WriteLine("5. Transfer");
            Console.WriteLine("6. Rollback");  // Add Rollback option here
            Console.WriteLine("7. Quit");
            Console.Write("Enter your choice: ");
            string? input = Console.ReadLine();

            if (int.TryParse(input, out option) && option >= 1 && option <= 7)
            {
                validInput = true;
                return (MenuOption)option;
            }
            else
            {
                Console.WriteLine("Invalid option. Please try again.");
                PauseForUser();
            }
        }

        return MenuOption.Quit;
    }
    private static Account? FindAccount(Bank bank)
    {
        // Ask the user for the account name
        Console.Write("Enter the account name: ");
        string? name = Console.ReadLine();

        // Check if the name is valid (not null or empty)
        if (string.IsNullOrWhiteSpace(name))
        {
            Console.WriteLine("Invalid account name entered.");
            return null; // Returning null indicates no account found
        }

        // Delegate the search to the bank's GetAccount method
        Account? account = bank.GetAccount(name);

        // Notify the user about the outcome of the search
        if (account != null)
        {
            Console.WriteLine($"Account found: {account.Name} (Account Number: {account.AccountNumber})");
        }
        else
        {
            Console.WriteLine("Account not found.");
        }

        // Return the found account (or null if not found)
        return account;
    }

    public static void DoAddAccount(Bank bank)
    {
        // Prompt user for the account name and balance
        Console.Write("Enter the name for the new account: ");
        string? name = Console.ReadLine();

        Console.Write("Enter the starting balance: ");
        string? balanceInput = Console.ReadLine();
        decimal balance;

        if (!string.IsNullOrEmpty(name) && decimal.TryParse(balanceInput, out balance))
        {
            // Create a new Account object
            string accountNumber = GenerateAccountNumber(); // You may want to implement this function
            Account newAccount = new Account(name, accountNumber, balance);

            // Add the new account to the bank
            bank.AddAccount(newAccount);

            Console.WriteLine($"Account for {name} has been added successfully with balance {balance:c}.");
        }
        else
        {
            Console.WriteLine("Invalid name or balance input. Account creation failed.");
        }

        PauseForUser();
    }

    public static void DoDeposit(Bank bank)
    {
        // Use FindAccount to get the account
        Account? account = FindAccount(bank);

        if (account != null)
        {
            // Ask for the deposit amount
            Console.Write("Enter the amount to deposit: ");
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                // Create a deposit transaction and execute it via the bank
                DepositTransaction deposit = new DepositTransaction(account, amount);
                bank.ExecuteTransaction(deposit);
            }
            else
            {
                Console.WriteLine("Invalid amount entered.");
            }
        }
        else
        {
            Console.WriteLine("No account found. Cannot deposit money.");
        }

        PauseForUser();
    }

    public static void DoWithdraw(Bank bank)
    {
        // Use FindAccount to get the account
        Account? account = FindAccount(bank);

        if (account != null)
        {
            // Ask for the withdrawal amount
            Console.Write("Enter the amount to withdraw: ");
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                // Create a withdraw transaction and execute it via the bank
                WithdrawTransaction withdrawal = new WithdrawTransaction(account, amount);
                bank.ExecuteTransaction(withdrawal);
            }
            else
            {
                Console.WriteLine("Invalid amount entered.");
            }
        }
        else
        {
            Console.WriteLine("No account found. Cannot proceed with withdrawal.");
        }

        PauseForUser();
    }

    public static void DoTransfer(Bank bank)
    {
        // Use FindAccount to get the source account
        Console.WriteLine("Transfer from: ");
        Account? fromAccount = FindAccount(bank);

        // Use FindAccount to get the destination account
        Console.WriteLine("Transfer to: ");
        Account? toAccount = FindAccount(bank);

        if (fromAccount != null && toAccount != null)
        {
            // Ask for the transfer amount
            Console.Write("Enter the amount to transfer: ");
            string? input = Console.ReadLine();
            if (decimal.TryParse(input, out decimal amount) && amount > 0)
            {
                // Create a transfer transaction and execute it via the bank
                TransferTransaction transfer = new TransferTransaction(fromAccount, toAccount, amount);
                bank.ExecuteTransaction(transfer);
            }
            else
            {
                Console.WriteLine("Invalid amount entered.");
            }
        }
        else
        {
            Console.WriteLine("One or both accounts not found. Cannot proceed with transfer.");
        }

        PauseForUser();
    }
    public static void DoRollback(Bank bank)
    {
        // Check if there are any transactions to rollback
        if (bank.Transactions.Count == 0)
        {
            Console.WriteLine("No transactions available to rollback.");
            PauseForUser();  // Pause before returning to the menu
            return;  // Exit the method if there are no transactions
        }

        // Print the transaction history
        bank.PrintTransactionHistory();

        // Ask the user to choose a transaction to rollback
        Console.Write("Enter the number of the transaction to rollback (or 0 to cancel): ");
        string? input = Console.ReadLine();

        if (int.TryParse(input, out int transactionIndex) && transactionIndex > 0 && transactionIndex <= bank.Transactions.Count)
        {
            // Get the transaction from the list (adjust for zero-based index)
            Transaction transaction = bank.Transactions[transactionIndex - 1];

            // Attempt to rollback the transaction
            try
            {
                bank.RollbackTransaction(transaction);
                Console.WriteLine("Transaction rollback successful.");
            }
            catch (InvalidOperationException ex)
            {
                Console.WriteLine($"Rollback failed: {ex.Message}");
            }
        }
        else if (transactionIndex == 0)
        {
            Console.WriteLine("Rollback canceled.");
        }
        else
        {
            Console.WriteLine("Invalid transaction number.");
        }

        PauseForUser();  // Pause to allow the user to see the result before returning to the menu
    }






    public static void DoPrint(Bank bank)
    {
        // Use FindAccount to get the account
        Account? account = FindAccount(bank);

        if (account != null)
        {
            // Print the account details
            account.Print();
        }
        else
        {
            Console.WriteLine("No account found. Cannot print account details.");
        }

        PauseForUser();
    }



    // Helper method to pause the console for user interaction
    private static void PauseForUser()
    {
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();
    }

    // Optional method to generate a unique account number (you may customize this)
    private static string GenerateAccountNumber()
    {
        return DateTime.Now.Ticks.ToString().Substring(0, 10); // Generates a 10-digit account number
    }
}
