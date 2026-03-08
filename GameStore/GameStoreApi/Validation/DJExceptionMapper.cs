namespace GameStoreApi.Validation {
    public static class ExceptionMapperExtensions
    {
        public static IServiceCollection AddExceptionMappers(this IServiceCollection services)
        {
            var mapperType = typeof(IExceptionMapper);

            var mappers = AppDomain.CurrentDomain.GetAssemblies()
                .SelectMany(s => s.GetTypes())
                .Where(p => mapperType.IsAssignableFrom(p) && !p.IsInterface && !p.IsAbstract);

            foreach (var mapper in mappers)
            {
                services.AddSingleton(mapperType, mapper);
            }

            return services;
        }
    }
}