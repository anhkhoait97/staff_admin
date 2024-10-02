using Microsoft.AspNetCore.Mvc.ModelBinding;
using VnvcStaffAdmin.Domain.Constants;
using VnvcStaffAdmin.Domain.Helpers;

namespace VnvcStaffAdmin.Domain.Model
{
    public class ResponseModel
    {
        public bool Success { get; set; }
        public string? Message { get; set; }
        public object? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public string? ErrorCode { get; set; }

        public ResponseModel()
        { }

        public ResponseModel(bool success, string? message = default, object? data = default)
        { 
            Success = success;
            Message = message;
            Data = data;
        }

        public static ResponseModel Successed(string? message = default, object? data = default) =>
            BuildResponse(true, data, message);

        public static ResponseModel Failed(string? message = default, object? data = default) =>
            BuildResponse(false, data, message, ErrorCodes.ServiceUnavailable);

        public static ResponseModel Failed(string? message = default, string? errorCode = default) =>
            BuildResponse(false, default, message, errorCode);

        public static ResponseModel Failed(string? message = default) =>
            BuildResponse(false, default, message, ErrorCodes.ServiceUnavailable);

        public static ResponseModel Exception(string? message = default) =>
            BuildResponse(false, default, message, ErrorCodes.InternalServerError);

        public static ResponseModel BadRequest(ModelStateDictionary? modelState = null)
        {
            return BuildResponse(false, null, "Bad Request", ErrorCodes.BadRequest,
                modelState != null ? ErrorHelper.GetErrorsFromModelState(modelState) : null);
        }

        private static ResponseModel BuildResponse(bool success, object? data, string? message = default, string? errorCode = default, Dictionary<string, List<string>>? errors = null)
        {
            return new ResponseModel
            {
                Success = success,
                Message = message,
                Data = data,
                ErrorCode = errorCode,
                Errors = errors
            };
        }
    }

    public class ResponseModel<T>
    {
        public bool? Success { get; set; }
        public string? Message { get; set; }
        public T? Data { get; set; }
        public Dictionary<string, List<string>>? Errors { get; set; }
        public string? ErrorCode { get; set; }

        public ResponseModel()
        { }

        public static ResponseModel<T> Successed(string? message = default) =>
            BuildResponse(true, default, message);

        public static ResponseModel<T> Successed(T? data = default, string? message = default) =>
            BuildResponse(true, data, message);

        public static ResponseModel<T> Failed(string? message = default, string? errorCode = default) =>
            BuildResponse(false, default, message, errorCode);

        public static ResponseModel<T> BadRequest(ModelStateDictionary? modelState = null) =>
            BuildResponse(false, default, "Bad Request", ErrorCodes.BadRequest,
                modelState != null ? ErrorHelper.GetErrorsFromModelState(modelState) : null);

        private static ResponseModel<T> BuildResponse(bool success, T? data, string? message = default, string? errorCode = default, Dictionary<string, List<string>>? errors = null)
        {
            return new ResponseModel<T>
            {
                Success = success,
                Message = message,
                Data = data,
                ErrorCode = errorCode,
                Errors = errors
            };
        }
    }
}