using BCrypt.Net;

Console.WriteLine("-- BCrypt Password Hashes (workFactor: 12)");
Console.WriteLine("-- Generated for HR System Employee Data");
Console.WriteLine();
Console.WriteLine("-- Username -> Password -> Hash");
Console.WriteLine();

var users = new Dictionary<string, string>
{
    { "admin", "Admin123!" },
    { "manager", "Manager123!" },
    { "hr", "HR123!" },
    { "employee1", "Password123!" },
    { "employee2", "Password123!" },
    { "employee3", "Password123!" }
};

foreach (var user in users)
{
    var hash = BCrypt.Net.BCrypt.HashPassword(user.Value, workFactor: 12);
    Console.WriteLine($"-- {user.Key} / {user.Value}");
    Console.WriteLine($"'{hash}'");
    Console.WriteLine();
}

Console.WriteLine();
Console.WriteLine("-- SQL UPDATE statements:");
Console.WriteLine();

foreach (var user in users)
{
    var hash = BCrypt.Net.BCrypt.HashPassword(user.Value, workFactor: 12);
    Console.WriteLine($"UPDATE Tbl_Employee SET Password = '{hash}' WHERE Username = '{user.Key}';");
}

