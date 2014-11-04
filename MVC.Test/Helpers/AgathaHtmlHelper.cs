using System;
using System.Text;
using System.Web.Mvc;

namespace MVC.Test.Helpers
{
    public static class TestHtmlHelper
    {
        public static string BuildPageLinksFrom(this HtmlHelper html, int currentPage,
                                       int totalPages, Func<int, string> pageUrl)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 1; i <= totalPages; i++)
            {
                TagBuilder tag = new TagBuilder("a");
                tag.MergeAttribute("href", pageUrl(i));
                tag.InnerHtml = i.ToString();
                if (i == currentPage)
                    tag.AddCssClass("selected");
                else
                    tag.AddCssClass("notselected");
                result.AppendLine(tag.ToString());
            }

            return result.ToString();
        }

        public static string Resolve(this HtmlHelper html, string resource)
        {
            return Agathas.Storefront.Infrastructure.Helpers.UrlHelper.Resolve(resource);
        }
    }
}
