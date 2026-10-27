using SQLite;
namespace Crud_net_maui_20253446;
[Table("cliente")]
public class Cliente
{
 [PrimaryKey, AutoIncrement, Column("id")] public int Id { get; set; }
 [Column("nombrecliente")] public string NombreCliente { get; set; } = "";
 [Column("email")] public string Email { get; set; } = "";
 [Column("movil")] public string Movil { get; set; } = "";
}
