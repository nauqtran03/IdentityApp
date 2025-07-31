
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using System.Linq;
using System.Text;
using WebApi.Data;
using WebApi.Models;
using WebApi.Services;

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddDbContext<Context>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });
            //defining our IdentityCore Service
            builder.Services.AddIdentityCore<User>(options =>
            {
                //password configuration
                options.Password.RequireDigit = true;
                options.Password.RequireLowercase = true;
                options.Password.RequireNonAlphanumeric = true;
                options.Password.RequireUppercase = true;
                options.Password.RequiredLength = 6;
                //for email confirmation
                options.SignIn.RequireConfirmedAccount = true;
            })
                .AddRoles<IdentityRole>() //be able to add roles
                .AddRoleManager<RoleManager<IdentityRole>>()//be able to make use of RoleManager
                .AddEntityFrameworkStores<Context>()//providing our context
                .AddSignInManager<SignInManager<User>>()//make use of SignInManager
                .AddUserManager<UserManager<User>>()//make use of UserManager to create user
                .AddDefaultTokenProviders();// be able to create tokens for email confirmation 
            // be able to authenticate users using JWT
            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        //Validate the token based on the key we have provided inside appsettings.development.json
                        ValidateIssuerSigningKey = true,
                        //the issuer signning key based on JWT:Key
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"])),
                        //the issuer which in here is the api project url we are using
                        ValidIssuer = builder.Configuration["JWT:Issuer"],
                        //validate the issuer (who ever is issuing the JWT)
                        ValidateIssuer = true,
                        // don't validate audience (angular side)
                        ValidateAudience = false, 
                    };
                });
            builder.Services.AddScoped<JWTService>();
            builder.Services.AddCors();

            builder.Services.Configure<ApiBehaviorOptions>(options =>
            {
                options.InvalidModelStateResponseFactory = actionContext =>
                {
                    var errors = actionContext.ModelState
                    .Where(x => x.Value.Errors.Count > 0)
                    .SelectMany(x => x.Value.Errors)
                    .Select(x => x.ErrorMessage).ToArray();

                    var toReturn = new
                    {
                        Errors = errors,
                    };
                    return new BadRequestObjectResult(toReturn);
                };
            });

            var app = builder.Build();

            app.UseCors(options =>
            {
                options.AllowAnyMethod() // allow any method (GET, POST, PUT, DELETE)
                       .AllowAnyHeader()
                       .AllowCredentials()
                       .WithOrigins(builder.Configuration["JWT:ClientUrl"]); 
            });

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
