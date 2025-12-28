using SimpleStorageSystem.Shared.Models;

namespace SimpleStorageSystem.Shared.Services.Helper;

public static class ApiResponseHelper
{
    public static ApiResponse ErrorsToMessage(this ApiResponse res)
    {
        if (res is null)
            throw new ArgumentNullException(nameof(res), "API validation error response cannot be null");

        string message = "";
        if (!String.IsNullOrWhiteSpace(res.Message))
            message = res.Message + "\n\n";

        if (res.Errors is not null)
            foreach (var key in res.Errors)
            {
                string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
                message += $"{combined}\n";
            }

        res.Message = message;

        return res;
    }

    // public static ApiResponse ErrorsToErrorMessage(this ApiResponse res)
    // {
    //     if (res is null)
    //         throw new ArgumentNullException(nameof(res), "API validation error response cannot be null");

    //     string message = "";
    //     if (!String.IsNullOrWhiteSpace(res.Message))
    //         message = res.Message + "\n\n";

    //     if (res.Errors is not null)
    //         foreach (var key in res.Errors)
    //         {
    //             string combined = string.Join(", ", key.Value ?? Array.Empty<string>());
    //             message += $"{combined}\n";
    //         }

    //     res.ErrorMessage = message;

    //     return res;
    // }

}
