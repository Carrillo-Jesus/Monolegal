using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace Monolegal.Shared
{
    /// <summary>
    /// Clase genérica para estandarizar las respuestas de la API
    /// </summary>
    public class Response<T>
    {
        public bool Success { get; set; }
        public string Message { get; set; }
        public HttpStatusCode StatusCode { get; set; }
        public T? Data { get; set; }
        public IEnumerable<string> Errors { get; private set; }

        private Response()
        {
            Success = true;
            Message = string.Empty;
            Errors = new List<string>();
        }

        // Respuesta exitosa
        public static Response<T> Ok(T data, string message = "Operación exitosa")
        {
            return new Response<T>
            {
                Success = true,
                Message = message,
                Data = data,
                StatusCode = HttpStatusCode.OK
            };
        }

        // respuesta de error
        public static Response<T> Fail(string message, HttpStatusCode statusCode = HttpStatusCode.BadRequest)
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                StatusCode = statusCode
            };
        }

        // no se encuentra un recurso
        public static Response<T> NotFound(string message = "Recurso no encontrado")
        {
            return new Response<T>
            {
                Success = false,
                Message = message,
                StatusCode = HttpStatusCode.NotFound
            };
        }
    }
}
