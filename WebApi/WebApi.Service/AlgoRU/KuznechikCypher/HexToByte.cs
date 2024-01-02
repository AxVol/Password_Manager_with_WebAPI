namespace WebApi.Service.AlgoRU.KuznechikCypher;

public class HexToByte
{
    public static byte[] HexToByteConvert(string inputString)
    {
        if (inputString.Length == 0 || inputString.Length % 2 != 0)
            return new byte[0];

        byte[] buffer = new byte[inputString.Length / 2];
        char c;
        
        for (int bx = 0, sx = 0; bx < buffer.Length; ++bx, ++sx)
        {
            // Convert first half of byte
            c = inputString[sx];
            buffer[bx] = (byte)((c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0')) << 4);

            // Convert second half of byte
            c = inputString[++sx];
            buffer[bx] |= (byte)(c > '9' ? (c > 'Z' ? (c - 'a' + 10) : (c - 'A' + 10)) : (c - '0'));
        }
        return buffer;
    }
}