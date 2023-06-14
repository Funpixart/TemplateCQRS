using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation.Results;
using Microsoft.AspNetCore.Identity;

namespace TemplateCQRS.Application.Extensions;

public static class ValidationExtensions
{
    public static void AddIdentityErrorsToValidationFailures(this List<ValidationFailure> validationFailures, IEnumerable<IdentityError> identityErrors)
    {
        validationFailures.AddRange(identityErrors.Select(error => new ValidationFailure
        {
            ErrorCode = error.Code,
            ErrorMessage = error.Description
        }));
    }

    public static void AddIdentityErrors(this ICollection<ValidationFailure> failures, IEnumerable<IdentityError> identityErrors)
    {
        foreach (var error in identityErrors)
        {
            failures.Add(new ValidationFailure
            {
                ErrorCode = error.Code,
                ErrorMessage = error.Description
            });
        }
    }
}
