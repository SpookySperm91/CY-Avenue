namespace CarlosYulo.backend.monolith.common;

using System;
using System.Security.Cryptography;
using System.Text;

public class PasswordHashing
{
    // Method to generate a salted and hashed password
    public void GenerateHashedPassword(SystemAccount account, string plainPassword)
    {
        // Generate a salt
        byte[] salt = GenerateSalt();

        // Hash the password with the salt
        byte[] hash = HashPasswordWithSalt(plainPassword, salt);

        // Convert the salt and hash to Base64 strings for storage
        account.PasswordSalt = Convert.ToBase64String(salt);
        account.Password = Convert.ToBase64String(hash);
    }

    // Method to verify if a password matches the stored hash
    public bool VerifyPassword(SystemAccount account, string enteredPassword)
    {
        // Convert the stored salt back from Base64
        byte[] salt = Convert.FromBase64String(account.PasswordSalt);

        // Hash the entered password with the stored salt
        byte[] hash = HashPasswordWithSalt(enteredPassword, salt);

        // Compare the stored hash with the newly generated hash
        string enteredHash = Convert.ToBase64String(hash);
        return enteredHash == account.Password;
    }

    // Generate a random salt 
    private byte[] GenerateSalt(int size = 16)
    {
        byte[] salt = new byte[size];
        using (var rng = new RNGCryptoServiceProvider())
        {
            rng.GetBytes(salt);
        }

        return salt;
    }

    // Hash the password using SHA256 with a salt
    private byte[] HashPasswordWithSalt(string password, byte[] salt)
    {
        using (var sha256 = SHA256.Create())
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] passwordWithSalt = new byte[passwordBytes.Length + salt.Length];

            // Combine password and salt bytes
            Buffer.BlockCopy(salt, 0, passwordWithSalt, 0, salt.Length);
            Buffer.BlockCopy(passwordBytes, 0, passwordWithSalt, salt.Length, passwordBytes.Length);

            // Return the hashed result
            return sha256.ComputeHash(passwordWithSalt);
        }
    }
}