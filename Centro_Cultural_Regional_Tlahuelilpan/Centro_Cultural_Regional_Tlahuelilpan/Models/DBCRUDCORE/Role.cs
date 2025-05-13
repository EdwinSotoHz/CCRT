using System;
using System.Collections.Generic;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

public partial class Role
{
    public int RolId { get; set; }

    public string Nombre { get; set; } = null!;

    public string? Descripcion { get; set; }
}
