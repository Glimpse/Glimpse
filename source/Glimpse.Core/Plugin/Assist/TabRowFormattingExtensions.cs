using System;
using System.Linq;

namespace Glimpse.Core.Plugin.Assist
{
    public static class TabRowFormattingExtensions
    {
        public static T Strong<T>(this ITabStyleValue<T> sectionRow)
        {
            return sectionRow.ApplyValueStyle(Formats.Strong);
        }

        public static T StrongIf<T>(this ITabStyleValue<T> sectionRow, bool condition)
        {
            return condition ? sectionRow.Strong() : (T)sectionRow;
        }

        public static T Emphasis<T>(this ITabStyleValue<T> sectionRow)
        {
            return sectionRow.ApplyValueStyle(Formats.Emphasis);
        }

        public static T EmphasisIf<T>(this ITabStyleValue<T> sectionRow, bool condition)
        {
            return condition ? sectionRow.Emphasis() : (T)sectionRow;
        }

        public static T Raw<T>(this ITabStyleValue<T> sectionRow)
        {
            return sectionRow.ApplyValueStyle(Formats.Raw);
        }

        public static T RawIf<T>(this ITabStyleValue<T> sectionRow, bool condition)
        {
            return condition ? sectionRow.Raw() : (T)sectionRow;
        }

        public static T Sub<T>(this ITabStyleValue<T> sectionRow)
        {
            return sectionRow.ApplyValueStyle(Formats.Sub);
        }

        public static T SubIf<T>(this ITabStyleValue<T> sectionRow, bool condition)
        {
            return condition ? sectionRow.Sub() : (T)sectionRow;
        }

        public static T Underline<T>(this ITabStyleValue<T> sectionRow)
        {
            return sectionRow.ApplyValueStyle(Formats.Underline);
        }

        public static T UnderlineIf<T>(this ITabStyleValue<T> sectionRow, bool condition)
        {
            return condition ? sectionRow.Underline() : (T)sectionRow;
        }
         
        public static void Strong(this ITabStyleValue sectionRow)
        {
            sectionRow.ApplyValueStyle(Formats.Strong);
        }

        public static void StrongIf(this ITabStyleValue sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Strong();
            }
        }

        public static void Emphasis(this ITabStyleValue sectionRow)
        {
            sectionRow.ApplyValueStyle(Formats.Emphasis);
        }

        public static void EmphasisIf(this ITabStyleValue sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Emphasis();
            }
        }

        public static void Raw(this ITabStyleValue sectionRow)
        {
            sectionRow.ApplyValueStyle(Formats.Raw);
        }

        public static void RawIf(this ITabStyleValue sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Raw();
            }
        }

        public static void Sub(this ITabStyleValue sectionRow)
        {
            sectionRow.ApplyValueStyle(Formats.Sub);
        }

        public static void SubIf(this ITabStyleValue sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Sub();
            }
        }

        public static void Underline(this ITabStyleValue sectionRow)
        {
            sectionRow.ApplyValueStyle(Formats.Underline);
        }

        public static void UnderlineIf(this ITabStyleValue sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Underline();
            }
        }

        public static void Error(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Error);
        }

        public static void ErrorIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Error();
            }
        }

        public static void Fail(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Fail);
        }

        public static void FailIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Fail();
            } 
        }

        public static void Info(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Info);
        }

        public static void InfoIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Info();
            }  
        }

        public static void Loading(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Loading);
        }

        public static void LoadingIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Loading();
            }   
        }

        public static void Ms(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Ms);
        }

        public static void MsIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Ms();
            }
        }

        public static void Quiet(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Quiet);
        }

        public static void QuietIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Quiet();
            }
        }

        public static void Selected(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Selected);
        }

        public static void SelectedIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Selected();
            } 
        }
        
        public static void Warn(this ITabStyleRow sectionRow)
        {
            sectionRow.ApplyRowStyle(FormattingKeywords.Warn);
        }

        public static void WarnIf(this ITabStyleRow sectionRow, bool condition)
        {
            if (condition)
            {
                sectionRow.Warn();
            }  
        }

        public static void Style(this ITabStyleRow sectionRow, string style)
        {
            if (!string.IsNullOrEmpty(style))
            {
                sectionRow.ApplyRowStyle(style);
            }
        }

        public static void StyleIf(this ITabStyleRow sectionRow, string style, bool condition)
        {
            if (condition)
            {
                sectionRow.Style(style);
            }
        }

        public static void Style(this ITabStyleRow sectionRow, FormattingKeywordEnum keyword)
        {
            if (keyword != FormattingKeywordEnum.None)
            {
                sectionRow.ApplyRowStyle(FormattingKeywords.Convert(keyword));
            }
        }

        public static void StyleIf(this ITabStyleRow sectionRow, FormattingKeywordEnum keyword, bool condition)
        {
            if (condition)
            {
                sectionRow.Style(keyword);
            }
        } 
    }
}