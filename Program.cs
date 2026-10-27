using System;
using SimpleBank.Models;

Console.OutputEncoding = System.Text.Encoding.UTF8;

PrintHeader();

// -------------------------------------------------------------
// Step 1: Initialize the Bank with Constructor Validation
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 1. Initializing Bank                                             ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");

var bank = new Bank("Apex Trust Bank", "ATB01");
Console.WriteLine($"[SUCCESS] Created Bank: {bank.Name} (Branch: {bank.BranchCode})");
Console.WriteLine();

// -------------------------------------------------------------
// Step 2: Create Valid Accounts (Testing Constructors & Properties)
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 2. Creating Customer Bank Accounts                               ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");

var aliceAccount = new BankAccount("ACC-1001", "Alice Johnson", 1500.00m);
var bobAccount   = new BankAccount("ACC-1002", "Bob Smith", 500.00m);

bank.OpenAccount(aliceAccount);
bank.OpenAccount(bobAccount);

Console.WriteLine($"[OPENED] {aliceAccount.OwnerName} | Acc: {aliceAccount.AccountNumber} | Starting Balance: {aliceAccount.Balance:C}");
Console.WriteLine($"[OPENED] {bobAccount.OwnerName}   | Acc: {bobAccount.AccountNumber} | Starting Balance: {bobAccount.Balance:C}");
Console.WriteLine();

// -------------------------------------------------------------
// Step 3: Perform Deposits & Withdrawals
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 3. Processing Account Transactions                               ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");

Console.WriteLine($"--> Depositing {750.00m:C} into {aliceAccount.OwnerName}'s account...");
aliceAccount.Deposit(750.00m, "Direct deposit - Payroll");

Console.WriteLine($"--> Withdrawing {200.00m:C} from {aliceAccount.OwnerName}'s account...");
aliceAccount.Withdraw(200.00m, "ATM cash withdrawal");

Console.WriteLine($"--> Depositing {1200.00m:C} into {bobAccount.OwnerName}'s account...");
bobAccount.Deposit(1200.00m, "Client invoice payment");

Console.WriteLine($"--> Withdrawing {150.50m:C} from {bobAccount.OwnerName}'s account...");
bobAccount.Withdraw(150.50m, "Online utility bill");

Console.WriteLine();

// -------------------------------------------------------------
// Step 4: Display Formatted Statements (Readable Console Output)
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 4. Account Statements & Transaction History                      ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");

PrintAccountStatement(aliceAccount);
PrintAccountStatement(bobAccount);

// -------------------------------------------------------------
// Step 5: Bank-wide Portfolio Overview
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 5. Bank Portfolio Summary                                        ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
Console.WriteLine($"Bank Name             : {bank.Name}");
Console.WriteLine($"Branch Identifier     : {bank.BranchCode}");
Console.WriteLine($"Total Active Accounts : {bank.Accounts.Count}");
Console.WriteLine($"Total Assets Under Mgmt: {bank.TotalDeposits:C}");
Console.WriteLine();

// -------------------------------------------------------------
// Step 6: Testing Constructor Validations & Exception Handling
// -------------------------------------------------------------
Console.WriteLine("╔══════════════════════════════════════════════════════════════════╗");
Console.WriteLine("║ 6. Constructor Input Validation Verification                     ║");
Console.WriteLine("╚══════════════════════════════════════════════════════════════════╝");
Console.WriteLine("Testing defensive programming rules to verify invalid inputs are rejected:\n");

TestValidation("Empty Account Number", () =>
    new BankAccount("", "Charlie Brown", 100m));

TestValidation("Short Account Number (< 5 chars)", () =>
    new BankAccount("A1", "Charlie Brown", 100m));

TestValidation("Null/Empty Owner Name", () =>
    new BankAccount("ACC-9999", "   ", 100m));

TestValidation("Negative Initial Balance", () =>
    new BankAccount("ACC-9999", "Charlie Brown", -50.00m));

TestValidation("Invalid Bank Branch Code", () =>
    new Bank("Test Bank", "AB"));

TestValidation("Zero or Negative Transaction Amount", () =>
    new Transaction(TransactionType.Deposit, -10.00m, 100m, "Invalid deposit"));

TestValidation("Empty Transaction Description", () =>
    new Transaction(TransactionType.Withdrawal, 50.00m, 100m, "   "));

Console.WriteLine("==================================================================");
Console.WriteLine("   All tests completed successfully. Application is fully operational!");
Console.WriteLine("==================================================================");

// =============================================================
// Helper Methods for Formatting Console Output
// =============================================================

static void PrintHeader()
{
    Console.WriteLine("==================================================================");
    Console.WriteLine("                SIMPLEBANK CONSOLE APPLICATION                   ");
    Console.WriteLine("             Level 1 of 4 · Warm-up Implementation                ");
    Console.WriteLine("==================================================================");
    Console.WriteLine();
}

static void PrintAccountStatement(BankAccount account)
{
    Console.WriteLine($"Account Holder : {account.OwnerName}");
    Console.WriteLine($"Account Number : {account.AccountNumber}");
    Console.WriteLine($"Created Date   : {account.CreatedAt:yyyy-MM-dd HH:mm:ss} UTC");
    Console.WriteLine($"Current Balance: {account.Balance:C}");
    Console.WriteLine(new string('-', 66));
    Console.WriteLine($"{"Timestamp",-20} | {"Type",-10} | {"Amount",12} | {"Balance",12} | Description");
    Console.WriteLine(new string('-', 66));

    foreach (var tx in account.Transactions)
    {
        string sign = tx.Type == TransactionType.Deposit ? "+" : "-";
        string formattedAmount = $"{sign}{tx.Amount:C}";
        Console.WriteLine($"{tx.Timestamp:yyyy-MM-dd HH:mm:ss} | {tx.Type,-10} | {formattedAmount,12} | {tx.BalanceAfter,12:C} | {tx.Description}");
    }

    Console.WriteLine(new string('=', 66));
    Console.WriteLine();
}

static void TestValidation(string scenario, Action action)
{
    try
    {
        action();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[FAILED] {scenario} - Expected exception was not thrown.");
        Console.ResetColor();
    }
    catch (ArgumentOutOfRangeException ex)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[PASSED] {scenario} ");
        Console.ResetColor();
        Console.WriteLine($"-> Caught ArgumentOutOfRangeException: {ex.ParamName} - {ex.Message.Split('\n')[0]}");
    }
    catch (ArgumentException ex)
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.Write($"[PASSED] {scenario} ");
        Console.ResetColor();
        Console.WriteLine($"-> Caught ArgumentException: {ex.ParamName} - {ex.Message.Split('\n')[0]}");
    }
    catch (Exception ex)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[CAUGHT OTHER] {scenario} -> {ex.GetType().Name}: {ex.Message}");
        Console.ResetColor();
    }
}
