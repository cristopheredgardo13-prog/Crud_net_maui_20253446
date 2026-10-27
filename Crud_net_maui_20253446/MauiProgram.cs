using Microsoft.Extensions.DependencyInjection;
namespace Crud_net_maui_20253446;
public static class MauiProgram
{
 public static MauiApp CreateMauiApp()
 {
  SQLitePCL.Batteries_V2.Init();
  var builder = MauiApp.CreateBuilder();
  builder.UseMauiApp<App>();
  builder.Services.AddSingleton<LocalDbService>();
  builder.Services.AddTransient<MainPage>();
  return builder.Build();
 }
}
