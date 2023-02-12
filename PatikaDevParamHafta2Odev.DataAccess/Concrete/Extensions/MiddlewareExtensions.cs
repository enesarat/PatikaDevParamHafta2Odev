using Microsoft.AspNetCore.Builder;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions;
using PatikaDevParamHafta2Odev.DataAccess.Concrete.Middlewares;

namespace PatikaDevParamHafta2Odev.DataAccess.Concrete.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseCustomLogger(this IApplicationBuilder app)
        {
            try
            {
                return app.UseMiddleware<CustomLoggerMiddleware>();
            }
            catch (Exception exp)
            {

                throw exp;
            }
        }

        public static IApplicationBuilder UseCustomExceptionHandler(this IApplicationBuilder app)
        {
            return app.UseMiddleware<CustomExceptionMiddleware>();
        }
    }
}
