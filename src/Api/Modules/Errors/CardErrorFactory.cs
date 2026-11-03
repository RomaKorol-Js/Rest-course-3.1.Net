using Application.Cards.Exceptions;
using Microsoft.AspNetCore.Mvc;

namespace Api.Modules.Errors;

public static class CardErrorFactory
{
    public static ObjectResult ToObjectResult(this CardException error)
    {
        return new ObjectResult(error.Message)
        {
            StatusCode = error switch
            {
                CardAlreadyExistsException => StatusCodes.Status409Conflict,
                CardNotFoundException => StatusCodes.Status404NotFound,
                CardUnhandledException => StatusCodes.Status500InternalServerError,
                _ => throw new NotImplementedException("Card error handler is not implemented")
            }
        };
    }d
}