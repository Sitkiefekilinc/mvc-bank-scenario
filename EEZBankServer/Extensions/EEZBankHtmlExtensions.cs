using EEZBankServer.Models;
using Microsoft.AspNetCore.Html;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace EEZBankServer.Extensions
{
    public static class EEZBankHtmlExtensions
    {
        public static IHtmlContent EezInputGroup(this IHtmlHelper htmlHelper, string name, string label, string placeholder, string inputType = "text")
        {
            var divBuilder = new TagBuilder("div");
            divBuilder.MergeAttribute("style", "margin-bottom:20px;");

            var labelBuilder = new TagBuilder("label");
            labelBuilder.InnerHtml.Append(label);

            var inputBuilder = new TagBuilder("input");
            inputBuilder.Attributes.Add("type", inputType);
            inputBuilder.Attributes.Add("name", name);
            inputBuilder.AddCssClass("form-control");
            inputBuilder.Attributes.Add("placeholder", placeholder);
            inputBuilder.Attributes.Add("required", "required");

            divBuilder.InnerHtml.AppendHtml(labelBuilder);
            divBuilder.InnerHtml.AppendHtml(inputBuilder);

            return divBuilder;
        }



    }
}
