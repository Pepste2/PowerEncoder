using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugin.Logger;
using PowerEncoder.Encoders;

namespace PowerEncoder
{
    /// <summary>
    /// 编解码核心逻辑，负责自动识别输入并提供转换建议
    /// </summary>
    public class EncoderHelper
    {
        private readonly List<Protocol.IEncoder> encoders;
        private readonly IPublicAPI? publicAPI;

        public EncoderHelper(IPublicAPI? api = null)
        {
            publicAPI = api;
            encoders = new List<Protocol.IEncoder>
            {
                new UnicodeEncoder(),
                new UrlEncoder(),
                new TimestampEncoder(),
                new BinaryEncoder(),
                new HexEncoder(),
                new Base64Encoder()
            };
        }

        /// <summary>
        /// 查询输入字符串，返回所有可能的编解码结果
        /// </summary>
        public List<ResultItem> Query(string input)
        {
            var results = new List<ResultItem>();

            if (string.IsNullOrEmpty(input))
            {
                results.Add(new ResultItem
                {
                    Title = "输入文本进行编解码转换",
                    SubTitle = "支持: Unicode、URL、时间戳、二进制、十六进制、Base64"
                });
                return results;
            }

            // 自动检测解码选项
            foreach (var encoder in encoders)
            {
                if (encoder.CanDecode(input))
                {
                    string decoded = encoder.Decode(input);
                    if (!string.IsNullOrEmpty(decoded) && decoded != input)
                    {
                        results.Add(CreateDecodeResult(encoder, decoded));
                    }
                }
            }

            // 提供编码选项（对于所有非空输入）
            foreach (var encoder in encoders)
            {
                if (encoder.CanEncode(input))
                {
                    // 时间戳编码器只有日期格式才能编码
                    if (encoder is TimestampEncoder && !encoder.CanDecode(input))
                        continue;

                    string encoded = encoder.Encode(input);
                    if (!string.IsNullOrEmpty(encoded) && encoded != input)
                    {
                        results.Add(CreateEncodeResult(encoder, encoded));
                    }
                }
            }

            // 如果没有结果，显示提示
            if (results.Count == 0)
            {
                results.Add(new ResultItem
                {
                    Title = "无法识别的格式",
                    SubTitle = "尝试使用其他编码方式"
                });
            }

            return results;
        }

        /// <summary>
        /// 创建解码结果项
        /// </summary>
        private ResultItem CreateDecodeResult(Protocol.IEncoder encoder, string result)
        {
            return new ResultItem
            {
                Title = result,
                SubTitle = $"{encoder.Name} 解码",
                CopyContent = result,
                Action = ctx =>
                {
                    SetClipboard(result);
                    publicAPI?.ShowMsg("已复制", result);
                    return true;
                }
            };
        }

        /// <summary>
        /// 创建编码结果项
        /// </summary>
        private ResultItem CreateEncodeResult(Protocol.IEncoder encoder, string result)
        {
            return new ResultItem
            {
                Title = result,
                SubTitle = $"{encoder.Name} 编码",
                CopyContent = result,
                Action = ctx =>
                {
                    SetClipboard(result);
                    publicAPI?.ShowMsg("已复制", result);
                    return true;
                }
            };
        }

        /// <summary>
        /// 设置剪贴板内容
        /// </summary>
        private void SetClipboard(string text)
        {
            try
            {
                System.Windows.Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                Log.Error($"设置剪贴板失败: {ex.Message}", typeof(EncoderHelper));
            }
        }
    }
}