using hotel.BLL.Contract;
using hotel.BLL;
using hotel.Context;
using hotel.DAL;
using hotel.DAL.Contract;
using hotel.Entities;
using hotel.Service.Interfaces;
using hotel.DAL.Repos;
using Microsoft.EntityFrameworkCore;
using hotel.Service;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;



var builder = WebApplication.CreateBuilder(args);

// Ajouter les services nécessaires
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
       .AddCookie(options =>
       {
           options.LoginPath = "/login"; // Chemin de la page de connexion
           options.LogoutPath = "/logout"; // Chemin de la déconnexion
       });

// Ajouter les contrôleurs
builder.Services.AddControllers();

// Add services to the container.
var Cnx = builder.Configuration.GetConnectionString("ConnectionString");
builder.Services.AddDbContext<DataContext>(options =>
                     options.UseSqlServer(Cnx, b => b.MigrationsAssembly("API")));

builder.Services.AddControllers();
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<DataContext>();

builder.Services.AddScoped<IPersonnelService, PersonnelService>();
builder.Services.AddScoped<IGenericBLL<Personnel>, GenericBLL<Personnel>>();
builder.Services.AddScoped<IRepository<Personnel>, PersonnelRepository>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IGenericBLL<Client>, GenericBLL<Client>>();
builder.Services.AddScoped<IRepository<Client>, ClientRepository>();
builder.Services.AddScoped<IChambreService, ChambreService>();
builder.Services.AddScoped<IGenericBLL<Chambre>, GenericBLL<Chambre>>();
builder.Services.AddScoped<IRepository<Chambre>, ChambreRepository>();
builder.Services.AddScoped<IReservationService, ReservationService>();
builder.Services.AddScoped<IGenericBLL<Reservation>, GenericBLL<Reservation>>();
builder.Services.AddScoped<IRepository<Reservation>, ReservationRepository>();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Projet DOTNET" });
});
var app = builder.Build();





// Configure the HTTP request pipeline.
app.UseAuthorization();
app.MapControllers();

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V2");
});

app.UseStaticFiles();
app.MapFallbackToFile("index.html"); // Adjust to your main HTML page



app.Run();


