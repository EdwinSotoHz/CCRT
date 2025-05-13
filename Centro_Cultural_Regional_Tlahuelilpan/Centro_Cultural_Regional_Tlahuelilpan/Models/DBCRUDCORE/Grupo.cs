using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Grupo
{
    public int GrupoId { get; set; }

    public int TallerId { get; set; }

    public int DocenteId { get; set; }

    public string NombreGrupo { get; set; } = null!;

    public string Horario { get; set; } = null!;

    public string? Aula { get; set; }

    public DateOnly? FechaInicio { get; set; }

    public DateOnly? FechaFin { get; set; }

    public string? Estado { get; set; }

    public virtual Docente Docente { get; set; } = null!;

    public virtual ICollection<ProgresoEstudiantil> ProgresoEstudiantils { get; set; } = new List<ProgresoEstudiantil>();

    public virtual Tallere Taller { get; set; } = null!;
}
