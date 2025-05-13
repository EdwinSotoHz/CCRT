using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class ProgresoEstudiantil
{
    public int ProgresoId { get; set; }

    public int AlumnoId { get; set; }

    public int GrupoId { get; set; }

    public string? Estado { get; set; }

    public decimal? Calificacion { get; set; }

    public decimal? Asistencia { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;

    public virtual Grupo Grupo { get; set; } = null!;
}
