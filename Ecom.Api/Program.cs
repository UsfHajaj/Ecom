
using Ecom.Api.Middleware;
using Ecom.infrastructure;

namespace Ecom.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);


            // Add services to the container.
            builder.Services.infrastructureConfiguration(builder.Configuration);

            builder.Services.AddCors(options =>
            {
                options.AddPolicy("CORSPolicy",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:4200")
                               .AllowAnyMethod()
                               .AllowAnyHeader()
                               .AllowCredentials();
                    });
            });
            builder.Services.AddMemoryCache();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
            builder.WebHost.UseUrls("http://0.0.0.0:5000");
            builder.Configuration.AddEnvironmentVariables();
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            builder.WebHost.UseUrls("http://0.0.0.0:5000"); 
            app.UseRouting();
            app.UseCors("CORSPolicy");
            app.UseMiddleware<ExceptionsMiddleware>();

            app.UseAuthentication(); // ãåã ÌÏÇð
            app.UseAuthorization();

            app.UseStatusCodePagesWithReExecute("/errors/{0}");   
            app.UseHttpsRedirection();
            
            

            app.UseStaticFiles();
            app.MapControllers();

            app.Run();
        }
    }
}
