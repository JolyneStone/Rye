using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Monica.Web.ResponseProvider.ModeValidationAttr
{
    public class ModeValidationContext
    {
        public ModeValidationContext(HttpContext httpContext, ModelStateDictionary modelState, ModelErrorCollection errors)
        {
            HttpContext = httpContext;
            ModelState = modelState;
            Errors = errors;
        }

        public ModelStateDictionary ModelState { get; }

        public HttpContext HttpContext { get; }

        public ModelErrorCollection Errors { get; }
    }
}
