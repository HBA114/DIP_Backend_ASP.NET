using DIP_Backend.ImageOperations.BaseOperations;
using DIP_Backend.ImageOperations.Filter;
using DIP_Backend.ImageOperations.Morphological;
using DIP_Backend.ImageOperations.PreProcessing1;
using DIP_Backend.ImageOperations.PreProcessing2;
using DIP_Backend.Repositories;

var builder = WebApplication.CreateBuilder(args);

// http://localhost:5071/swagger/index.html

builder.Services.AddSingleton<InMemoryImageRepository>();
builder.Services.AddSingleton<BasicOperations>();
builder.Services.AddSingleton<ColorOperations>();
builder.Services.AddSingleton<HistogramOperations>();
builder.Services.AddSingleton<FilterOperations>();
builder.Services.AddSingleton<MorphologicalOperations>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.MapControllers();

app.Run();
