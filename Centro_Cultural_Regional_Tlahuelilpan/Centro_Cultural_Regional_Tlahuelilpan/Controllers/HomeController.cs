using System.Diagnostics;
using Centro_Cultural_Regional_Tlahuelilpan.Models;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Text;


using System.Security.Cryptography;

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

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        /********************************************** LOGIN ***********************************************/
        [HttpGet]
        public IActionResult Login()
        {
            var vm = new LoginVM
            {
                RolesDisponibles = _DBContext.Roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.RolId.ToString(),
                        Text = r.Nombre
                    }).ToList()
            };

            return View(vm);
        }

        [HttpPost]
        public IActionResult Login(LoginVM vm)
        {
            if (!ModelState.IsValid)
            {
                vm.RolesDisponibles = _DBContext.Roles
                    .Select(r => new SelectListItem
                    {
                        Value = r.RolId.ToString(),
                        Text = r.Nombre
                    }).ToList();

                return View(vm);
            }

            // Buscar usuario por nombre y rol
            var usuarioEncontrado = _DBContext.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefault(u => u.NombreUsuario == vm.NombreUsuario && u.RolId == vm.RolId);

            if (usuarioEncontrado == null)
            {
                TempData["ErrorMessage"] = "Usuario o contraseña incorrectos";
                return RedirectToAction("Login");
            }

            // Hashear la contraseña ingresada y comparar
            var contraseñaHash = Convert.ToHexString(
                SHA256.HashData(Encoding.UTF8.GetBytes(vm.Contraseña)));

            if (usuarioEncontrado.Contraseña != contraseñaHash)
            {
                TempData["ErrorMessage"] = "Contraseña incorrecta";
                return RedirectToAction("Login");
            }

            // Si es Docente, mandar sus grupos al ViewBag
            if (usuarioEncontrado.Rol.Nombre == "Docente")
            {
                var docenteId = usuarioEncontrado.DocenteId;

                var gruposDelDocente = _DBContext.Grupos
                    .Where(g => g.DocenteId == docenteId)
                    .Select(g => g.NombreGrupo)
                    .ToList();

                ViewBag.Grupos = gruposDelDocente;
                return RedirectToAction("TeachersDashboard", "TeachersDashboard");
            }

            // Si es Administrador
            if (usuarioEncontrado.Rol.Nombre == "Administrador")
            {
                return RedirectToAction("Dashboard", "Home");
            }

            // Otros roles no autorizados
            TempData["ErrorMessage"] = "Acceso denegado para este rol.";
            return RedirectToAction("Login");
        }
    }
}
