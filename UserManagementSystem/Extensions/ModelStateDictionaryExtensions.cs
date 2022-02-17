using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using UserManagementSystem.Utilities;

namespace UserManagementSystem.Extensions
{
    public static class ModelStateDictionaryExtensions
    {
        public static bool TryAddError(this ModelStateDictionary modelState, Dictionary<string, bool> errorsToCheck)
        {
            var errorMessage = ErrorChecker.GetFirstErrorMessageOrDefault(errorsToCheck);

            if (!String.IsNullOrEmpty(errorMessage))
            {
                modelState.AddModelError(String.Empty, errorMessage);

                return true;
            }
            
            return false;
        }
    }
}
