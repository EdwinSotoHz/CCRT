using Microsoft.AspNetCore.Mvc.Rendering;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class StudentVM
    {
        public Alumno Alumno { get; set; }
        public Expediente Expediente { get; set; }
        public ProgresoEstudiantil Progreso { get; set; }

        // Listas para dropdowns
        public List<SelectListItem> GruposDisponibles { get; set; } = new List<SelectListItem>();
    }
}