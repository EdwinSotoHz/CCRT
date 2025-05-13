using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Expediente
{
    public int ExpedienteId { get; set; }

    public int AlumnoId { get; set; }

    public bool? ActaNacimiento { get; set; }

    public bool? Curp { get; set; }

    public bool? ComprobanteDomicilio { get; set; }

    public bool? Ine { get; set; }

    public bool? CertificadoMedico { get; set; }

    public bool? ReciboPago { get; set; }

    public bool? Fotografias { get; set; }

    public bool? DocumentosCompletos { get; set; }

    public virtual Alumno Alumno { get; set; } = null!;
}
