using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    [Table("FolderQueue")]
    public class FolderRecord
    {
        [Key]
        [Column("Id")]
        public int Id { get; set; }
        [Column("Token")]
        public string Token { get; set; }
        [Column("Path")]
        public string Path { get; set; }
        [Column("WasDelivered")]
        public bool WasDelivered { get; set; }
    }
}
