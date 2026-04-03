using System.Globalization;

namespace PowerEncoder.Encoders
{
    /// <summary>
    /// Unix 时间戳转换器
    /// 支持秒级(10位)和毫秒级(13位)时间戳与日期时间互转
    /// </summary>
    public class TimestampEncoder : Protocol.IEncoder
    {
        public string Name => "Timestamp";

        // Unix 时间戳的合理范围 (1970-2100)
        private const long MinSecondTimestamp = 0;
        private const long MaxSecondTimestamp = 4102444800; // 2100-01-01
        private const long MinMillisecondTimestamp = 0;
        private const long MaxMillisecondTimestamp = 4102444800000;

        public string Encode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            // 尝试解析为日期时间
            if (DateTime.TryParse(input, out DateTime dt))
            {
                // 转为秒级时间戳
                long timestamp = new DateTimeOffset(dt).ToUnixTimeSeconds();
                return timestamp.ToString();
            }
            return string.Empty;
        }

        public string Decode(string input)
        {
            if (string.IsNullOrEmpty(input)) return string.Empty;

            if (!long.TryParse(input, out long timestamp)) return string.Empty;

            DateTime result;
            string formatDesc;

            // 判断是秒级还是毫秒级
            if (timestamp >= MinMillisecondTimestamp && timestamp <= MaxMillisecondTimestamp)
            {
                if (timestamp > MaxSecondTimestamp)
                {
                    // 毫秒级时间戳
                    result = DateTimeOffset.FromUnixTimeMilliseconds(timestamp).LocalDateTime;
                    formatDesc = "毫秒级时间戳";
                }
                else
                {
                    // 尝试秒级
                    result = DateTimeOffset.FromUnixTimeSeconds(timestamp).LocalDateTime;
                    formatDesc = "秒级时间戳";
                }
            }
            else
            {
                return string.Empty;
            }

            return $"{result:yyyy-MM-dd HH:mm:ss} ({formatDesc})";
        }

        public bool CanDecode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;

            // 检查是否为纯数字，长度10-13位
            if (!long.TryParse(input, out long timestamp)) return false;
            if (input.Length < 9 || input.Length > 14) return false;

            // 检查是否在合理范围内
            return timestamp >= MinSecondTimestamp && timestamp <= MaxMillisecondTimestamp;
        }

        public bool CanEncode(string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            return DateTime.TryParse(input, out _);
        }
    }
}