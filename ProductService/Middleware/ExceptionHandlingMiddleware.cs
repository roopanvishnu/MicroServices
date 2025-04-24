﻿//using System;
//using System.Net;
//using System.Text.Json;
//using System.Threading.Tasks;
//using Microsoft.AspNetCore.Http;
//using Microsoft.Extensions.Logging;

//namespace ProductService.Middleware
//{
//    public class ExceptionHandlingMiddleware
//    {
//        private readonly RequestDelegate _next;
//        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

//        public ExceptionHandlingMiddleware(RequestDelegate next, ILogger<ExceptionHandlingMiddleware> logger)
//        {
//            _next = next;
//            _logger = logger;
//        }

//        public async Task InvokeAsync(HttpContext context)
//        {
//            try
//            {
//                await _next(context);
//            }
//            catch (Exception ex)
//            {
//                _logger.LogError(ex, "Unhandled exception occurred");

//                context.Response.ContentType = "application/json";
//                context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

//                var response = new
//                {
//                    StatusCode = context.Response.StatusCode,
//                    Message = "An unexpected error occurred. Please try again later."
//                };

//                var json = JsonSerializer.Serialize(response);
//                await context.Response.WriteAsync(json);
//            }
//        }
//    }
//}