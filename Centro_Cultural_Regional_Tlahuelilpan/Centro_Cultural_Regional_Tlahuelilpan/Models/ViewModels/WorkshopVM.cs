using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class WorkshopVM
    {
        public Tallere objTaller { get; set; }

        [Display(Name = "Imagen del Taller")]
        public IFormFile ImagenArchivo { get; set; }
    }
}
