using Microsoft.EntityFrameworkCore;
using RoyalHillHotel.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
//.AddJsonOptions(options =>
//{
//    options.JsonSerializerOptions.PropertyNameCaseInsensitive = true;
//});
builder.Services.AddControllers()
.AddJsonOptions(options =>
{
    // Optional: disables camelCase naming (keeps PascalCase)
    options.JsonSerializerOptions.PropertyNamingPolicy = null;

    // Optional: ignore null values in response
    options.JsonSerializerOptions.DefaultIgnoreCondition = System.Text.Json.Serialization.JsonIgnoreCondition.WhenWritingNull;
});

builder.Services.AddDbContext<ApplicationDbContext>
    (options => options.UseSqlServer(builder.Configuration.GetConnectionString("Dbcs")));


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseDeveloperExceptionPage(); 

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
