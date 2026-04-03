using System.Text;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// Base64 编解码器
    /// </summary>
    public class Base64Encoder : Protocol.IEncoder
    {
        public string Name => "Base64";

        // Base64 字符集
        private static readonly string Base64Chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/=";

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            byte[] bytes = Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(bytes);
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            try
            {
                byte[] bytes = Convert.FromBase64String(input.Trim());
                return Encoding.UTF8.GetString(bytes);
            }
            catch
            {
                return string.Empty;
            }
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            string trimmed = input.Trim();
            if (trimmed.Length < 4) return false;

            // 检查是否只包含 Base64 字符
            if (!trimmed.All(c => Base64Chars.Contains(c))) return false;

            // 检查长度是否为4的倍数
            if (trimmed.Length % 4 != 0) return false;

            // 尝试实际解码验证
            try
            {
                Convert.FromBase64String(trimmed);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public bool CanEncode(string input) => !string.IsNullOrEmpty(input);
    }
}