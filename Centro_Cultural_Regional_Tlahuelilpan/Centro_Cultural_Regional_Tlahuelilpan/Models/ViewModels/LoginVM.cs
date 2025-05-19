using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class LoginVM
    {
        [Required(ErrorMessage = "El nombre de usuario es obligatorio")]
        [Display(Name = "Nombre de Usuario")]
        public string NombreUsuario { get; set; } = null!;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [DataType(DataType.Password)]
        public string Contraseña { get; set; } = null!;

        [Required(ErrorMessage = "El rol es obligatorio")]
        [Display(Name = "Rol")]
        public int RolId { get; set; }

        // Para mostrar opciones en la vista
        public List<SelectListItem> RolesDisponibles { get; set; } = new List<SelectListItem>();
    }
}