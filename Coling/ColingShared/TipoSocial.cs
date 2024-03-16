using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ColingShared
{
    public class TipoSocial
    {
        [Key] 
        public int Id { get; set; }
        public string? NombreSocial { get; set; }
        public string? Estado {  get; set; }   
    }
}
