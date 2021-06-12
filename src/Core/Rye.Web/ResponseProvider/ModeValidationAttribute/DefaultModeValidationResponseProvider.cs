using Microsoft.AspNetCore.Mvc;
using Rye.Web.Internal;
using System.Linq;
using System.Net;

namespace Rye.Web.ResponseProvider.ModeValidationAttr
{
    public class DefaultModeValidationResponseProvider : IModeValidationResponseProvider
    {
        public IActionResult CreateResponse(ModeValidationContext context)
        {
            return new JsonResult(Result
                .Create(HttpStatusCode.BadRequest, I18n.GetText(LangKeyEnum.ParameterInValid, context.FirstField, context.Errors.FirstOrDefault()?.ErrorMessage)));
        }
    }
}
