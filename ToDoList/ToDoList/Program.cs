using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoList;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection")));
builder.Services.AddAuthorization();
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidIssuer = AuthOptions.ISSUER,
            ValidateAudience = true,
            ValidAudience = AuthOptions.AUDIENCE,
            ValidateLifetime = true,
            IssuerSigningKey = AuthOptions.GetSymmetricSecurityKey(),
            ValidateIssuerSigningKey = true
        };
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapGet("/note", async (DataContext db) =>
    await db.Notes.ToListAsync());

app.MapGet("/note/{noteId}", async (int id, DataContext db) =>
    await db.Notes.FindAsync(id) 
        is Note note 
        ? Results.Ok(note) 
        : Results.NotFound());

app.MapGet("/note/complete", async (DataContext db) =>
    await db.Notes.Where(x => x.IsCompleted).ToListAsync());



app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

//Authorization
app.Map("/login/{username}", (string username) =>
{
    var claims = new List<Claim> {new Claim(ClaimTypes.Name, username)};
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256));
});

app.Map("/data", [Authorize](HttpContext httpContext) => $"Hello!");

var users = new List<User>()
{
    new User
    {
        Email = "admin@main.com",
        Password = "admin123"
    }
};

app.MapPost("/login", (User userdata) =>
{
    User? user = users.FirstOrDefault(x => x.Email == userdata.Email && x.Password == userdata.Password);
    if (user is null) return Results.NotFound();

    var claims = new List<Claim> {new Claim(ClaimTypes.Name, user.Email)};

    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256));
    var encodedJWT = new JwtSecurityTokenHandler().WriteToken(jwt);

    var responce = new
    {
        access_token = encodedJWT,
        username = user.Email
    };
    return Results.Json(responce);
});

app.MapControllers();

app.Run();