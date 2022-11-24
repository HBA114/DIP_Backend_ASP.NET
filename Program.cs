using DIP_Backend.ImageOperations.Filter;
using DIP_Backend.ImageOperations.PreProcessing1;
using DIP_Backend.ImageOperations.PreProcessing2;
using DIP_Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// http://localhost:5071/swagger/index.html

// Add services to the container.

builder.Services.AddSingleton<InMemoryImageRepository>();
builder.Services.AddSingleton<ColorOperations>();
builder.Services.AddSingleton<HistogramOperations>();
builder.Services.AddSingleton<FilterOperations>();

builder.Services.AddControllers();
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

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
