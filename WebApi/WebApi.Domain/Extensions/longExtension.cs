namespace WebApi.Domain.Extensions
{
    public static class LongExtension
    {
        public static byte[] ToBigEndianBytes(this long segmentIndex)
        {
            byte[] bytes = BitConverter.GetBytes(Convert.ToUInt64(segmentIndex));

            if (BitConverter.IsLittleEndian)
                Array.Reverse(bytes);

            return bytes;
        }
    }
}
