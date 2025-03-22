namespace APILayer
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();  // Add Swagger generation

            var app = builder.Build();

            // Enable middleware to serve generated Swagger as a JSON endpoint.
            app.UseSwagger();

            // Enable middleware to serve Swagger UI (HTML, JS, CSS, etc.), specifying the Swagger JSON endpoint.
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // You can add additional setup for OpenAPI in Development environment
            }

            app.UseHttpsRedirection();  // Ensure HTTPS redirection is set

            app.UseAuthorization();

            // Map controllers to their routes
            app.MapControllers();

            // Run the application
            app.Run();
        }
    }
}
