using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json.Serialization;

namespace Vnr.Storage.API.Infrastructure.BaseResponse
{
    public class ResponseModel
    {
        public ResponseModel()
        {
            Errors = new Dictionary<string, string[]>();
        }

        [JsonIgnore]
        public HttpStatusCode StatusCode { get; set; }

        public bool Successed { get; set; }

        public IDictionary<string, string[]> Errors { get; set; }
    }

    public class ResponseModel<T> : ResponseModel
    {
        public T Data { get; set; }
    }

    public static class ResponseProvider
    {
        #region Ok

        public static ResponseModel Ok(HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModel
            {
                Successed = true,
                StatusCode = statusCode,
            };
        }

        public static ResponseModel<T> Ok<T>(T value, HttpStatusCode statusCode = HttpStatusCode.OK)
        {
            return new ResponseModel<T>
            {
                Successed = true,
                StatusCode = statusCode,
                Data = value
            };
        }

        #endregion Ok

        #region Bad Request

        public static ResponseModel BadRequest(string error, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModel
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { ["Error"] = new string[] { error } }
            };
        }

        public static ResponseModel BadRequest(string[] errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModel
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { ["Errors"] = errors }
            };
        }

        public static ResponseModel<T> BadRequest<T>(string[] errors, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModel<T>
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { ["Errors"] = errors }
            };
        }

        public static ResponseModel BadRequest(ModelStateDictionary model, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new ResponseModel
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = model
                .Where(x => x.Value.Errors.Any())
                .ToDictionary(
                    x => x.Key,
                    x => x.Value.Errors.Select(e => e.ErrorMessage).ToArray()
                )
            };
        }

        #endregion Bad Request

        #region Not Found

        public static ResponseModel NotFound(string entityName, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            return new ResponseModel
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { [entityName] = new string[] { "The requested resource could not be found" } }
            };
        }

        public static ResponseModel<T> NotFound<T>(string entityName, HttpStatusCode statusCode = HttpStatusCode.NotFound)
        {
            return new ResponseModel<T>
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { [entityName] = new string[] { "The requested resource could not be found" } }
            };
        }

        #endregion Not Found

        #region UnAuthorized

        public static ResponseModel UnAuthorized(string errorMessage, HttpStatusCode statusCode = HttpStatusCode.Unauthorized)
        {
            return new ResponseModel
            {
                Successed = false,
                StatusCode = statusCode,
                Errors = new Dictionary<string, string[]> { ["UnAuthorized"] = new string[] { errorMessage } }
            };
        }

        #endregion UnAuthorized
    }
}