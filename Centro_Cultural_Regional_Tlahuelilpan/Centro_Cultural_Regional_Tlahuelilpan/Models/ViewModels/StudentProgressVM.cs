using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class StudentProgressVM
    {
        public int ProgresoId { get; set; }

        public int AlumnoId { get; set; }

        public int GrupoId { get; set; }

        public string NombreAlumno { get; set; } = string.Empty;

        public string NombreGrupo { get; set; } = string.Empty;

        public string TallerNombre { get; set; } = string.Empty;

        public string Estado { get; set; } = "Inscrito";

        public decimal? Calificacion { get; set; }

        public decimal? Asistencia { get; set; }
    }
}