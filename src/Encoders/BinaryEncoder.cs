using System.Text;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// 二进制编解码器
    /// 将 ASCII 字符转换为二进制表示（每个字符8位）
    /// </summary>
    public class BinaryEncoder : Protocol.IEncoder
    {
        public string Name => "Binary";

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = new StringBuilder();
            foreach (char c in input)
            {
                // 将每个字符转为8位二进制
                result.Append(Convert.ToString((byte)c, 2).PadLeft(8, '0'));
            }
            return result.ToString();
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 移除空格和其他分隔符
            input = input.Replace(" ", "").Replace("-", "");

            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i += 8)
            {
                if (i + 8 > input.Length) break;

                string byteStr = input.Substring(i, 8);
                if (byteStr.All(c => c == '0' || c == '1'))
                {
                    int charCode = Convert.ToInt32(byteStr, 2);
                    result.Append((char)charCode);
                }
            }
            return result.ToString();
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            // 移除分隔符后检查
            string clean = input.Replace(" ", "").Replace("-", "");
            if (clean.Length < 8 || clean.Length % 8 != 0) return false;

            // 必须只包含 0 和 1
            return clean.All(c => c == '0' || c == '1');
        }

        public bool CanEncode(string input) => !string.IsNullOrEmpty(input);
    }
}