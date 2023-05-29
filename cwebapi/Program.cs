using Newtonsoft.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//Enable CORS
builder.Services.AddCors(c=>{
    c.AddPolicy("AllowOrigin",options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

//JSON Serializer
builder.Services.AddControllersWithViews().AddNewtonsoftJson(options=>options.SerializerSettings.ReferenceLoopHandling=NewtonsoftJson.ReferenceLoopHandling.Ignore).AddNewtonsoftJson(options=>options.SerializerSettings.ContractResolver = new DefaultContractResolver());

builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

//Enable CORS
app.UseCors(options=>options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.UseStaticFiles(new StaticFileOptions
{
    // Requires the following import:
    // using System.IO;
    // using Microsoft.Extensions.FileProviders;
    FileProvider = new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), "Resources")),
    RequestPath = "/Resources",
        
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=600");
    }
});

app.Run();
