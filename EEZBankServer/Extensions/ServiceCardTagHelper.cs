using Microsoft.AspNetCore.Razor.TagHelpers;

namespace EEZBank.TagHelpers 
{
    [HtmlTargetElement("service-icon")]
    public class ServiceIconTagHelper : TagHelper
    {
        public string Symbol { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            output.TagName = "div";
            output.Attributes.SetAttribute("class", "service-icon");
            output.Content.SetHtmlContent($"<i class='fas fa-{Symbol}'></i>");
        }
    }

}