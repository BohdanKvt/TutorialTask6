using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tutorial6.Services;

namespace Tutorial6.Middleware
{
    public class CustomLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public CustomLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context, IDbService service)
        {
            if (context.Request != null)
            {
                string path = context.Request.Path;  
                string method = context.Request.Method;  
                string queryString = context.Request.QueryString.ToString();
                string bodyStr = "";

                using (StreamReader reader = new StreamReader(context.Request.Body, Encoding.UTF8, true, 1024, true))
                {
                    bodyStr = await reader.ReadToEndAsync();
                }

                string log = "1. " + method + "\r\n2. " + path + "\r\n3. " + bodyStr + "\r\n4. " + queryString + " ";
                service.SaveLogData(log);
            }

            if (_next != null) await _next(context);  
        }

    }
}