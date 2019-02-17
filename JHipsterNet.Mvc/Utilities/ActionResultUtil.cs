
namespace JHipsterNet.Mvc.Utilities {
    using Microsoft.AspNetCore.Mvc;

    public static class ActionResultUtil {
        public static ActionResult WrapOrNotFound(object value)
        {
            return value != null ? (ActionResult) new OkObjectResult(value) : new NotFoundResult();
        }
    }
}
