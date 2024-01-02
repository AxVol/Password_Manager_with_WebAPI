namespace WebApi.Service.AlgoRU.KuznechikCypher;

public interface IKuznechik
{   
    void SetKey(byte[] key);
    byte[] Encrypt(byte[] data);
    byte[] Decrypt(byte[] data);
}