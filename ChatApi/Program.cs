using ChatApi.Data;
using ChatApi.Entities;
using ChatApi.Services;
using ChatApi.Services.Interfaces;
using ChatApi.WebSocket;
using ChatApi.WebSocket.Handlers;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;

var builder = WebApplication.CreateBuilder(args);


string security_key = builder.Configuration.GetSection("TokenAuthentication")["SecretKey"];
var symetricSecurityKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(security_key));

if (builder.Environment.IsDevelopment())
{
    string connection = builder.Configuration.GetConnectionString("localMySqlConnection");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.IssuerSigningKey = symetricSecurityKey;
        options.TokenValidationParameters.ValidAudience = builder.Configuration.GetSection("TokenAuthentication")["Audience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration.GetSection("TokenAuthentication")["Issuer"];
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    });
}
else
{
    string connection = Environment.GetEnvironmentVariable("MySqlConnectionString");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseMySql(connection, ServerVersion.AutoDetect(connection)));

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    }).AddJwtBearer(options =>
    {
        options.SaveToken = true;
        options.TokenValidationParameters.ValidateIssuerSigningKey = true;
        options.TokenValidationParameters.IssuerSigningKey = symetricSecurityKey;
        options.TokenValidationParameters.ValidAudience = builder.Configuration.GetSection("TokenAuthentication")["Audience"];
        options.TokenValidationParameters.ValidIssuer = builder.Configuration.GetSection("TokenAuthentication")["Issuer"];
        options.TokenValidationParameters.ClockSkew = TimeSpan.Zero;
    });
}
builder.Services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", builder => builder
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
    );
});


builder.Services.AddAuthorization(auth =>
{
    auth.AddPolicy("Bearer", new AuthorizationPolicyBuilder()
        .AddAuthenticationSchemes(JwtBearerDefaults.AuthenticationScheme‌​)
        .RequireAuthenticatedUser().Build());
});

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddResponseCaching();
builder.Services.AddRazorPages();
builder.Services.AddWebSocketManager();
builder.Services.AddSingleton<RoomHandler>();

builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IChatService, ChatService>();
builder.Services.AddScoped<IGroupService, GroupService>();

builder.Services.AddIdentity<User, IdentityRole>(options =>
{
    options.User.AllowedUserNameCharacters = String.Empty;
    options.User.RequireUniqueEmail = true;
    options.Password.RequiredLength = 6;
    options.Password.RequireLowercase = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
}).AddRoleManager<RoleManager<IdentityRole>>()
      .AddRoles<IdentityRole>()
      .AddEntityFrameworkStores<AppDbContext>()
      .AddDefaultTokenProviders();

var app = builder.Build();
app.UseCors("CorsPolicy");

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

var webSocketOptions = new WebSocketOptions()
{
    KeepAliveInterval = TimeSpan.FromSeconds(120),
};

app.UseResponseCaching();
app.UseRouting();
app.UseAuthorization();
app.UseCors("CorsPolicy");

app.UseStaticFiles();

app.UseWebSockets(webSocketOptions);
app.MapWebSocketManager("/ws", app.Services.GetService<RoomHandler>());

app.UseStaticFiles();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
});

app.UseHttpsRedirection();

app.Run();
