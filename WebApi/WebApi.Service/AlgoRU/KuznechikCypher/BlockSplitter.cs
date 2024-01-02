namespace WebApi.Service.AlgoRU.KuznechikCypher;

public class BlockSplitter
{
    public static string[] BlockSplitterContainer128(string inputText)
    {	
        //Chuck size is 32 for 128-bit text
        int chunkSize = 32;
        int inputStringSize = inputText.Length;
        double doubleVarChunkCount = (double)inputStringSize / (double)chunkSize;
        int chunkCount = (int)Math.Ceiling(doubleVarChunkCount);
        
        if (inputStringSize < chunkCount * chunkSize) 
        {
            string zeroChar = "0";
            
            for (int j = inputStringSize; j < chunkCount * chunkSize; j++) 
            {
                inputText += zeroChar; 
            }
        }

        string[] block = new string[chunkCount];
        
        for (int i = 0; i < chunkCount; i++) 
        {
            block[i] = inputText.Substring(i * chunkSize, chunkSize);
        }
        
        return block;
    }
}