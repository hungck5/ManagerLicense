using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

public static class RSAHelper
{
    public static void GenerateKeys(string publicPath, string privatePath)
    {
        using var rsa = RSA.Create(2048);
        var privateKey = rsa.ExportRSAPrivateKeyPem();
        var publicKey = rsa.ExportRSAPublicKeyPem();
        Directory.CreateDirectory(Path.GetDirectoryName(publicPath));
        File.WriteAllText(publicPath, publicKey);
        File.WriteAllText(privatePath, privateKey);
    }

    public static string SignData(License license, string privateKeyPem)
    {
        var data = JsonSerializer.Serialize(new { license.User, license.ExpirationDate });
        var dataBytes = Encoding.UTF8.GetBytes(data);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(privateKeyPem);

        var signature = rsa.SignData(dataBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
        return Convert.ToBase64String(signature);
    }

    public static bool VerifySignature(License license, string publicKeyPem)
    {
        var data = JsonSerializer.Serialize(new { license.LicenseKey, license.ExpirationDate, license.User, Signature = (string)null });
        var dataBytes = Encoding.UTF8.GetBytes(data);
        var signatureBytes = Convert.FromBase64String(license.Signature);

        using var rsa = RSA.Create();
        rsa.ImportFromPem(publicKeyPem);

        return rsa.VerifyData(dataBytes, signatureBytes, HashAlgorithmName.SHA256, RSASignaturePadding.Pkcs1);
    }
}