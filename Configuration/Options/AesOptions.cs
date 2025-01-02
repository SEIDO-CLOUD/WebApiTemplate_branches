namespace Configuration;

public class AesOptions
{
    public const string Position = "AesEcryptionDetails";
    public string Key { get; set; }
    public string Iv { get; set; }

    public byte[] KeyBytes { get; set; }
    public byte[] IvBytes { get; set; }
}