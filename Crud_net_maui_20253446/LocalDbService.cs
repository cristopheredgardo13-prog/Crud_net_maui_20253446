using SQLite;
namespace Crud_net_maui_20253446;
// Una conexión por aplicación; la tabla se inicializa de forma asíncrona antes de usarla.
public sealed class LocalDbService
{
 private readonly SQLiteAsyncConnection _connection;
 private readonly SemaphoreSlim _initialization = new(1, 1);
 private bool _ready;
 public LocalDbService() : this(Path.Combine(FileSystem.AppDataDirectory, "clientes20253446.db3")) { }
 public LocalDbService(string path)
 {
  _connection = new SQLiteAsyncConnection(path,
   SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.FullMutex);
 }
 private async Task InitializeAsync()
 {
  await _initialization.WaitAsync();
  try { if (!_ready) { await _connection.CreateTableAsync<Cliente>(); _ready = true; } }
  finally { _initialization.Release(); }
 }
 public async Task<List<Cliente>> GetAllAsync()
 { await InitializeAsync(); return await _connection.Table<Cliente>().OrderByDescending(x => x.Id).ToListAsync(); }
 public async Task<Cliente?> GetByIdAsync(int id)
 { await InitializeAsync(); return await _connection.FindAsync<Cliente>(id); }
 public async Task CreateAsync(Cliente item)
 { await InitializeAsync(); await _connection.InsertAsync(item); }
 public async Task UpdateAsync(Cliente item)
 {
  await InitializeAsync();
  if (await _connection.UpdateAsync(item) != 1) throw new InvalidOperationException("El registro ya no existe.");
 }
 public async Task DeleteAsync(Cliente item)
 {
  await InitializeAsync();
  if (await _connection.DeleteAsync(item) != 1) throw new InvalidOperationException("El registro ya no existe.");
 }
}
