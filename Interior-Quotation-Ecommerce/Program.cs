using Interior_Quotation_Ecommerce.Mapper;
using Interior_Quotation_Ecommerce.MongoDB.Implements;
using Interior_Quotation_Ecommerce.MongoDB.Interfaces;
using Interior_Quotation_Ecommerce.Repository;
using Interior_Quotation_Ecommerce.Repository.Implements;
using Interior_Quotation_Ecommerce.Repository.Interfaces;
using Interior_Quotation_Ecommerce.Services.Implements;
using Interior_Quotation_Ecommerce.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero,
            ValidIssuer = builder.Configuration["Jwt:Issuer"],
            ValidAudience = builder.Configuration["Jwt:Audience"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
        };
    });
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

// Add services to the container.
builder.Services.AddScoped<IAccountsService, AccountsService>();
builder.Services.AddScoped<ICategoriesService, CategoriesService>();
builder.Services.AddScoped<IConstructsService, ConstructsService>();
builder.Services.AddScoped<IProductsService, ProductsService>();
builder.Services.AddScoped<IContractorsService, ContractorsService>();
builder.Services.AddScoped<IConstructProductsService, ConstructProductsService>();
builder.Services.AddScoped<IConstructImagesService, ConstructImagesService>();
// Add repositories to the container.
builder.Services.AddScoped<IConstructImagesRepository, ConstructImagesRepository>();
builder.Services.AddScoped<IProductImagesRepository, ProductImagesRepository>();

//Add automapper
builder.Services.AddAutoMapper(typeof(Program), typeof(MapperProfile));

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors();
builder.Services.AddDbContext<ApplicationDbContext>(options => 
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// CORS
builder.Services.AddCors(options => options.AddDefaultPolicy(policy => policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

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

app.UseCors();

app.Run();
