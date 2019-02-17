
namespace JHipsterNet.Mvc.Problems {
    using Hellang.Middleware.ProblemDetails;
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;

    public class InvalidPasswordException : ProblemDetailsException {
        public InvalidPasswordException() : base(new ProblemDetails {
            Type = ErrorConstants.InvalidPasswordType,
            Detail = "Incorrect Password",
            Status = StatusCodes.Status400BadRequest
        })
        {
        }
    }
}
