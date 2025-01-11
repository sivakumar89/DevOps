using BlogApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container
builder.Services.AddScoped<IBlogService, BlogService>();
builder.Services.AddControllers();

// Build the app
var app = builder.Build();

// Configure the HTTP request pipeline
app.MapControllers();
app.Run();
