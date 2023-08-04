using Chat.Web;

var builder = WebApplication.CreateBuilder(args);
var services = builder.Services;

services.AddControllersWithViews();

//MessageConsumer consome automatico as mensagens
//services.AddHostedService<MessageConsumer>();

services.Configure<RabbitMqConfiguration>(builder.Configuration.GetSection("RabbitMqConfig"));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Messages}/{action=Index}/{id?}");

app.Run();
