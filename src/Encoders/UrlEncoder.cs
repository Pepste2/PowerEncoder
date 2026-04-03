using System.Web;
using System.Text.RegularExpressions;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// URL 编解码器
    /// 支持 %XX 格式的 URL 编码
    /// </summary>
    public class UrlEncoder : Protocol.IEncoder
    {
        public string Name => "URL";

        // 匹配 URL 编码格式 %XX
        private static readonly Regex UrlEncodedRegex = new(@"%[0-9A-Fa-f]{2}", RegexOptions.Compiled);

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return Uri.EscapeDataString(input);
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;
            return Uri.UnescapeDataString(input);
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return UrlEncodedRegex.IsMatch(input);
        }

        public bool CanEncode(string input) => !string.IsNullOrEmpty(input);
    }
}