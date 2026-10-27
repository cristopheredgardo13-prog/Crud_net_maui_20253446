namespace Crud_net_maui_20253446;
public partial class MainPage : ContentPage
{
 private readonly LocalDbService _db;
 private int _editingId;
 private bool _busy;
 public MainPage(LocalDbService db) { InitializeComponent(); _db = db; }
 protected override async void OnAppearing()
 {
  base.OnAppearing();
  await RunAsync(RefreshAsync);
 }
 private async Task RefreshAsync()
 {
  var items = await _db.GetAllAsync();
  Records.ItemsSource = items;
  StatusLabel.Text = $"Registros guardados: {items.Count}";
 }
 // Evita que dos pulsaciones creen o modifiquen simultáneamente el mismo registro.
 private async Task RunAsync(Func<Task> action)
 {
  if (_busy) return;
  _busy = true; SaveButton.IsEnabled = false; CancelButton.IsEnabled = false; Records.IsEnabled = false;
  try { await action(); }
  catch (OverflowException) { await DisplayAlert("Número demasiado grande", "La suma supera el rango permitido.", "Aceptar"); }
  catch (ArgumentException ex) { await DisplayAlert("Revisa los datos", ex.Message, "Aceptar"); }
  catch (Exception ex) { await DisplayAlert("No se pudo completar", ex.Message, "Aceptar"); }
  finally { _busy = false; SaveButton.IsEnabled = true; CancelButton.IsEnabled = true; Records.IsEnabled = true; }
 }
 private async void SaveClicked(object? sender, EventArgs e) => await RunAsync(async () =>
 {
  var nombre = NameEntry.Text?.Trim() ?? "";
  var email = EmailEntry.Text?.Trim() ?? "";
  var movil = PhoneEntry.Text?.Trim() ?? "";
  if (string.IsNullOrWhiteSpace(nombre)) throw new ArgumentException("Ingresa el nombre del cliente.");
  if (!System.Net.Mail.MailAddress.TryCreate(email, out var address) || address.Address != email)
   throw new ArgumentException("Ingresa un correo electrónico válido.");
  if (movil.Count(char.IsDigit) < 7 || movil.Any(c => !char.IsDigit(c) && c != '+' && c != '-' && c != ' ' && c != '(' && c != ')'))
   throw new ArgumentException("Ingresa un teléfono válido con al menos 7 dígitos.");
  var item = new Cliente { Id = _editingId, NombreCliente = nombre, Email = email, Movil = movil };
  if (_editingId == 0) await _db.CreateAsync(item); else await _db.UpdateAsync(item);
  ResetForm(); await RefreshAsync();
 });
 private async void RecordSelected(object? sender, SelectionChangedEventArgs e)
 {
  if (e.CurrentSelection.FirstOrDefault() is not Cliente item) return;
  Records.SelectedItem = null;
  await RunAsync(async () =>
  {
   var action = await DisplayActionSheet("Registro #" + item.Id, "Cancelar", "Eliminar", "Editar");
   if (action == "Editar")
   {
    _editingId = item.Id; NameEntry.Text = item.NombreCliente; EmailEntry.Text = item.Email; PhoneEntry.Text = item.Movil;
    ModeLabel.Text = $"Editando registro #{item.Id}"; SaveButton.Text = "Actualizar"; CancelButton.Text = "Cancelar edición";
   }
   else if (action == "Eliminar" && await DisplayAlert("Eliminar registro", "¿Deseas eliminar este registro?", "Eliminar", "Cancelar"))
   {
    await _db.DeleteAsync(item); if (_editingId == item.Id) ResetForm(); await RefreshAsync();
   }
  });
 }
 private void CancelClicked(object? sender, EventArgs e) => ResetForm();
 private void ResetForm()
 {
  _editingId = 0; NameEntry.Text = ""; EmailEntry.Text = ""; PhoneEntry.Text = "";
  ModeLabel.Text = "Nuevo registro"; SaveButton.Text = "Guardar"; CancelButton.Text = "Limpiar";
 }
 
}
