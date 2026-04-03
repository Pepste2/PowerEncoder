using System.Text;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// 十六进制编解码器
    /// 将 ASCII 字符转换为十六进制表示
    /// </summary>
    public class HexEncoder : Protocol.IEncoder
    {
        public string Name => "Hex";

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = new StringBuilder();
            foreach (char c in input)
            {
                result.AppendFormat("{0:X2}", (byte)c);
            }
            return result.ToString();
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 移除空格和其他分隔符
            input = input.Replace(" ", "").Replace("-", "").Replace(":", "");

            var result = new StringBuilder();
            for (int i = 0; i < input.Length; i += 2)
            {
                if (i + 2 > input.Length) break;

                string hexStr = input.Substring(i, 2);
                if (IsHexString(hexStr))
                {
                    int charCode = int.Parse(hexStr, System.Globalization.NumberStyles.HexNumber);
                    result.Append((char)charCode);
                }
            }
            return result.ToString();
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            // 移除分隔符后检查
            string clean = input.Replace(" ", "").Replace("-", "").Replace(":", "");
            if (clean.Length < 2 || clean.Length % 2 != 0) return false;

            return IsHexString(clean);
        }

        public bool CanEncode(string input) => !string.IsNullOrEmpty(input);

        private static bool IsHexString(string s)
        {
            return s.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
        }
    }
}