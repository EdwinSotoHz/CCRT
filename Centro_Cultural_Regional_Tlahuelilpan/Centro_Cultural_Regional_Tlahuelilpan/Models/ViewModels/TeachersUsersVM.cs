using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class TeachersUsersVM
    {
        public Usuario objUsuario { get; set; }
        public Docente objDocente { get; set; }
        public List<SelectListItem> lstRoles { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Contraseña")]
        public string ConfirmarClave { get; set; }
    }
}