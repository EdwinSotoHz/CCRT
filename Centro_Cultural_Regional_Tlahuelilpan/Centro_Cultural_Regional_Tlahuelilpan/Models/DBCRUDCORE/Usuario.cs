using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Usuario
{
    public int UsuarioId { get; set; }

    public int DocenteId { get; set; }

    public string NombreUsuario { get; set; } = null!;

    public string Contraseña { get; set; } = null!;

    public int RolId { get; set; }

    public virtual Docente Docente { get; set; } = null!;

    public virtual Role Rol { get; set; } = null!;
}
