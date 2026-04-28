using Npgsql;

// Connection string - 'localhost' works because of shared network mode
var connectionString = "Host=localhost;Username=postgres;Password=password123;Database=CustomersDB";

using var dataSource = NpgsqlDataSource.Create(connectionString);

// 1. Setup Table
await using (var cmd = dataSource.CreateCommand("CREATE TABLE IF NOT EXISTS Customers (id SERIAL PRIMARY KEY, name TEXT)"))
{
    await cmd.ExecuteNonQueryAsync();
}

// 2. Insert a Customer
Console.Write("Enter customer name: ");
var name = Console.ReadLine() ?? "Unknown";

await using (var cmd = dataSource.CreateCommand("INSERT INTO Customers (name) VALUES ($1)"))
{
    cmd.Parameters.AddWithValue(name);
    await cmd.ExecuteNonQueryAsync();
    Console.WriteLine($"Inserted: {name}");
}

// 3. Display Customers
Console.WriteLine("\n--- Customer List ---");
await using (var cmd = dataSource.CreateCommand("SELECT id, name FROM Customers"))
await using (var reader = await cmd.ExecuteReaderAsync())
{
    while (await reader.ReadAsync())
    {
        Console.WriteLine($"{reader.GetInt32(0)}: {reader.GetString(1)}");
    }
}