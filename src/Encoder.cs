using System;
using System.Collections.Generic;
using Wox.Plugin;
using Wox.Plugin.Logger;
using ManagedCommon;

namespace PowerEncoder
{
    /// <summary>
    /// PowerToys Run 编解码插件主入口
    /// </summary>
    public class Encoder : IPlugin, IDisposable, IDelayedExecutionPlugin
    {
        public string Name => "PowerEncoder";
        public static string PluginID => "POWERENCODER123456789ABCDEFG";
        public string Description => "编解码转换工具 - Unicode、URL、时间戳、二进制、十六进制、Base64";

        private PluginInitContext? context;
        private EncoderHelper? helper;
        private string iconPath = "Images/encoder.dark.png";

        /// <summary>
        /// 普通查询（立即执行）- 返回空列表，实际逻辑在延迟查询中
        /// </summary>
        public List<Result> Query(Query query)
        {
            return new List<Result>();
        }

        /// <summary>
        /// 延迟查询（防抖，避免频繁触发）
        /// </summary>
        public List<Result> Query(Query query, bool delayedExecution)
        {
            if (!delayedExecution) return new List<Result>();

            var querySearch = query.Search;
            if (string.IsNullOrEmpty(querySearch))
            {
                return new List<Result>
                {
                    new Result
                    {
                        Title = "输入文本进行编解码转换",
                        SubTitle = "支持: Unicode、URL、时间戳、二进制、十六进制、Base64",
                        IcoPath = iconPath
                    }
                };
            }

            return ConvertResults(helper?.Query(querySearch) ?? new List<ResultItem>());
        }

        public void Init(PluginInitContext ctx)
        {
            Log.Info("PowerEncoder init", typeof(Encoder));
            context = ctx;
            helper = new EncoderHelper(ctx.API);

            ctx.API.ThemeChanged += UpdateIconPath;
            UpdateIconPath(Theme.Light, ctx.API.GetCurrentTheme());
        }

        private void UpdateIconPath(Theme pre, Theme now)
        {
            if (now == Theme.Light || now == Theme.HighContrastWhite)
            {
                iconPath = "Images/encoder.light.png";
            }
            else
            {
                iconPath = "Images/encoder.dark.png";
            }
        }

        public void Dispose()
        {
            if (context != null)
            {
                context.API.ThemeChanged -= UpdateIconPath;
            }
        }

        /// <summary>
        /// 将内部结果转换为 PowerToys Run Result 格式
        /// </summary>
        private List<Result> ConvertResults(List<ResultItem> items)
        {
            var results = new List<Result>();
            foreach (var item in items)
            {
                results.Add(new Result
                {
                    Title = item.Title,
                    SubTitle = item.SubTitle,
                    IcoPath = item.IconPath ?? iconPath,
                    Action = c =>
                    {
                        SetClipboard(item.CopyContent ?? item.Title);
                        context?.API.ShowMsg("已复制", item.Title);
                        return true;
                    }
                });
            }
            return results;
        }

        private void SetClipboard(string text)
        {
            try
            {
                System.Windows.Clipboard.SetText(text);
            }
            catch (Exception ex)
            {
                Log.Error($"设置剪贴板失败: {ex.Message}", typeof(Encoder));
            }
        }
    }
}