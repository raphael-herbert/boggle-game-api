var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR(o=> 
{
    o.EnableDetailedErrors = true;
});
builder.Services.AddHttpClient();
builder.Services.AddSingleton<IBoggleService, BoggleService>();
builder.Services.AddSingleton<IDictionaryService, DictionaryService>();
builder.Services.AddSingleton<IBoardService, BoardService>();
builder.Services.AddSingleton<IBoggleTimerService, BoggleTimerService>();


builder.Services.AddControllers();
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(builder =>
    {
        builder.WithOrigins("http://192.168.1.23:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();

        builder.WithOrigins("http://localhost:4200")
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
    });
});

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

app.UseCors();
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection(); // Ajouter un certificat SSL OBLIGATOIRE POUR LE MODE PRODUCTION

app.UseAuthorization();
app.MapHub<BoggleHub>("/boggleHub");

app.MapControllers();

app.Run();
