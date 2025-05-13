using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Microsoft.AspNetCore.Mvc;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class StudentsController : Controller
    {
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;

        public StudentsController(CentroCulturalRegionalTlahuelilpanContext context)
        {
            _DBContext = context;
        }

        public IActionResult Students()
        {
            var students = _DBContext.Alumnos.Include(a => a.Expediente).Include(a => a.ProgresoEstudiantils).ToList();
            return View(students);
        }
    }
}
