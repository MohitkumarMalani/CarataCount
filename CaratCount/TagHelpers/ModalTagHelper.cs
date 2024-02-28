using CaratCount.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Razor.TagHelpers;

namespace CaratCount.TagHelpers
{
    // You may need to install the Microsoft.AspNetCore.Razor.Runtime package into your project
    [HtmlTargetElement("modal")]
    public class ModalTagHelper : TagHelper
    {

        private readonly IHtmlHelper _htmlHelper;

        public ModalTagHelper(IHtmlHelper htmlHelper)
        {
            _htmlHelper = htmlHelper;
        }

        // Prop that gives us access to the ViewContext
        [ViewContext()]
        [HtmlAttributeNotBound()]
        public ViewContext? ViewContext { get; set; }
        public override void Process(TagHelperContext context, TagHelperOutput output)
        {
            if (ViewContext == null)
                return;


            string? modalAction = ViewContext?.TempData["ModalAction"]?.ToString() ?? "Index";
            string? modalController = ViewContext?.TempData["ModalController"]?.ToString() ?? "Home";
            string? modalMessage = ViewContext?.TempData["ModalMessage"]?.ToString() ?? "Please confirm your action.";
            string? modalTitle = ViewContext?.TempData["ModalTitle"]?.ToString() ?? "Confirm action";
            string? modalId = ViewContext?.TempData["ModalId"]?.ToString();

            ModalViewModel? modalViewModel = new()
            {
                Id = modalId,
                Title = modalTitle,
                Message = modalMessage,
                Action = modalAction,
                Controller = modalController
            };

            ((IViewContextAware)_htmlHelper).Contextualize(ViewContext);


            // render the partial view with the provided model
            var modalContent = _htmlHelper.Partial("_ModalPartial", modalViewModel);

            // build the HTML structure for the modal
            output.TagName = "div";
            output.Attributes.Add("class", "modal fade");
            output.Attributes.Add("id", "modal");
            output.Attributes.Add("tabindex", "-1");
            output.Attributes.Add("aria-labelledby", "modalLabel");
            output.Attributes.Add("aria-hidden", "true");

            // add the modal content
            output.Content.SetHtmlContent(modalContent);
        }
    }
}
