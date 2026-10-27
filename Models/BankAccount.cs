namespace SimpleBank.Models;

/// <summary>
/// Represents a customer's bank account.
/// Encapsulates all state via properties, never exposing public fields.
/// </summary>
public class BankAccount
{
    // Private state encapsulation
    private readonly List<Transaction> _transactions = new();

    // Properties (no public fields)
    public string AccountNumber { get; }
    public string OwnerName { get; private set; }
    public decimal Balance { get; private set; }
    public DateTime CreatedAt { get; }

    // Expose transaction history as an immutable read-only view
    public IReadOnlyList<Transaction> Transactions => _transactions.AsReadOnly();

    /// <summary>
    /// Initializes a new instance of <see cref="BankAccount"/> with constructor input validation.
    /// </summary>
    /// <param name="accountNumber">Unique account identifier (minimum 5 characters).</param>
    /// <param name="ownerName">Full name of the account owner.</param>
    /// <param name="initialDeposit">Starting deposit amount (must be non-negative).</param>
    /// <exception cref="ArgumentException">Thrown when account number or owner name is null or whitespace.</exception>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when initial deposit is negative.</exception>
    public BankAccount(string accountNumber, string ownerName, decimal initialDeposit)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            throw new ArgumentException("Account number cannot be null or empty.", nameof(accountNumber));
        }

        if (accountNumber.Trim().Length < 5)
        {
            throw new ArgumentException("Account number must be at least 5 characters long.", nameof(accountNumber));
        }

        if (string.IsNullOrWhiteSpace(ownerName))
        {
            throw new ArgumentException("Owner name cannot be null or empty.", nameof(ownerName));
        }

        if (initialDeposit < 0)
        {
            throw new ArgumentOutOfRangeException(nameof(initialDeposit), "Initial deposit cannot be negative.");
        }

        AccountNumber = accountNumber.Trim();
        OwnerName = ownerName.Trim();
        CreatedAt = DateTime.UtcNow;
        Balance = initialDeposit;

        if (initialDeposit > 0)
        {
            _transactions.Add(new Transaction(
                TransactionType.Deposit,
                initialDeposit,
                Balance,
                "Initial account opening deposit"
            ));
        }
    }

    /// <summary>
    /// Deposits funds into the account and appends to the transaction history.
    /// </summary>
    public void Deposit(decimal amount, string description)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Deposit amount must be greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Deposit description cannot be null or empty.", nameof(description));
        }

        Balance += amount;
        _transactions.Add(new Transaction(TransactionType.Deposit, amount, Balance, description));
    }

    /// <summary>
    /// Withdraws funds from the account if sufficient balance is available.
    /// </summary>
    public void Withdraw(decimal amount, string description)
    {
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Withdrawal amount must be greater than zero.");
        }

        if (amount > Balance)
        {
            throw new InvalidOperationException($"Insufficient funds. Current balance: {Balance:C}, Requested: {amount:C}");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Withdrawal description cannot be null or empty.", nameof(description));
        }

        Balance -= amount;
        _transactions.Add(new Transaction(TransactionType.Withdrawal, amount, Balance, description));
    }
}
