using AgendaManicure.Models;
using AgendaManicure.Services;

var builder = WebApplication.CreateBuilder(args);


// CONFIGURAR MONGODB DESDE appsettings.json

builder.Services.Configure<MongoDBSettings>(
    builder.Configuration.GetSection("MongoDB"));


// REGISTRAR SERVICIOS DE ACCESO A DATOS

builder.Services.AddSingleton<UserService>();
builder.Services.AddSingleton<ServicioService>();
builder.Services.AddSingleton<CategoriaService>();
builder.Services.AddSingleton<AgendaService>();
builder.Services.AddSingleton<PagoService>();
builder.Services.AddSingleton<ResenasService>();


// CONTROLADORES

builder.Services.AddControllers();


// CORS (NECESARIO PARA CONECTAR EL FRONTEND)

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll",
        policy => policy
            .AllowAnyOrigin()
            .AllowAnyMethod()
            .AllowAnyHeader()
    );
});


// SWAGGER

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();


// USAR SWAGGER SOLO EN DESARROLLO

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


// ACTIVAR CORS

app.UseCors("AllowAll");


// MAPEAR CONTROLADORES

app.MapControllers();

app.Run();


