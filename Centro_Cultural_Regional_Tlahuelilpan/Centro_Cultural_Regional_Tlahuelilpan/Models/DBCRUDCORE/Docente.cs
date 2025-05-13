using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Docente
{
    public int DocenteId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public string? Localidad { get; set; }

    public string NumeroContacto { get; set; } = null!;

    public string? Email { get; set; }

    public virtual ICollection<Grupo> Grupos { get; set; } = new List<Grupo>();
}
