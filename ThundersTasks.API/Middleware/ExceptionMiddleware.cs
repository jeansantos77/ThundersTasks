namespace ThundersTasks.API.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }
        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                // Chama o próximo middleware no pipeline
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                // Loga a exceção
                _logger.LogError(ex, "Um erro ocorreu.");

                // Retorna uma resposta personalizada para o cliente
                httpContext.Response.StatusCode = StatusCodes.Status500InternalServerError;
                httpContext.Response.ContentType = "application/json";
                var errorResponse = new { Message = "Um erro ocorreu. Por favor tente novamente." };
                await httpContext.Response.WriteAsJsonAsync(errorResponse);
            }
        }
    }
}
