using System.Diagnostics;
using Centro_Cultural_Regional_Tlahuelilpan.Models;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public HomeController(CentroCulturalRegionalTlahuelilpanContext context, IWebHostEnvironment hostingEnvironment, ILogger<HomeController> logger)
        {
            _DBContext = context;
            _hostingEnvironment = hostingEnvironment;
            _logger = logger;
        }

        public IActionResult Index()
        {
            List<Tallere> list = _DBContext.Talleres.ToList();
            return View(list);
        }

        public IActionResult Dashboard()
        {
            var vm = new DashboardVM
            {
                TotalTalleres = _DBContext.Talleres.Count(),
                TotalDocentes = _DBContext.Docentes.Count(),
                TotalGrupos = _DBContext.Grupos.Count(),
                TotalAlumnos = _DBContext.Alumnos.Count()
            };

            return View(vm);
        }

        public IActionResult TeachersAccess()
        {
            var vm = new DashboardVM
            {
                TotalTalleres = _DBContext.Talleres.Count(),
                TotalDocentes = _DBContext.Docentes.Count(),
                TotalGrupos = _DBContext.Grupos.Count(),
                TotalAlumnos = _DBContext.Alumnos.Count()
            };

            return View(vm);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
