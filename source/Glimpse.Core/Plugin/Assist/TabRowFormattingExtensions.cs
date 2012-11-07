using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public static class TabRowFormattingExtensions
    {
        public static TabSectionRow Strong(this TabSectionRow sectionRow)
        {
            return ApplyToLastColumn(sectionRow, Formats.Strong);
        }

        public static TabSectionRow StrongIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Strong() : sectionRow;
        }

        public static TabSectionRow Emphasis(this TabSectionRow sectionRow)
        {
            return ApplyToLastColumn(sectionRow, Formats.Emphasis);
        }

        public static TabSectionRow EmphasisIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Emphasis() : sectionRow;
        }

        public static TabSectionRow Raw(this TabSectionRow sectionRow)
        {
            return ApplyToLastColumn(sectionRow, Formats.Raw);
        }

        public static TabSectionRow RawIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Raw() : sectionRow;
        }

        public static TabSectionRow Sub(this TabSectionRow sectionRow)
        {
            return ApplyToLastColumn(sectionRow, Formats.Sub);
        }

        public static TabSectionRow SubIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Sub() : sectionRow;
        }

        public static TabSectionRow Underline(this TabSectionRow sectionRow)
        {
            return ApplyToLastColumn(sectionRow, Formats.Underline);
        }

        public static TabSectionRow UnderlineIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Underline() : sectionRow;
        }

        public static TabSectionRow Error(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Error);
        }

        public static TabSectionRow ErrorIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Error() : sectionRow;
        }

        public static TabSectionRow Fail(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Fail);
        }

        public static TabSectionRow FailIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Fail() : sectionRow;
        }

        public static TabSectionRow Info(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Info);
        }

        public static TabSectionRow InfoIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Info() : sectionRow;
        }

        public static TabSectionRow Loading(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Loading);
        }

        public static TabSectionRow LoadingIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Loading() : sectionRow;
        }

        public static TabSectionRow Ms(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Ms);
        }

        public static TabSectionRow MsIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Ms() : sectionRow;
        }

        public static TabSectionRow Quiet(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Quiet);
        }

        public static TabSectionRow QuietIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Quiet() : sectionRow;
        }

        public static TabSectionRow Selected(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Selected);
        }

        public static TabSectionRow SelectedIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Selected() : sectionRow;
        }
        
        public static TabSectionRow Warn(this TabSectionRow sectionRow)
        {
            return VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Warn);
        }

        public static TabSectionRow WarnIf(this TabSectionRow sectionRow, bool condition)
        {
            return condition ? sectionRow.Warn() : sectionRow;
        }

        public static TabSectionRow Style(this TabSectionRow sectionRow, string @class)
        {
            return !string.IsNullOrEmpty(@class) ? VerifyAndApplyFormatting(sectionRow, @class) : sectionRow;
        }

        public static TabSectionRow StyleIf(this TabSectionRow sectionRow, string @class, bool condition)
        {
            return condition ? sectionRow.Style(@class) : sectionRow;
        }

        public static TabSectionRow Style(this TabSectionRow sectionRow, FormattingKeywordEnum keyword)
        {
            return keyword != FormattingKeywordEnum.None ? VerifyAndApplyFormatting(sectionRow, FormattingKeywords.Convert(keyword)) : sectionRow;
        }

        public static TabSectionRow StyleIf(this TabSectionRow sectionRow, FormattingKeywordEnum keyword, bool condition)
        {
            return condition ? sectionRow.Style(keyword) : sectionRow;
        }

        private static TabSectionRow VerifyAndApplyFormatting(TabSectionRow sectionRow, string operation)
        {
            if (!sectionRow.Columns.Any())
            {
                throw new InvalidOperationException(string.Format("The operation '{0}' is only valid when row has columns.", operation));
            }

            sectionRow.Column(operation.ToLower());
            return sectionRow;
        }

        private static TabSectionRow ApplyToLastColumn(TabSectionRow sectionRow, string format)
        {
            var data = sectionRow.Columns.Last().Data;
            var formattedData = format.FormatWith(data);
            sectionRow.Columns.Last().OverrideData(formattedData);
            return sectionRow;
        }
    }
}