global using AppContext = BeProductive.Modules.Common.Persistence.AppContext;
global using AppContextFactory = Microsoft.EntityFrameworkCore.IDbContextFactory<BeProductive.Modules.Common.Persistence.AppContext>;
using BeProductive.Modules.Common.Domain;
using BeProductive.Modules.Common.Infrastructure;
using BeProductive.Modules.Common.Persistence;
using BeProductive.Modules.Goals.Infrastructure;
using BeProductive.Modules.Rituals.Domain;
using BeProductive.Modules.Rituals.Infrastructure;
using BeProductive.Modules.Settings.Infrastructure;
using BeProductive.Modules.Timer.Infrastructure;
using BeProductive.Modules.Users.Domain;
using BeProductive.Modules.Users.Infrastructure;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Identity;
using Plk.Blazor.DragDrop;
using Serilog;
using Serilog.Events;

#region Logging

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("BeProductive", LogEventLevel.Debug)
    .MinimumLevel.Override("Microsoft.AspNetCore", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Warning)
    // .MinimumLevel.Override("Microsoft.EntityFrameworkCore", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .Destructure.ByTransforming<Goal>(goal => new { goal.Id, goal.UserId, goal.Name })
    .Destructure.ByTransforming<Ritual>(ritual => new { ritual.Id, ritual.UserId, ritual.Type, ritual.Title })
    .Destructure.ByTransforming<User>(user => new { user.Id, user.UserName })
    .Destructure.ByTransformingWhere<BaseEntity>(type => typeof(BaseEntity).IsAssignableFrom(type),
        (ent) => new { Type = ent.GetType().Name, ent.Id })

    // .Destructure.ByTransforming<User>(user => new { user.Id, user.UserName, user.FullName })
    .WriteTo.Console(
        outputTemplate: "[{Timestamp:HH:mm:ss} {Level:u4}] {SourceContext:l}: {Message:lj}{NewLine}{Exception}")
    .CreateLogger();

#endregion

var builder = WebApplication.CreateBuilder(args);
builder.WebHost.UseSerilog();

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAntDesign();
builder.Services.AddBlazorDragDrop();
builder.Services.AddBlazoredLocalStorage();

builder.Services.AddCommonModule(options =>
{
    options.Database = builder.Configuration.GetRequiredSection("Database")
        .Get<CommonModuleExtensions.DbOptions>();
    AppContext.Provider = options.Database.Provider.ToString();
});
builder.Services.AddSettingsModule();
builder.Services.AddUsersModule();
builder.Services.AddGoalsModule();
builder.Services.AddRitualsModule();
builder.Services.AddTimerModule();

var app = builder.Build();

await using var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateAsyncScope();
var context = scope.ServiceProvider.GetRequiredService<AppContext>();
var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
await DbUtils.InitializeDb(context, userManager);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

if (app.Environment.IsProduction())
{
    app.UseHttpsRedirection();
}

app.UseStaticFiles();

app.UseRouting();

app.MapBlazorHub();

app.UseAuthentication();
// app.UseAuthorization();

app.MapFallbackToPage("/_Host");

app.Run();