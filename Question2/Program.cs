using Microsoft.EntityFrameworkCore;
using PhoneLookupApi.Data;
using PhoneLookupApi.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddProblemDetails();
builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("PhoneLookupDb"));
builder.Services.AddScoped<IPhoneNumberService, PhoneNumberService>();

var app = builder.Build();

// Seed the in-memory database with the data from Figure 1 and Figure 2.
using (var scope = app.Services.CreateScope())
{
    scope.ServiceProvider.GetRequiredService<AppDbContext>().Database.EnsureCreated();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.MapControllers();

app.Run();
