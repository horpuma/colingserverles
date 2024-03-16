using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ColingShared;

namespace Coling.API.Afiliados
{
    public class Contexto: DbContext
    {
        public Contexto(DbContextOptions<Contexto> options):base(options) 
        {

        }
        public virtual DbSet<Persona>Personas { get; set; }
        public virtual DbSet<Telefono> Telefonos { get; set; }
        public virtual DbSet<Afiliado> Afiliados { get; set; }
        public virtual DbSet<Direccion> Direccions { get; set; }
        public virtual DbSet<PersonaTipoSocial> PersonaTipoSocials { get; set; }
        public virtual DbSet<ProfesionAfiliado> ProfesionAfiliados { get; set; }
        public virtual DbSet<TipoSocial> TipoSocials { get; set; }
    }
}
