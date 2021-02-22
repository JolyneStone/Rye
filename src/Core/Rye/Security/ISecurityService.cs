namespace Rye.Security
{
    public interface ISecurityService
    {
        string Encrypt(int appId, string value);
        string Decrypt(int appId, string value);
        string Encrypt(string key, string iv, string value);
        string Decrypt(string key, string iv, string value);

        byte[] Encrypt(int appId, byte[] bytes);
        byte[] Decrypt(int appId, byte[] bytes);
        byte[] Encrypt(string key, string iv, byte[] bytes);
        byte[] Decrypt(string key, string iv, byte[] bytes);
    }
}
