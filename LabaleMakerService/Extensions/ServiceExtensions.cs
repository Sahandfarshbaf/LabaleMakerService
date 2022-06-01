
using System.Diagnostics;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Text.Json.Serialization;
using Contracts;
using Entities.Models;
using LabaleMakerService.Exceptions;
using LabaleMakerService.Services;
using LabaleMakerService.Services.Interfaces;
using Logger;
using Logger.Models;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;
using Repositories;

namespace LabaleMakerService.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    context.Response.StatusCode = (int)HttpStatusCode.OK;
                    context.Response.ContentType = "application/json";
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    if (contextFeature != null)
                    {
                        #region Read Body

                        var b = context.Features.Get<IHttpRequestFeature>();
                        b.Body.Seek(0, SeekOrigin.Begin);
                        var c = await new StreamReader(b.Body).ReadToEndAsync();

                        #endregion

                        using var p = app.ApplicationServices.CreateScope();

                        #region Get Method Info

                        var cap = p.ServiceProvider.GetRequiredService<IActionDescriptorCollectionProvider>();
                        var controller = cap.ActionDescriptors.Items.FirstOrDefault();
                        var methodInfo = ((Microsoft.AspNetCore.Mvc.Controllers.ControllerActionDescriptor)controller)
                            .MethodInfo;

                        #endregion

                        var logger = p.ServiceProvider.GetService<ILogHandler>();

                        #region Get Line Number

                        var stackTrace = (StackTrace)contextFeature.Error.Data[2];
                        int? lineNumber = null;
                        if (stackTrace != null)
                        {
                            lineNumber = stackTrace.GetFrame(0)?.GetFileLineNumber();
                        }

                        #endregion

                        if (contextFeature.Error is BusinessException)
                        {
                            var excep = ((BusinessException)contextFeature.Error);
                            logger.LogError(new HandledErrorLog()
                            {
                                CreateDateTime = DateTime.Now,
                                ErrorMessage = contextFeature.Error.Message,
                                ErrorCode = excep.Code,
                                ServiceName = methodInfo.DeclaringType?.FullName,
                                ServiceMethodName = methodInfo.Name,
                                ServiceParameters = string.Concat("{",
                                    $"QueryString:{(context.Request.QueryString.HasValue ? context.Request.QueryString.Value?.Replace("?", "") : "empty")} , Body:{(string.IsNullOrEmpty(c) ? "empty" : c)}", "}"),


                            });
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = excep.Message
                            }.ToString());
                        }
                        else
                        {

                            logger.LogError(new SystemErrorLog
                            {
                                CreateDateTime = DateTime.Now,
                                ExceptionStr = contextFeature.Error.ToString(),
                                ServiceName = methodInfo.DeclaringType?.FullName,
                                ServiceMethodName = methodInfo.Name,
                                ServiceParameters = string.Concat("{",
                                    $"QueryString:{(context.Request.QueryString.HasValue ? context.Request.QueryString.Value?.Replace("?", "") : "empty")} , Body:{(string.IsNullOrEmpty(c) ? "empty" : c)}", "}"),
                                InnerLineNumber = lineNumber

                            });

                            ;
                            await context.Response.WriteAsync(new ErrorDetails()
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = "خطا در سامانه."
                            }.ToString());

                        }


                    }
                });
            });
        }
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy",
                    builder => builder.AllowAnyOrigin()
                        .AllowAnyMethod()
                        .AllowAnyHeader());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options =>
            {

            });
        }
        public static void ConfigureServices(this IServiceCollection services)
        {
       
            services.AddScoped<ICurrentUserService, CurrentUserService>();
            services.AddScoped<IBaseInfoService, BaseInfoService>();
            services.AddScoped<IAccountService, AccountService>();
            services.AddScoped<ICommodityService, CommodityService>();
          


        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILogHandler, LogHandler>();
        }
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration config)
        {
            var connectionString = config["ConnectionStrings:DefaultConnection"];
            services.AddDbContext<LabelMaker_BP_DBContext>(o => o.UseSqlServer(connectionString));

            var logconnectionString = config["ConnectionStrings:LogConnection"];
            services.AddDbContext<LableMaker_Log_DBContext>(o => o.UseSqlServer(logconnectionString));

        }
        public static void ConfigureRepositoryWrapper(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryWrapper, RepositoryWrapper>();
        }
    }
}
