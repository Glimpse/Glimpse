namespace Glimpse.Core.Plugin.Assist
{
    public static class FormattingKeywords
    {
        public const string Error = "error";
        public const string Fail = "fail";
        public const string Info = "info";
        public const string Loading = "loading";
        public const string Ms = "ms";
        public const string Quiet = "quiet";
        public const string Selected = "selected";
        public const string Warn = "warn";

        public static string Convert(FormattingKeywordEnum keyword)
        {
            switch (keyword)
            {
                case FormattingKeywordEnum.Error:
                    return Error;
                case FormattingKeywordEnum.Fail:
                    return Fail;
                case FormattingKeywordEnum.Info:
                    return Info;
                case FormattingKeywordEnum.Loading:
                    return Loading;
                case FormattingKeywordEnum.Quite:
                    return Quiet;
                case FormattingKeywordEnum.Selected:
                    return Selected;
                case FormattingKeywordEnum.System:
                    return Ms;
                case FormattingKeywordEnum.Warn:
                    return Warn;
                default:
                    return null;
            }
        }
    }
}