using System;
using System.Web.WebPages;

public static class ViewExtensions
{
    private static readonly object _o = new object();

    public static HelperResult RenderSection(this WebPageBase page, string sectionName, Func<object, HelperResult> defaultContent)
    {
        if (page.IsSectionDefined(sectionName)) 
            return page.RenderSection(sectionName); 
        return defaultContent(_o); 
    }
}