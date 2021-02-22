namespace Rye.Web.Options
{
    public class RyeWebOptions
    {
        public GlobalExceptionFilterOptions GlobalExceptionFilter { get; set; } = new GlobalExceptionFilterOptions();
        public GlobalActionFilterOptions GlobalActionFilter { get; set; } = new GlobalActionFilterOptions();
        public AvoidRepeatableRequestOptions AvoidRepeatableRequest { get; set; } = new AvoidRepeatableRequestOptions();
        public ModeValidationOptions ModeValidation { get; set; } = new ModeValidationOptions();
        public AuthorizationOptions Authorization { get; set; } = new AuthorizationOptions();
        public SecurityOptions Security { get; set; } = new SecurityOptions();
    }
}
