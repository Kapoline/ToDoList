using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using DataAccess;
using Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using ToDoList;
using ToDoList.Repos;

var MyAllowSpecificOrigins = "_myAllowSpecificOrigins";  

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddTransient<Seed>();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<INoteRepo,NoteRepo>();
builder.Services.AddScoped<IUserRepo, UserRepo>();
 
//add CORS
builder.Services.AddCors(options =>
    options.AddPolicy(name: MyAllowSpecificOrigins, policy =>
    {
        policy.WithOrigins("http://localhost:5124", "http://localhost:3000");
    }));

builder.Services.AddDbContext<DataContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("Connection")));

/*builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
    }); */
builder.Services.AddAuthentication();
builder.Services.AddAuthorization();

var app = builder.Build();

if (args.Length == 1 && args[0].ToLower() == "seeddata")
    SeedData(app);

void SeedData(IHost app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using (var scope = scopedFactory.CreateScope())
    {
        var service = scope.ServiceProvider.GetService<Seed>();
        if (service != null) service.SeedContext();
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

//Authorization
/*app.Map("/login/{username}", (string username) =>
{
    var claims = new List<Claim> {new Claim(ClaimTypes.Name, username)};
    var jwt = new JwtSecurityToken(
        issuer: AuthOptions.ISSUER,
        audience: AuthOptions.AUDIENCE,
        claims: claims,
        expires: DateTime.UtcNow.Add(TimeSpan.FromMinutes(2)),
        signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(),
            SecurityAlgorithms.HmacSha256));
});*/


/*app.MapPost("/login", (User userdata, DataContext dataContext) =>
{
    User? user = dataContext.Users.FirstOrDefault(x => x.Email == userdata.Email && x.Password == userdata.Password);
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
});*/

/*app.Map("/note", [Authorize](HttpContext httpContext) => $"Hello!");*/


app.UseCors(MyAllowSpecificOrigins);

app.MapControllers();

app.Run();