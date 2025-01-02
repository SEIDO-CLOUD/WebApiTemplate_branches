using System.Security.Cryptography;
using System.Text;
using Configuration;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace Services;

public class EncryptionService 
{
    private readonly ILogger<EncryptionService> _logger;
    private readonly PasswordOptions _passwordOptions;
    private readonly AesOptions _aesOption;
    
    public EncryptionService(ILogger<EncryptionService> logger, IOptions<PasswordOptions> passwordOptions, IOptions<AesOptions> aesOptions)
    {
        _logger = logger;
        _passwordOptions = passwordOptions.Value;
        _aesOption = aesOptions.Value;
        _aesOption.KeyBytes = Pbkdf2HashToBytes(16, _aesOption.Key);
        _aesOption.IvBytes = Pbkdf2HashToBytes(16, _aesOption.Iv);
    }

    public string AesEncryptToBase64<T> (T sourceToEncrypt) 
    {
        string stringToEncrypt = JsonConvert.SerializeObject(this);    
        byte[] dataset = System.Text.Encoding.Unicode.GetBytes(stringToEncrypt);

        //Encrypt using AES
        byte[] encryptedBytes;
        using (SymmetricAlgorithm algorithm = Aes.Create())
        using (ICryptoTransform encryptor = algorithm.CreateEncryptor(_aesOption.KeyBytes, _aesOption.IvBytes))
        {
            encryptedBytes = encryptor.TransformFinalBlock(dataset, 0, dataset.Length);
        }
        
        return Convert.ToBase64String(encryptedBytes);
    }

    public T AesDecryptToBase64<T> (string encryptedBase64) 
    {
        byte[] encryptedBytes = Convert.FromBase64String(encryptedBase64);

        byte[] decryptedBytes;
        using (SymmetricAlgorithm algorithm = Aes.Create())
        using (ICryptoTransform decryptor = algorithm.CreateDecryptor(_aesOption.KeyBytes, _aesOption.IvBytes))
        {
            decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
        }
        
        string decryptedString = System.Text.Encoding.Unicode.GetString(decryptedBytes);
        T decryptedObject = JsonConvert.DeserializeObject<T>(decryptedString);
                
        return decryptedObject;
    }

    public byte[] Pbkdf2HashToBytes (int nrBytes, string Password)
    {
        byte[] registeredPasswordKeyDerivation = KeyDerivation.Pbkdf2(
        password: Password,
        salt: Encoding.UTF8.GetBytes(_passwordOptions.Salt),
        prf: KeyDerivationPrf.HMACSHA512,
        iterationCount: _passwordOptions.Iterations,
        numBytesRequested: nrBytes);

        return registeredPasswordKeyDerivation;
    }
}