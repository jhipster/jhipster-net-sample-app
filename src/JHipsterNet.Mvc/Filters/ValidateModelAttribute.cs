
namespace JHipsterNet.Mvc.Filters {
    using JHipsterNet.Mvc.Problems;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;

    public class ValidateModelAttribute : ActionFilterAttribute {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
                context.Result = new BadRequestObjectResult(new ValidationFailedProblem(context.ModelState));
        }
    }
}
