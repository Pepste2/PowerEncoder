namespace PowerEncoder.Protocol
{
    /// <summary>
    /// 编码器接口，所有编解码器都实现此接口
    /// </summary>
    public interface IEncoder
    {
        /// <summary>
        /// 编码器名称，用于显示
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 编码输入字符串
        /// </summary>
        string Encode(string input);

        /// <summary>
        /// 解码输入字符串
        /// </summary>
        string Decode(string input);

        /// <summary>
        /// 判断输入是否可以被解码（检测输入格式）
        /// </summary>
        bool CanDecode(string input);

        /// <summary>
        /// 判断输入是否可以被编码（有些格式有特殊要求）
        /// </summary>
        bool CanEncode(string input);
    }
}