using BlogPetNews.API.Extensions;
using BlogPetNews.API.Infra.Utils;
using BlogPetNews.API.Presentation.News;
using BlogPetNews.API.Presentation.Users;

using MediatR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbServices(builder.Configuration);
builder.Services.AddTransient<TokenService>();
builder.Services.AddMediatR(typeof(Program));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.ApplyMigrations();
app.UseHttpsRedirection();

// Endpoints
app.AddNewsEndpoints();
app.AddUserEndpoints();

app.Run();
