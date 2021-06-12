
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.DependencyInjection;

using Rye;
using Rye.Web;

using System.Linq;

namespace Demo.WebApi
{
    public class ValidationAttribute : ModelValidationAttribute
    {
        protected override string GetErrorMessage(HttpContext httpContext, ModelErrorCollection errors)
        {
            var dicKey = errors.First().ErrorMessage;
            return I18n.GetText(dicKey, dicKey);
        }
    }
}
