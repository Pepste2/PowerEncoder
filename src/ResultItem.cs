using Wox.Plugin;

namespace PowerEncoder
{
    /// <summary>
    /// 结果项，用于显示在 PowerToys Run 结果列表中
    /// </summary>
    public class ResultItem
    {
        /// <summary>
        /// 主标题，显示转换结果
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// 子标题，显示操作类型说明
        /// </summary>
        public string SubTitle { get; set; } = string.Empty;

        /// <summary>
        /// 要复制到剪贴板的内容（默认使用 Title）
        /// </summary>
        public string CopyContent { get; set; } = string.Empty;

        /// <summary>
        /// 图标路径
        /// </summary>
        public string? IconPath { get; set; }

        /// <summary>
        /// 点击后的操作
        /// </summary>
        public Func<ActionContext, bool>? Action { get; set; }
    }
}