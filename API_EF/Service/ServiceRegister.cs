using API_EF.Data;
using API_EF.Helper;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace API_EF.Service
{
    public static class ServiceRegister
    {

        public static IdentityBuilder AddApplicationIdentity(this IServiceCollection services)
        {
            return services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                //Setting for sign in
                options.SignIn.RequireConfirmedAccount = false;

                //Setting Password
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 5;
                options.Password.RequireLowercase = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequiredUniqueChars = 0;


                //LockOut Setting
                //if user lock over 5 time , this account will locked
                options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(5);
                options.Lockout.MaxFailedAccessAttempts = 5;
                options.Lockout.AllowedForNewUsers = true;

                //User Setting
                options.User.AllowedUserNameCharacters =
                   "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789-._@+";
                options.User.RequireUniqueEmail = true;

            }).AddEntityFrameworkStores<DataContext>().AddDefaultTokenProviders();


        }
        public static IServiceCollection AddApplicationJwtAuthentication(this IServiceCollection service, IConfiguration config)
        {
            service
                .AddAuthentication(options =>
                {
                    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                })
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters()
                    {
                        ValidateActor = true,
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        RequireExpirationTime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = config.GetSection("Jwt:Issuer").Value,
                        ValidAudience = config.GetSection("Jwt:Audience").Value,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.GetSection("Jwt:Key").Value))
                    };

                    /* options.Events = new JwtBearerEvents()
                     {
                         OnMessageReceived = context =>
                         {
                             context.Token = context.Request.Cookies[TypeSafe.CookiesName.Token];
                             return Task.CompletedTask;
                         }
                     };*/
                });

            return service;
        }


        public static IServiceCollection AddApplitaionAuthenciation(this IServiceCollection service)
        {
            service.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme).AddCookie(options =>
            {
                options.Cookie.Name = CookieAuthenticationDefaults.AuthenticationScheme;
                options.ExpireTimeSpan = TimeSpan.FromDays(1);
                options.Cookie.HttpOnly = true;
                options.AccessDeniedPath = "/AuthenController/Login";
                options.LogoutPath = "/AuthenController/Login";
                options.SlidingExpiration = true;
            });


            return service;
        }

        public static IServiceCollection AddApplicationAuthorization(this IServiceCollection services)
        {
            services.AddAuthorization(options =>
            {

                //Require Role
                options.AddPolicy(TypeSafe.Policy.FullPolicy, policy =>
                {
                    policy.RequireRole("Admin");
                });

                //Require Claim
                //I Think controller with authori with pilicy same it and user have claim same claim require then i accept it
                options.AddPolicy(TypeSafe.Policy.FullPolicy, policy =>
                {
                    policy.RequireClaim("AdminController",
                        TypeSafe.Permistion.Read.ToString(),
                        TypeSafe.Permistion.Write.ToString());
                });

            


            });
            return services;
        }


        }
}
