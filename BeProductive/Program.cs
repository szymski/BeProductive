global using AppContext = BeProductive.Modules.Common.Persistence.AppContext;

using BeProductive.Modules.Common.Infrastructure;
using BeProductive.Modules.Common.Persistence;
using BeProductive.Modules.Goals.Infrastructure;
using BeProductive.Modules.Rituals.Infrastructure;
using Plk.Blazor.DragDrop;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddBlazorDragDrop();

builder.Services.AddCommonModule(options =>
{
    options.DbFile = "database.db";
});
builder.Services.AddGoalsModule();
builder.Services.AddRitualsModule();

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<AppContext>();
await DbUtils.InitializeDb(context);

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

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();