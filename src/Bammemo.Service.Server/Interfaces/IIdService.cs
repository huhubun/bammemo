namespace Bammemo.Service.Server.Interfaces;

public interface IIdService
{
    Task<int> DecodeAsync(string str);
    Task<string> EncodeAsync(int number);
}