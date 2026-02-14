using Microsoft.AspNetCore.Mvc;

namespace APILayer
{
    public class ApiResponse<T>
    {
        public int statusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }


}
