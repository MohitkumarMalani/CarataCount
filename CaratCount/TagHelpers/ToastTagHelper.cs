using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CaratCount.TagHelpers
{
    [HtmlTargetElement("toast")]
    public class ToastTagHelper : TagHelper
    {
        // Prop that gives us access to the ViewContext
        [ViewContext()]
        [HtmlAttributeNotBound()]
        public ViewContext ViewContext { get; set; }

        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext.TempData.ContainsKey("ToastMessage"))
            {

                // status and message from TempData
                string? status = ViewContext?.TempData["ToastStatus"]?.ToString();
                string? message = ViewContext?.TempData["ToastMessage"]?.ToString();

                // close button
                var closeButton = new TagBuilder("button");
                closeButton.Attributes.Add("type", "button");
                closeButton.Attributes.Add("class", "btn-close me-2 m-auto");
                closeButton.Attributes.Add("data-bs-dismiss", "toast");
                closeButton.Attributes.Add("aria-label", "Close");

                // body div
                var bodyDiv = new TagBuilder("div");
                bodyDiv.Attributes.Add("class", "toast-body");
                bodyDiv.InnerHtml.Append(message);


                // inner div
                var innerDiv = new TagBuilder("div");
                innerDiv.Attributes.Add("class", "d-flex");

                // appending body div and close button to inner div
                innerDiv.InnerHtml.AppendHtml(bodyDiv);
                innerDiv.InnerHtml.AppendHtml(closeButton);

                // toast div
                var toastDiv = new TagBuilder("div");
                toastDiv.Attributes.Add("data-bs-autohide", "true");
                toastDiv.Attributes.Add("data-bs-delay", "3000");
                toastDiv.Attributes.Add("class", $"toast align-items-center alert-{status} border-0");
                toastDiv.Attributes.Add("role", "alert");
                toastDiv.Attributes.Add("aria-live", "assertive");
                toastDiv.Attributes.Add("aria-atomic", "true");
                toastDiv.Attributes.Add("id", "toast");
                toastDiv.InnerHtml.AppendHtml(innerDiv);

                // output content
                output.TagName = "div";
                output.Attributes.Add("class", "position-fixed bottom-0 end-0 p-3");
                output.Attributes.Add("style", "z-index: 1000");

                // append toast div to output content
                output.Content.AppendHtml(toastDiv);
            }
            else
            {
                output.SuppressOutput();
            }
        }
    }
}
