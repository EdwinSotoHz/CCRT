using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class StudentDetailsVM
    {
        public Alumno Alumno { get; set; }
        public Expediente Expediente { get; set; }
        public List<ProgresoEstudiantil> Progresos { get; set; }
        public List<Grupo> Grupos { get; set; }
    }
}