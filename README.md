# SimpleBank Console App (.NET)

**Level 1 of 4 · Warm-up — One sitting, one concept**

A clean, object-oriented .NET console application modeling a core banking domain (`Bank`, `BankAccount`, and `Transaction`). Designed specifically to demonstrate encapsulation, defensive constructor input validation, and readable console reporting.

---

## 🎯 Requirements Checklist

| Requirement | Implementation Detail | Status |
| :--- | :--- | :---: |
| **Two or three classes** | Models 3 core domain classes (`Bank`, `BankAccount`, `Transaction`) and an enum (`TransactionType`) | ✅ Met |
| **Properties, not public fields** | All classes use auto-implemented properties (`{ get; }`, `{ get; private set; }`), private fields, and read-only collection wrappers (`IReadOnlyList<Transaction>`) | ✅ Met |
| **Constructor input validation** | Defensive validation in every constructor throwing `ArgumentException` and `ArgumentOutOfRangeException` for invalid states | ✅ Met |
| **Readable console output** | Clean structured output with banners, account statements, tabular transaction ledger, and validation test reports | ✅ Met |
| **Builds and runs** | Configured with modern .NET SDK (`net10.0` / `net8.0`) without external runtime dependencies | ✅ Met |

---

## 🏛️ Domain Architecture

```
Models/
├── TransactionType.cs   # Enum defining transaction operations (Deposit, Withdrawal)
├── Transaction.cs       # Immutable record of a financial transaction
├── BankAccount.cs       # Account state with balance management and history
└── Bank.cs              # Financial institution managing multiple customer accounts
```

### 1. `Transaction`
- **Properties**:
  - `Id` (`Guid`): Unique identifier for auditability.
  - `Timestamp` (`DateTime`): Time the transaction was logged.
  - `Type` (`TransactionType`): `Deposit` or `Withdrawal`.
  - `Amount` (`decimal`): Transaction amount.
  - `BalanceAfter` (`decimal`): Running account balance after the transaction.
  - `Description` (`string`): Mandatory note describing the transaction.
- **Constructor Validation**:
  - `amount > 0`: Prevents non-positive or negative amounts (`ArgumentOutOfRangeException`).
  - `!string.IsNullOrWhiteSpace(description)`: Requires a valid description (`ArgumentException`).

### 2. `BankAccount`
- **Properties**:
  - `AccountNumber` (`string`): Read-only unique account identifier.
  - `OwnerName` (`string`): Read-only owner name.
  - `Balance` (`decimal`): Private setter to enforce business rules (`Deposit`, `Withdraw`).
  - `CreatedAt` (`DateTime`): UTC creation timestamp.
  - `Transactions` (`IReadOnlyList<Transaction>`): Read-only view of internal `List<Transaction>` preventing external mutation.
- **Constructor Validation**:
  - `accountNumber`: Cannot be null/whitespace and must be at least 5 characters (`ArgumentException`).
  - `ownerName`: Cannot be null/whitespace (`ArgumentException`).
  - `initialDeposit`: Cannot be negative (`ArgumentOutOfRangeException`).

### 3. `Bank`
- **Properties**:
  - `Name` (`string`): Name of the bank institution.
  - `BranchCode` (`string`): Normalized branch code.
  - `Accounts` (`IReadOnlyCollection<BankAccount>`): Collection of open accounts.
  - `TotalDeposits` (`decimal`): Computed property summing all active account balances.
- **Constructor Validation**:
  - `name`: Cannot be null/whitespace (`ArgumentException`).
  - `branchCode`: Must be between 4 and 8 characters (`ArgumentException`).

---

## 💡 Key Design Decisions

1. **Immutability for Transactions**:
   Financial transactions represent historical facts that should never change once executed. All properties on `Transaction` are get-only, and the `BankAccount` exposes its history as `IReadOnlyList<Transaction>` using `.AsReadOnly()`.

2. **Decimal Precision**:
   Financial figures use `decimal` instead of `double` or `float` to avoid floating-point rounding errors.

3. **Defensive Constructor Validation**:
   Constructors reject bad data immediately at instantiation time (fail fast) rather than allowing an object to exist in an inconsistent state.

4. **Demonstrating Validation in `Program.cs`**:
   The console application not only demonstrates the "happy path" (creating accounts, depositing, withdrawing, printing formatted statements), but also programmatically tests each constructor validation rule, catching and printing expected exceptions with green success indicators.

---

## 🚀 How to Run

### Prerequisites
- [.NET SDK](https://dotnet.microsoft.com/download) (version 8.0, 9.0, or 10.0)

### Clone & Run
```bash
# 1. Clone the repository
git clone <REPO_URL>
cd <REPO_FOLDER>

# 2. Build the project
dotnet build

# 3. Run the console application
dotnet run
```

---

## 📋 Sample Output

```text
==================================================================
                SIMPLEBANK CONSOLE APPLICATION                   
             Level 1 of 4 · Warm-up Implementation                
==================================================================

╔══════════════════════════════════════════════════════════════════╗
║ 1. Initializing Bank                                             ║
╚══════════════════════════════════════════════════════════════════╝
[SUCCESS] Created Bank: Apex Trust Bank (Branch: ATB01)

╔══════════════════════════════════════════════════════════════════╗
║ 2. Creating Customer Bank Accounts                               ║
╚══════════════════════════════════════════════════════════════════╝
[OPENED] Alice Johnson | Acc: ACC-1001 | Starting Balance: $1,500.00
[OPENED] Bob Smith     | Acc: ACC-1002 | Starting Balance: $500.00

...

╔══════════════════════════════════════════════════════════════════╗
║ 6. Constructor Input Validation Verification                     ║
╚══════════════════════════════════════════════════════════════════╝
Testing defensive programming rules to verify invalid inputs are rejected:

[PASSED] Empty Account Number -> Caught ArgumentException: accountNumber - Account number cannot be null or empty. (Parameter 'accountNumber')
[PASSED] Short Account Number (< 5 chars) -> Caught ArgumentException: accountNumber - Account number must be at least 5 characters long. (Parameter 'accountNumber')
[PASSED] Null/Empty Owner Name -> Caught ArgumentException: ownerName - Owner name cannot be null or empty. (Parameter 'ownerName')
[PASSED] Negative Initial Balance -> Caught ArgumentOutOfRangeException: initialDeposit - Initial deposit cannot be negative. (Parameter 'initialDeposit')
[PASSED] Invalid Bank Branch Code -> Caught ArgumentException: branchCode - Branch code must be between 4 and 8 characters long. (Parameter 'branchCode')
[PASSED] Zero or Negative Transaction Amount -> Caught ArgumentOutOfRangeException: amount - Transaction amount must be strictly greater than zero. (Parameter 'amount')
[PASSED] Empty Transaction Description -> Caught ArgumentException: description - Transaction description cannot be null or whitespace. (Parameter 'description')
==================================================================
   All tests completed successfully. Application is fully operational!
==================================================================
```
