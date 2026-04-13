var builder = WebApplication.CreateBuilder(args);

// ✅ Add controller support
builder.Services.AddControllers();

// Optional (Swagger if needed)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

// ✅ This is the MOST IMPORTANT line
app.MapControllers();

app.Run();