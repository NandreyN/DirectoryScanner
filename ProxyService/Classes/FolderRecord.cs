using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ProxyService.Classes
{
    [Table("FolderQueue")]
    public class FolderRecord
    {
        [Key] [Column("Id")] public int Id { get; set; }

        [Column("Token")] public string Token { get; set; }

        [Column("InnerToken")] public string InnerToken { get; set; }

        [Column("Path")] public string Path { get; set; }

        [Column("WasDelivered")] public bool WasDelivered { get; set; }

        [Column("WasSent")] public bool WasSent { get; set; }
    }
}
