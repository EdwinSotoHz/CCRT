using System.Diagnostics;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class TeachersDashboardController : Controller
    {
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;

        public TeachersDashboardController(CentroCulturalRegionalTlahuelilpanContext context)
        {
            _DBContext = context;
        }

        // Vista principal (ya la tenemos)
        public IActionResult TeachersDashboard()
        {
            var gruposConAlumnos = _DBContext.Grupos
                .Include(g => g.Taller)
                .Include(g => g.Docente)
                .Include(g => g.ProgresoEstudiantils)
                    .ThenInclude(p => p.Alumno)
                .ToList();

            return View(gruposConAlumnos);
        }
        /************************************************************************************** 
         * Funcion no es necesaria ya que se usa Students/Details.cshtml para ver los detalles
        [HttpGet]
        public IActionResult Details(int id)
        {
            var studentDetails = new StudentDetailsVM
            {
                Alumno = _DBContext.Alumnos.Find(id),
                Expediente = _DBContext.Expedientes.FirstOrDefault(e => e.AlumnoId == id),
                Progresos = _DBContext.ProgresoEstudiantils
                    .Where(p => p.AlumnoId == id)
                    .Include(p => p.Grupo)
                        .ThenInclude(g => g.Taller)
                    .Include(p => p.Grupo)
                        .ThenInclude(g => g.Docente)
                    .ToList(),
                Grupos = _DBContext.Grupos
                    .Where(g => g.ProgresoEstudiantils.Any(p => p.AlumnoId == id))
                    .Include(g => g.Taller)
                    .Include(g => g.Docente)
                    .ToList()
            };

            if (studentDetails.Alumno == null)
            {
                return NotFound();
            }

            return View(studentDetails);
        }
        **************************************************************************************/

        [HttpGet]
        public JsonResult GetGruposByTaller(int tallerId)
        {
            var grupos = _DBContext.Grupos
                .Where(g => g.TallerId == tallerId && g.Estado == "En curso")
                .Select(g => new SelectListItem
                {
                    Value = g.GrupoId.ToString(),
                    Text = $"{g.NombreGrupo} ({g.Horario})"
                }).ToList();

            return Json(grupos);
        }


        [HttpGet]
        public IActionResult EditAlumnProgress(int alumnoId, int grupoId)
        {
            var progreso = _DBContext.ProgresoEstudiantils
                .Include(p => p.Alumno)
                .Include(p => p.Grupo)
                    .ThenInclude(g => g.Taller)
                .FirstOrDefault(p => p.AlumnoId == alumnoId && p.GrupoId == grupoId);

            if (progreso == null)
            {
                return NotFound();
            }

            var vm = new StudentProgressVM
            {
                ProgresoId = progreso.ProgresoId,
                AlumnoId = progreso.AlumnoId,
                GrupoId = progreso.GrupoId,
                NombreAlumno = $"{progreso.Alumno.Nombre} {progreso.Alumno.ApellidoPaterno} {progreso.Alumno.ApellidoMaterno}",
                NombreGrupo = progreso.Grupo.NombreGrupo,
                TallerNombre = progreso.Grupo.Taller.NombreTaller,
                Estado = progreso.Estado,
                Calificacion = progreso.Calificacion,
                Asistencia = progreso.Asistencia
            };

            return View(vm);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Confirm_EditAlumnProgress(StudentProgressVM vm)
        {
            var progreso = _DBContext.ProgresoEstudiantils.Find(vm.ProgresoId);

            if (progreso == null || progreso.AlumnoId != vm.AlumnoId || progreso.GrupoId != vm.GrupoId)
            {
                return NotFound();
            }

            // Solo permitimos editar estos campos
            progreso.Estado = vm.Estado;
            progreso.Calificacion = vm.Calificacion;
            progreso.Asistencia = vm.Asistencia;

            _DBContext.ProgresoEstudiantils.Update(progreso);
            _DBContext.SaveChanges();

            TempData["SuccessMessage"] = "✅ Progreso del alumno actualizado exitosamente.";

            // Redireccionar a la acción Details del StudentsController
            return RedirectToAction("Details", "Students", new { id = vm.AlumnoId });
        }
    }
}
