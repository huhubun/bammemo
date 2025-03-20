using Bammemo.Service.Abstractions.Enums;

namespace Bammemo.Service.Interfaces;

public interface ISecurityService
{
    void GenerateAesKeyToLocalStorage();
    string Encrypt(string plainText);
    string Decrypt(string cipherText);
    KeySource GetKeySource();
}
