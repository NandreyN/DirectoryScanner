using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace ProxyService.Classes
{
    public class QueryItem
    {
        [Column("Folder")]
        string Folder { get; set; }
    }
}
