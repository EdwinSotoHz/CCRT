using Microsoft.AspNetCore.Mvc.Rendering;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class StudentGroupVM
    {
        public int AlumnoId { get; set; }
        public string NombreAlumno { get; set; } = string.Empty;
        public int GrupoId { get; set; }

        // Listas para dropdowns
        public List<SelectListItem> GruposDisponibles { get; set; } = new List<SelectListItem>();
    }
}