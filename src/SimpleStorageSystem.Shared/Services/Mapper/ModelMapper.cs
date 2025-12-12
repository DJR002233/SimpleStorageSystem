using System.Reflection;

namespace SimpleStorageSystem.Shared.Services.Mapper;

public static class ModelMapper
{
    public static T Map<T>(object model)
    {
        if (model is null) throw new Exception("model to map is null");

        Type typeIn = model.GetType();
        Type typeOut = typeof(T);
        PropertyInfo[] inProperties = typeIn.GetProperties();
        PropertyInfo[] resProperties = typeOut.GetProperties();

        object res = Activator.CreateInstance(typeOut) ?? throw new Exception("Failed to create instance of object");

        foreach (PropertyInfo inProperty in inProperties)
        {
            var row = resProperties.FirstOrDefault(r => r.Name == inProperty.Name);
            if (row is null)
                continue;
                
            int resIndex = Array.IndexOf(resProperties, row);
            PropertyInfo resProperty = resProperties[resIndex];

            var value = inProperty.GetValue(model);

            if (value is not null && resProperty.PropertyType.IsAssignableFrom(value.GetType()))
                resProperty.SetValue(res, value);
        }

        return (T)res;
    }

    // public static ApiResponse MapFromApiResponse<T>(T response)
    // {
    //     return MapToApiResponse<ApiResponse>(response!);
    // }

    // public static ApiResponse<OUT> MapToApiResponse<T, OUT>(ApiResponse<T> res)
    // {
    //     return new ApiResponse<OUT>
    //     {
    //         Type = res.Type,
    //         Title = res.Title,
    //         Status = res.Status,
    //         StatusCode = res.StatusCode,
    //         StatusMessage = res.StatusMessage,
    //         Message = res.Message,
    //         Errors = res.Errors,
    //         TraceId = res.TraceId,
    //         Data = res.Data
    //     };
    // }
    // public ApiResponse MapToApiResponse(ApiResponse res)
    // {
    //     return MapToApiResponse<object>(res);
    // }
}
