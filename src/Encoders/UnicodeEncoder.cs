using System.Text;
using System.Text.RegularExpressions;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// Unicode 编解码器
    /// 支持 \uXXXX 格式和 &#XXXX; 格式
    /// </summary>
    public class UnicodeEncoder : Protocol.IEncoder
    {
        public string Name => "Unicode";

        // 匹配 \uXXXX 格式
        private static readonly Regex UnicodeEscapeRegex = new(@"\\u([0-9A-Fa-f]{4})", RegexOptions.Compiled);
        // 匹配 &#XXXX; 或 &#xXXXX; 格式
        private static readonly Regex HtmlEntityRegex = new(@"&#x([0-9A-Fa-f]+);|&#(\d+);", RegexOptions.Compiled);

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            var result = new StringBuilder();
            foreach (char c in input)
            {
                result.AppendFormat("\\u{0:X4}", (int)c);
            }
            return result.ToString();
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 处理 \uXXXX 格式
            string result = UnicodeEscapeRegex.Replace(input, match =>
            {
                int codePoint = int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
                return char.ConvertFromUtf32(codePoint);
            });

            // 处理 HTML 实体格式 &#xXXXX; 或 &#XXXX;
            result = HtmlEntityRegex.Replace(result, match =>
            {
                if (match.Groups[1].Success) // &#xXXXX; 十六进制
                {
                    int codePoint = int.Parse(match.Groups[1].Value, System.Globalization.NumberStyles.HexNumber);
                    return char.ConvertFromUtf32(codePoint);
                }
                else if (match.Groups[2].Success) // &#XXXX; 十进制
                {
                    int codePoint = int.Parse(match.Groups[2].Value);
                    return char.ConvertFromUtf32(codePoint);
                }
                return match.Value;
            });

            return result;
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return UnicodeEscapeRegex.IsMatch(input) || HtmlEntityRegex.IsMatch(input);
        }

        public bool CanEncode(string input) => !string.IsNullOrEmpty(input);
    }
}