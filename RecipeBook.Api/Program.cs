
using RecipeBook.Api.Extensions;
using RecipeBook.Api.Middlewares;



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGenExtension();

builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();/// szczego�y wyj�tk�w
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ExceptionHandlerMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.Run();
