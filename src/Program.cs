using EvApplicationApi.Helpers;
using EvApplicationApi.Repositories.Interfaces;
using EvApplicationApi.Repository;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;
using src.Repositories;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("WebApiDatabase"))
);

builder.Services.AddScoped<IApplicationRepository, ApplicationRepository>();
builder.Services.AddScoped<IFileUploadRepository, FileUploadRepository>();

builder.Services.AddRateLimiter(options =>
    options
        .AddFixedWindowLimiter(
            policyName: "startApplication_fixed",
            options =>
            {
                options.PermitLimit = 4;
                options.Window = TimeSpan.FromSeconds(12);
            }
        )
        .RejectionStatusCode = StatusCodes.Status429TooManyRequests
);

builder.Services.AddRateLimiter(options =>
    options
        .AddFixedWindowLimiter(
            policyName: "uploadFile_fixed",
            options =>
            {
                options.PermitLimit = 10;
                options.Window = TimeSpan.FromSeconds(15);
            }
        )
        .RejectionStatusCode = StatusCodes.Status429TooManyRequests
);

var app = builder.Build();

app.UseRateLimiter();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

// app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
