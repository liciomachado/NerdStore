using MediatR;
using Microsoft.EntityFrameworkCore;
using NerdStore.API.Setup;
using NerdStore.Catalogo.Application.AutoMapper;
using NerdStore.Catalogo.Data;
using NerdStore.Pagamentos.Data;
using NerdStore.Vendas.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connection = builder.Configuration.GetConnectionString("MySQLConnectionString");
builder.Services.AddDbContext<CatalogoContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 20))));
builder.Services.AddDbContext<VendasContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 20))));
builder.Services.AddDbContext<PagamentoContext>(options => options.UseMySql(connection, new MySqlServerVersion(new Version(8, 0, 20))));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddAutoMapper(typeof(DomainToViewModelMappingProfile), typeof(ViewModelToDomainMappingProfile));
builder.Services.AddMediatR(typeof(Program));
builder.Services.RegisterService();

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
