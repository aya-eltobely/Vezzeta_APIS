using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;
using VezetaApi.Controllers;
using VezetaApi.Helper;
using VezetaApi.Interfaces;
using VezetaApi.Models;
using VezetaApi.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();



builder.Services.AddDbContext<ApplicationDbContext>(option =>
                option.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

///

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", builder =>
    {
        builder.AllowAnyOrigin()//.WithOrigins("http://127.0.0.1:5500") // Replace with your client application's origin
               .AllowAnyHeader()
               .AllowAnyMethod();
    });
});
///


builder.Services.AddIdentity<ApplicationUser, IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
                 //.AddDefaultTokenProviders();

//service to check for authentication  (Token)
builder.Services.AddAuthentication(option =>
{
    option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme; //check for token valid or not
    option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;  //if token not valid return user to login
    option.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
}).AddJwtBearer(option =>
{
    option.SaveToken = true;
    option.RequireHttpsMetadata = false;
    option.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidateIssuer = true,
        ValidIssuer = builder.Configuration["JWT:issuer"],
        ValidateAudience = true,
        ValidAudience = builder.Configuration["JWT:audiance"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:secretKey"]))
    };
});


//builder.Services.AddAuthorization(options =>
//{
//    options.AddPolicy("AdminPolicy", policy => policy.RequireRole(WebSiteRoles.SiteAdmin,WebSiteRoles.SiteDoctor,WebSiteRoles.SitePatient));
//});

////////////////////////////////////(for testing in swagger add this service) make swagger authorize for token 
///

builder.Services.AddSwaggerGen(setup =>
{
    // Include 'SecurityScheme' to use JWT Authentication
    var jwtSecurityScheme = new OpenApiSecurityScheme
    {
        BearerFormat = "JWT",
        Name = "JWT Authentication",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = JwtBearerDefaults.AuthenticationScheme,
        Description = "Put **_ONLY_** your JWT Bearer token on textbox below!",

        Reference = new OpenApiReference
        {
            Id = JwtBearerDefaults.AuthenticationScheme,
            Type = ReferenceType.SecurityScheme
        }
    };

    setup.AddSecurityDefinition(jwtSecurityScheme.Reference.Id, jwtSecurityScheme);

    setup.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        { jwtSecurityScheme, Array.Empty<string>() }
    });

});

//////////////////////////////////////////////////
///

builder.Services.AddScoped(typeof(IBaseRepository<>), typeof(BaseRepository<>));
builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddRazorPages();





var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

/// SeedData
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    dbContext.Database.Migrate(); // Apply Migrations
    SeedData.Seed(scope.ServiceProvider);
}


app.UseCors("AllowSpecificOrigin");
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();


