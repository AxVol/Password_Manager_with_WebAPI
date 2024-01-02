namespace WebApi.Service.AlgoRU.KuznechikCypher;

public class BlockBuilder
{
    public static string BlockBuilderContainerFrom128(string[] inputBlocks)
    {
        string result = null;

        for (int i = 0; i < inputBlocks.Length; i++) 
        {
            result += inputBlocks[i];
        }

        return result;
    }
}