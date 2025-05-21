using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class GraduatesVM
    {
        public string Taller { get; set; } = string.Empty;
        public string Grupo { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
        public decimal? Calificacion { get; set; }
        public decimal? Asistencia { get; set; }
        public string Telefono { get; set; } = string.Empty;
        public string AdultoResponsable { get; set; } = string.Empty;
        public string TelefonoResponsable { get; set; } = string.Empty;
    }
}