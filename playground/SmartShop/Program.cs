using CoreAssistant.Extensions;
using SmartShop.Hubs;

var builder = WebApplication.CreateBuilder(args);

// Add CoreAssistant services to the container.
builder.Services.AddCoreAssistant(options => { 
    options.ApiKey = builder.Configuration["CoreAssistantOptions:ApiKey"];
    options.DefaultContext = builder.Configuration["CoreAssistantOptions:DefaultContext"];
});

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSignalR();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();
app.MapHub<AssistantHub>("/assistant");

app.Run();
