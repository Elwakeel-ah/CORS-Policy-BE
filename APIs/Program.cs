namespace APIs;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddAuthorization();

        // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
        builder.Services.AddOpenApi();

        builder.Services.AddCors(opt =>
        {
            opt.AddPolicy("AllowPostMethod", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5500")
                      .WithMethods("POST", "OPTIONS").AllowAnyHeader();
            });

            opt.AddPolicy("AllowCredentials", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5500")
                     .WithMethods("POST", "OPTIONS")
                     .AllowCredentials()
                     .AllowAnyHeader();
            });

            opt.AddPolicy("PreflightCache", policy =>
            {
                policy.WithOrigins("http://127.0.0.1:5500")
                    .WithMethods("POST", "OPTIONS")
                    .SetPreflightMaxAge(TimeSpan.FromMinutes(10))
                    .AllowAnyHeader();
            });
        });

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        // app.UseHttpsRedirection();
        app.UseCors();

        app.UseAuthorization();


        app.MapGet("/simple-get", () =>
        {
            Console.WriteLine("GET:simple-get api called");
            return "Hello World!";
        });

        app.MapPost("/preflighted-post-no-cors", (UserDto user) =>
        {
            Console.WriteLine("POST:preflighted-post-no-cors api called");
            return Results.Created($"/preflighted-post-with-cors/{user.id}", user);
        });

        app.MapPost("/preflighted-post-with-cors", (UserDto user) =>
        {
            Console.WriteLine("POST:preflighted-post-with-cors api called");
            return Results.Created($"/preflighted-post-with-cors/{user.id}", user);
        }).RequireCors("AllowPostMethod");

        app.MapPost("/credentialed-post-with-no-cors", (UserDto user) =>
        {
            Console.WriteLine("POST:credentialed-post-with-no-cors api called");
            return Results.Created($"/credentialed-post-with-no-cors/{user.id}", user);
        });

        app.MapPost("/credentialed-post-with-cors", (UserDto user) =>
        {
            Console.WriteLine("POST:credentialed-post-with-cors api called");
            return Results.Created($"/credentialed-post-with-cors/{user.id}", user);
        }).RequireCors("AllowCredentials");

        app.MapPost("/submit-form", (HttpContext context) =>
{
    Console.WriteLine("Form submitted! No CORS needed!");
    var id = context.Request.Form["id"];
    var name = context.Request.Form["name"];
    return Results.Created($"/credentialed-post-with-cors/{id}", new UserDto { id = int.TryParse(id, out int parsedId) ? parsedId : 0, name = name });
});

        app.MapPost("/preflighted-cached", (UserDto user) =>
        {
            Console.WriteLine("POST:preflighted-cached:");
            return Results.Created($"/preflighted-cached/{user.id}", user);
        }).RequireCors("PreflightCache");

        app.Run();
    }
}

internal class UserDto
{
    public int id { get; set; }
    public string name { get; set; }
}
