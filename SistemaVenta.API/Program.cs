using SistemaVenta.IOC;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
//Referencia de la capa
builder.Services.InyectarDependencias(builder.Configuration);

//Configuramos los cors
builder.Services.AddCors(opt =>
{
    opt.AddPolicy("Nueva Politica", app =>
    {
        app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
    });
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}
app.UseCors("Nueva Politica");//Activamos los cors
app.UseAuthorization();

app.MapControllers();

app.Run();
