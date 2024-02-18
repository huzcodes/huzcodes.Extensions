using FluentValidation;
using huzcodes.Extensions.API.Models;
using huzcodes.Extensions.API.Models.ValidationUsingFluent;
using huzcodes.Extensions.Exceptions;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace huzcodes.Extensions.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ExtensionsController : ControllerBase
    {

        [HttpGet(Name = "resultExceptions")]
        public ActionResult Get(int customException)
        {
            if (customException == 1)
                throw new CustomResultException(new CustomExceptionResponse()
                {
                    Message = "response error from huzcodes.extensions plugin using custom result exception class",
                    StatusCode = (int)HttpStatusCode.BadRequest,
                    ClassName = nameof(ExtensionsController),
                    FunctionName = nameof(Get),
                });

            else throw new ResultException("response error from huzcodes.extensions plugin using result exception class",
                                           (int)HttpStatusCode.BadRequest);
        }

        [HttpPost(Name = "fluentException")]
        public ActionResult Post([FromBody] FluentValidationRequest validationRequest)
        {
            var validators = new FluentValidationTestingModelValidator();
            var results = validators.Validate(validationRequest);
            if (!results.IsValid)
                throw new ValidationException(results.Errors);


            return Ok(new FluentValidationRequest()
            {
                FirstName = validationRequest.FirstName,
                LastName = validationRequest.LastName
            });
        }
    }
}
