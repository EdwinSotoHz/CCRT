using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Alumno
{
    public int AlumnoId { get; set; }

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateOnly? FechaNacimiento { get; set; }

    public int? Edad { get; set; }

    public string? Localidad { get; set; }

    public string NumeroTelefono { get; set; } = null!;

    public string? CorreoElectronico { get; set; }

    public string? AdultoResponsable { get; set; }

    public string? TelefonoResponsable { get; set; }

    public virtual Expediente? Expediente { get; set; }

    public virtual ICollection<ProgresoEstudiantil> ProgresoEstudiantils { get; set; } = new List<ProgresoEstudiantil>();
}
