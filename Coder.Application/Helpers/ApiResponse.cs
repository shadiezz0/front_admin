using Azure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Coder.Application.Helpers
{
    public class ApiResponse<T> where T : class
    {
        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingNull)]
        public T Data { get; set; }

        #region Success Methods

        public static ApiResponse<T> Success(T data, string message = "success")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Created(T data, string message = "created")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.Created,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> Accepted(T data, string message = "accepted")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.Accepted,
                Message = message,
                Data = data
            };
        }

        public static ApiResponse<T> NoContent(string message = "no content")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.NoContent,
                Message = message,
                Data = null!
            };
        }

        #endregion

        #region Client Error Methods

        public static ApiResponse<T> BadRequest(string message = "bad request")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> Unauthorized(string message = "unauthorized")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> Forbidden(string message = "forbidden")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.Forbidden,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> NotFound(string message = "not found")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> Conflict(string message = "conflict")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.Conflict,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> UnprocessableEntity(string message = "unprocessable entity")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.UnprocessableEntity,
                Message = message,
                Data = null!
            };
        }

        #endregion

        #region Server Error Methods

        public static ApiResponse<T> InternalServerError(string message = "internal server error")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> NotImplemented(string message = "not implemented")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.NotImplemented,
                Message = message,
                Data = null!
            };
        }

        public static ApiResponse<T> ServiceUnavailable(string message = "service unavailable")
        {
            return new ApiResponse<T>
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Message = message,
                Data = null!
            };
        }

        #endregion

        #region Custom Status Code

        public static ApiResponse<T> Custom(HttpStatusCode statusCode, T data, string message = "operation completed")
        {
            return new ApiResponse<T>
            {
                StatusCode = statusCode,
                Message = message,
                Data = data
            };
        }

        #endregion
    }

    /// <summary>
    /// Non-generic ApiResponse for operations that don't return data
    /// </summary>
    public class Response
    {
        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        #region Success Methods

        public static Response Success(string message = "success")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.OK,
                Message = message
            };
        }

        public static Response Created(string message = "created")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.Created,
                Message = message
            };
        }

        public static Response NoContent(string message = "no content")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.NoContent,
                Message = message
            };
        }

        #endregion

        #region Client Error Methods

        public static Response BadRequest(string message = "bad request")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message
            };
        }

        public static Response Unauthorized(string message = "unauthorized")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.Unauthorized,
                Message = message
            };
        }

        public static Response Forbidden(string message = "forbidden")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.Forbidden,
                Message = message
            };
        }

        public static Response NotFound(string message = "not found")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message
            };
        }

        public static Response Conflict(string message = "conflict")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.Conflict,
                Message = message
            };
        }

        #endregion

        #region Server Error Methods

        public static Response InternalServerError(string message = "internal server error")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = message
            };
        }

        public static Response ServiceUnavailable(string message = "service unavailable")
        {
            return new Response
            {
                StatusCode = HttpStatusCode.ServiceUnavailable,
                Message = message
            };
        }

        #endregion

        #region Custom Status Code

        public static Response Custom(HttpStatusCode statusCode, string message = "operation completed")
        {
            return new Response
            {
                StatusCode = statusCode,
                Message = message
            };
        }

        #endregion
    }

    /// <summary>
    /// Paged Response for paginated data
    /// </summary>
    public class PagedResponse<T> where T : class
    {
        [JsonPropertyName("statusCode")]
        public HttpStatusCode StatusCode { get; set; }

        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("data")]
        public List<T> Data { get; set; } = new();

        [JsonPropertyName("totalCount")]
        public int TotalCount { get; set; }

        [JsonPropertyName("pageNumber")]
        public int PageNumber { get; set; }

        [JsonPropertyName("pageSize")]
        public int PageSize { get; set; }

        [JsonPropertyName("totalPages")]
        public int TotalPages => (TotalCount + PageSize - 1) / PageSize;

        [JsonPropertyName("hasPreviousPage")]
        public bool HasPreviousPage => PageNumber > 1;

        [JsonPropertyName("hasNextPage")]
        public bool HasNextPage => PageNumber < TotalPages;

        public static PagedResponse<T> Success(List<T> data, int totalCount, int pageNumber, int pageSize, string message = "success")
        {
            return new PagedResponse<T>
            {
                StatusCode = HttpStatusCode.OK,
                Message = message,
                Data = data,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };
        }

        public static PagedResponse<T> NotFound(string message = "not found")
        {
            return new PagedResponse<T>
            {
                StatusCode = HttpStatusCode.NotFound,
                Message = message,
                Data = new(),
                TotalCount = 0
            };
        }

        public static PagedResponse<T> BadRequest(string message = "bad request")
        {
            return new PagedResponse<T>
            {
                StatusCode = HttpStatusCode.BadRequest,
                Message = message,
                Data = new(),
                TotalCount = 0
            };
        }
    }
}
    
