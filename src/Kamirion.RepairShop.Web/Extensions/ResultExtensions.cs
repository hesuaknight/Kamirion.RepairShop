using Kamirion.RepairShop.Shared.Results;
using Microsoft.AspNetCore.Mvc;

namespace Kamirion.RepairShop.Web.Extensions;

public static class ResultExtensions
{
    public static IActionResult ToActionResult(this Result result) =>
        result.IsSuccess ? new OkResult() : result.Error.ToActionResult();

    public static IActionResult ToActionResult<T>(this Result<T> result) =>
        result.IsSuccess ? new OkObjectResult(result.Value) : result.Error.ToActionResult();

    private static IActionResult ToActionResult(this Error error)
    {
        var problem = new ProblemDetails
        {
            Detail = error.Description,
            Extensions = { ["errorCode"] = error.Code }
        };

        if (error.Code.EndsWith(".NotFound"))
        {
            problem.Status = StatusCodes.Status404NotFound;
            problem.Title = "Resource not found.";
            return new NotFoundObjectResult(problem);
        }

        if (error.Code.StartsWith("Validation."))
        {
            problem.Status = StatusCodes.Status400BadRequest;
            problem.Title = "Validation error.";
            return new BadRequestObjectResult(problem);
        }

        if (error.Code == "Auth.Unauthorized")
        {
            problem.Status = StatusCodes.Status403Forbidden;
            problem.Title = "Forbidden.";
            return new ObjectResult(problem) { StatusCode = StatusCodes.Status403Forbidden };
        }

        if (error.Code.StartsWith("Conflict"))
        {
            problem.Status = StatusCodes.Status409Conflict;
            problem.Title = "Conflict.";
            return new ConflictObjectResult(problem);
        }

        problem.Status = StatusCodes.Status400BadRequest;
        problem.Title = "Bad request.";
        return new BadRequestObjectResult(problem);
    }
}
