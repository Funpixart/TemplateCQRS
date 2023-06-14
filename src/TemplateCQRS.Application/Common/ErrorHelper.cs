using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Http;

namespace TemplateCQRS.Application.Common;

public class ErrorHelper
{
    public static List<ValidationFailure> LogExceptionAndReturnError<T>(Exception ex)
    {
        Log.Logger.ForContext("Payload", typeof(T).Name).Error(ex, $"{typeof(T).Name} was caught!");

        var message = ex.InnerException is not null
            ? ex.InnerException.Message
            : ex.Message;

        var errors = new List<ValidationFailure>
        {
            new ()
            {
                ErrorCode = StatusCodes.Status500InternalServerError.ToString(),
                ErrorMessage = message
            }
        };

        return errors;
    }
}