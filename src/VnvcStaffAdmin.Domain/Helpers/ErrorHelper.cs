using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace VnvcStaffAdmin.Domain.Helpers
{
    public static class ErrorHelper
    {
        public static Dictionary<string, List<string>> GetErrorsFromModelState(ModelStateDictionary modelState)
        {
            return modelState
                .Where(ms => ms.Value.Errors.Any())
                .ToDictionary(
                    ms => ms.Key,
                    ms => ms.Value.Errors.Select(e => e.ErrorMessage).ToList()
                );
        }
    }
}