using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels
{
    public class GrupoVM
    {
        public Grupo objGrupo { get; set; }
        public List<SelectListItem> lstTalleres { get; set; }
        public List<SelectListItem> lstDocentes { get; set; }
    }
}