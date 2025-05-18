using System.Diagnostics;
using Centro_Cultural_Regional_Tlahuelilpan.Models;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class TeachersControlController : Controller
    {
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;

        public TeachersControlController(CentroCulturalRegionalTlahuelilpanContext context)
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

        [HttpGet]
        public IActionResult CreateEditStudent(int id = 0)
        {
            var vm = new StudentVM
            {
                Alumno = new Alumno(),
                Expediente = new Expediente(),
                Progreso = new ProgresoEstudiantil { Estado = "Inscrito" },
                GruposDisponibles = _DBContext.Grupos
                    .Where(g => g.Estado == "En curso")
                    .Select(g => new SelectListItem
                    {
                        Value = g.GrupoId.ToString(),
                        Text = $"{g.NombreGrupo} - {g.Taller.NombreTaller} ({g.Horario})"
                    }).ToList()
            };

            if (id != 0)
            {
                vm.Alumno = _DBContext.Alumnos.Find(id);

                vm.Expediente = _DBContext.Expedientes
                    .FirstOrDefault(e => e.AlumnoId == id);

                vm.Progreso = _DBContext.ProgresoEstudiantils
                    .Include(p => p.Grupo) // <-- Aquí se incluye el Grupo
                    .FirstOrDefault(p => p.AlumnoId == id);

                if (vm.Progreso != null && vm.Progreso.GrupoId > 0)
                {
                    int tallerId = vm.Progreso.Grupo.TallerId; // Ya no da error

                    vm.GruposDisponibles = _DBContext.Grupos
                        .Where(g => g.TallerId == tallerId && g.Estado == "En curso")
                        .Select(g => new SelectListItem
                        {
                            Value = g.GrupoId.ToString(),
                            Text = $"{g.NombreGrupo} - {g.Taller.NombreTaller} ({g.Horario})"
                        }).ToList();
                }
            }

            return View(vm);
        }

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

        [HttpPost]
        public IActionResult Confirm_CreateEditStudent(StudentVM vm)
        {
            try
            {
                if (vm.Alumno.AlumnoId == 0)
                {
                    // 👤 Crear nuevo alumno
                    _DBContext.Alumnos.Add(vm.Alumno);
                    _DBContext.SaveChanges();

                    // 📄 Asignar expediente
                    vm.Expediente.AlumnoId = vm.Alumno.AlumnoId;
                    _DBContext.Expedientes.Add(vm.Expediente);

                    // 📚 Registrar progreso
                    if (vm.Progreso.GrupoId > 0)
                    {
                        vm.Progreso.AlumnoId = vm.Alumno.AlumnoId;
                        _DBContext.ProgresoEstudiantils.Add(vm.Progreso);
                    }
                }
                else
                {
                    // 📝 Actualizar alumno
                    _DBContext.Alumnos.Update(vm.Alumno);

                    // 📁 Actualizar o crear expediente
                    var existingExpediente = _DBContext.Expedientes.FirstOrDefault(e => e.AlumnoId == vm.Alumno.AlumnoId);
                    if (existingExpediente != null)
                    {
                        existingExpediente.ActaNacimiento = vm.Expediente.ActaNacimiento;
                        existingExpediente.Curp = vm.Expediente.Curp;
                        existingExpediente.ComprobanteDomicilio = vm.Expediente.ComprobanteDomicilio;
                        existingExpediente.Ine = vm.Expediente.Ine;
                        existingExpediente.CertificadoMedico = vm.Expediente.CertificadoMedico;
                        existingExpediente.ReciboPago = vm.Expediente.ReciboPago;
                        existingExpediente.Fotografias = vm.Expediente.Fotografias;
                        existingExpediente.DocumentosCompletos = vm.Expediente.DocumentosCompletos;

                        _DBContext.Expedientes.Update(existingExpediente);
                    }
                    else
                    {
                        vm.Expediente.AlumnoId = vm.Alumno.AlumnoId;
                        _DBContext.Expedientes.Add(vm.Expediente);
                    }

                    // 📚 Actualizar progreso
                    var existingProgreso = _DBContext.ProgresoEstudiantils
                        .FirstOrDefault(p => p.AlumnoId == vm.Alumno.AlumnoId);

                    if (existingProgreso != null)
                    {
                        existingProgreso.GrupoId = vm.Progreso.GrupoId;
                        existingProgreso.Estado = vm.Progreso.Estado;
                        _DBContext.ProgresoEstudiantils.Update(existingProgreso);
                    }
                    else if (vm.Progreso.GrupoId > 0)
                    {
                        vm.Progreso.AlumnoId = vm.Alumno.AlumnoId;
                        _DBContext.ProgresoEstudiantils.Add(vm.Progreso);
                    }
                }

                _DBContext.SaveChanges();
                TempData["SuccessMessage"] = "✅ Alumno guardado exitosamente.";
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"❌ Excepción: {ex.Message}\n{ex.StackTrace}");
                TempData["ErrorMessage"] = $"❌ Ocurrió un error: {ex.Message}";
                return View("CreateEditStudent", vm);
            }

            return RedirectToAction("TeachersDashboard");
        }

        [HttpGet]
        public IActionResult DeleteStudent(int id)
        {
            var alumno = _DBContext.Alumnos
                .Include(a => a.Expediente)
                .Include(a => a.ProgresoEstudiantils)
                .FirstOrDefault(a => a.AlumnoId == id);

            if (alumno == null)
            {
                return NotFound();
            }

            return View(alumno);
        }

        [HttpPost]
        public IActionResult Confirm_DeleteStudent(int AlumnoId)
        {
            var alumno = _DBContext.Alumnos
                .Include(a => a.Expediente)
                .Include(a => a.ProgresoEstudiantils)
                .FirstOrDefault(a => a.AlumnoId == AlumnoId);

            if (alumno == null)
            {
                return NotFound();
            }

            // Eliminar progresos primero
            _DBContext.ProgresoEstudiantils.RemoveRange(alumno.ProgresoEstudiantils);

            // Eliminar expediente
            if (alumno.Expediente != null)
            {
                _DBContext.Expedientes.Remove(alumno.Expediente);
            }

            // Finalmente eliminar alumno
            _DBContext.Alumnos.Remove(alumno);
            _DBContext.SaveChanges();

            return RedirectToAction("TeachersDashboard");
        }
    }
}
