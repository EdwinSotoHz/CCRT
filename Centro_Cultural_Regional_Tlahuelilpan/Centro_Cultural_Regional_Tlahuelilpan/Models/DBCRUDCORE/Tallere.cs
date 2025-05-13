using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Tallere
{
    public int TallerId { get; set; }

    public string NombreTaller { get; set; } = null!;

    public string? Descripcion { get; set; }

    public decimal? Precio { get; set; }

    public string? Duracion { get; set; }

    public string? UrlImagen { get; set; }

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
