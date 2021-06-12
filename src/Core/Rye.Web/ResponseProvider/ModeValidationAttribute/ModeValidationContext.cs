using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Rye.Web.ResponseProvider.ModeValidationAttr
{
    public class ModeValidationContext
    {
        public ModeValidationContext(HttpContext httpContext, ModelStateDictionary modelState, string firstField, ModelErrorCollection errors)
        {
            HttpContext = httpContext;
            ModelState = modelState;
            FirstField = firstField;
            Errors = errors;
        }

        public ModelStateDictionary ModelState { get; }

        public HttpContext HttpContext { get; }

        public string FirstField { get; set; }

        public ModelErrorCollection Errors { get; }
    }
}
