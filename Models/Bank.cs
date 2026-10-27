namespace SimpleBank.Models;

/// <summary>
/// Represents a banking institution that manages customer accounts.
/// Encapsulates all bank state using properties, without exposing public fields.
/// </summary>
public class Bank
{
    // Private dictionary storing accounts by account number
    private readonly Dictionary<string, BankAccount> _accounts = new(StringComparer.OrdinalIgnoreCase);

    // Encapsulated properties (no public fields)
    public string Name { get; }
    public string BranchCode { get; }

    // Read-only collection of accounts
    public IReadOnlyCollection<BankAccount> Accounts => _accounts.Values;

    // Computed property calculating total assets under management
    public decimal TotalDeposits => _accounts.Values.Sum(acc => acc.Balance);

    /// <summary>
    /// Initializes a new instance of <see cref="Bank"/> with constructor input validation.
    /// </summary>
    /// <param name="name">Name of the bank institution.</param>
    /// <param name="branchCode">A 4 to 8 alphanumeric character branch identifier.</param>
    /// <exception cref="ArgumentException">Thrown when name or branch code is null, empty, or invalid.</exception>
    public Bank(string name, string branchCode)
    {
        // Validate inputs
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new ArgumentException("Bank name cannot be null or empty.", nameof(name));
        }

        if (string.IsNullOrWhiteSpace(branchCode))
        {
            throw new ArgumentException("Branch code cannot be null or empty.", nameof(branchCode));
        }

        if (branchCode.Trim().Length < 4 || branchCode.Trim().Length > 8)
        {
            throw new ArgumentException("Branch code must be between 4 and 8 characters long.", nameof(branchCode));
        }

        Name = name.Trim();
        BranchCode = branchCode.Trim().ToUpperInvariant();
    }

    /// <summary>
    /// Opens and registers a new bank account.
    /// </summary>
    public void OpenAccount(BankAccount account)
    {
        ArgumentNullException.ThrowIfNull(account);

        if (_accounts.ContainsKey(account.AccountNumber))
        {
            throw new InvalidOperationException($"Account '{account.AccountNumber}' already exists in {Name}.");
        }

        _accounts[account.AccountNumber] = account;
    }

    /// <summary>
    /// Finds an account by account number.
    /// </summary>
    public BankAccount? FindAccount(string accountNumber)
    {
        if (string.IsNullOrWhiteSpace(accountNumber))
        {
            return null;
        }

        _accounts.TryGetValue(accountNumber.Trim(), out var account);
        return account;
    }
}
