
namespace JHipsterNet.Mvc.Extensions {
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class ActionResultWithHeaders : ActionResult {
        private readonly IHeaderDictionary _headers;
        private readonly ActionResult _result;

        public ActionResultWithHeaders(ActionResult receiver, IHeaderDictionary headers)
        {
            _result = receiver;
            _headers = headers;
        }

        private void AddHeaders(HttpResponse response)
        {
            foreach (var key in _headers.Keys) {
                response.Headers.Add(key, _headers[key]);
            }
        }

        public override Task ExecuteResultAsync(ActionContext context)
        {
            AddHeaders(context.HttpContext.Response);
            return _result.ExecuteResultAsync(context);
        }

        public override void ExecuteResult(ActionContext context)
        {
            AddHeaders(context.HttpContext.Response);
            _result.ExecuteResult(context);
        }
    }
}
