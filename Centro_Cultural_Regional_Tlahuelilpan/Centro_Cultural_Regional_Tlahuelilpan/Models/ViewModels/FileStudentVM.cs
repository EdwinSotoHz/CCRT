using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class FileStudentVM
    {
        public Alumno Alumno { get; set; } = new();
        public Expediente Expediente { get; set; } = new();
        public bool ActaNacimiento { get; set; }
        public bool Curp { get; set; }
        public bool ComprobanteDomicilio { get; set; }
        public bool Ine { get; set; }
        public bool CertificadoMedico { get; set; }
        public bool ReciboPago { get; set; }
        public bool Fotografias { get; set; }
        public bool DocumentosCompletos { get; set; }
        public bool Becado { get; set; }
    }
}