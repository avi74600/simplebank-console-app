namespace SimpleBank.Models;

/// <summary>
/// Represents an immutable financial transaction recorded against a bank account.
/// Adheres to object-oriented encapsulation: uses properties, not public fields.
/// </summary>
public class Transaction
{
    // Properties with encapsulation (read-only, no public fields)
    public Guid Id { get; }
    public DateTime Timestamp { get; }
    public TransactionType Type { get; }
    public decimal Amount { get; }
    public decimal BalanceAfter { get; }
    public string Description { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Transaction"/> class with strict input validation.
    /// </summary>
    /// <param name="type">Type of transaction (Deposit or Withdrawal).</param>
    /// <param name="amount">Transaction amount (must be strictly greater than zero).</param>
    /// <param name="balanceAfter">The running balance after this transaction occurred.</param>
    /// <param name="description">A descriptive note explaining the transaction.</param>
    /// <exception cref="ArgumentOutOfRangeException">Thrown when amount is less than or equal to zero.</exception>
    /// <exception cref="ArgumentException">Thrown when description is null or whitespace.</exception>
    public Transaction(TransactionType type, decimal amount, decimal balanceAfter, string description)
    {
        // Constructor validation
        if (amount <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(amount), "Transaction amount must be strictly greater than zero.");
        }

        if (string.IsNullOrWhiteSpace(description))
        {
            throw new ArgumentException("Transaction description cannot be null or whitespace.", nameof(description));
        }

        Id = Guid.NewGuid();
        Timestamp = DateTime.UtcNow;
        Type = type;
        Amount = amount;
        BalanceAfter = balanceAfter;
        Description = description.Trim();
    }

    public override string ToString()
    {
        string prefix = Type == TransactionType.Deposit ? "+" : "-";
        return $"{Timestamp:yyyy-MM-dd HH:mm:ss} | {Type,-10} | {prefix}{Amount,10:C} | Balance: {BalanceAfter,10:C} | {Description}";
    }
}
