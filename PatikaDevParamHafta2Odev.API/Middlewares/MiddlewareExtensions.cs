namespace PatikaDevParamHafta2Odev.API.Middlewares
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
    }
}
