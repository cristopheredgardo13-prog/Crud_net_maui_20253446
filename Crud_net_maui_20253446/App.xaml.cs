namespace Crud_net_maui_20253446;
public partial class App : Application
{
 private readonly MainPage _page;
 public App(MainPage page) { InitializeComponent(); _page = page; }
 protected override Window CreateWindow(IActivationState? activationState) =>
  new Window(new NavigationPage(_page)) { Width = 620, Height = 800 };
}
