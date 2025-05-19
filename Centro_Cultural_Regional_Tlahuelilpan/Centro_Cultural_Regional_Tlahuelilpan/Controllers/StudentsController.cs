using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;
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

        // Vista principal - Lista de todos los alumnos
        public IActionResult Students()
        {
            var alumnos = _DBContext.Alumnos.Include(a => a.Expediente).ToList();
            return View(alumnos);
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

        // Vista Crear / Editar Alumno + Expediente
        [HttpGet]
        public IActionResult CreateEditStudent(int id = 0)
        {
            var vm = new FileStudentVM
            {
                Alumno = new Alumno(),
                Expediente = new Expediente()
            };

            if (id != 0)
            {
                vm.Alumno = _DBContext.Alumnos.Find(id);
                vm.Expediente = _DBContext.Expedientes.FirstOrDefault(e => e.AlumnoId == id) ?? new Expediente();

                // Cargar valores actuales del expediente
                vm.ActaNacimiento = vm.Expediente.ActaNacimiento ?? false;
                vm.Curp = vm.Expediente.Curp ?? false;
                vm.ComprobanteDomicilio = vm.Expediente.ComprobanteDomicilio ?? false;
                vm.Ine = vm.Expediente.Ine ?? false;
                vm.CertificadoMedico = vm.Expediente.CertificadoMedico ?? false;
                vm.ReciboPago = vm.Expediente.ReciboPago ?? false;
                vm.Fotografias = vm.Expediente.Fotografias ?? false;
                vm.DocumentosCompletos = vm.Expediente.DocumentosCompletos ?? false;
            }

            return View(vm);
        }

        // Acción POST para guardar
        [HttpPost]
        public IActionResult Confirm_CreateEditStudent(FileStudentVM vm)
        {
            if (vm.Alumno.AlumnoId == 0)
            {
                // Crear nuevo alumno
                _DBContext.Alumnos.Add(vm.Alumno);
                _DBContext.SaveChanges();

                // Asignar expediente con ID del nuevo alumno
                var expediente = new Expediente
                {
                    AlumnoId = vm.Alumno.AlumnoId,
                    ActaNacimiento = vm.ActaNacimiento,
                    Curp = vm.Curp,
                    ComprobanteDomicilio = vm.ComprobanteDomicilio,
                    Ine = vm.Ine,
                    CertificadoMedico = vm.CertificadoMedico,
                    ReciboPago = vm.ReciboPago,
                    Fotografias = vm.Fotografias,
                    DocumentosCompletos = vm.DocumentosCompletos
                };

                _DBContext.Expedientes.Add(expediente);
            }
            else
            {
                // 📝 Actualizar alumno
                _DBContext.Alumnos.Update(vm.Alumno);

                // 📁 Obtener expediente existente o crear uno si no existe
                var existingExpediente = _DBContext.Expedientes
                    .FirstOrDefault(e => e.AlumnoId == vm.Alumno.AlumnoId);

                if (existingExpediente != null)
                {
                    // Actualizar campos
                    existingExpediente.ActaNacimiento = vm.ActaNacimiento;
                    existingExpediente.Curp = vm.Curp;
                    existingExpediente.ComprobanteDomicilio = vm.ComprobanteDomicilio;
                    existingExpediente.Ine = vm.Ine;
                    existingExpediente.CertificadoMedico = vm.CertificadoMedico;
                    existingExpediente.ReciboPago = vm.ReciboPago;
                    existingExpediente.Fotografias = vm.Fotografias;
                    existingExpediente.DocumentosCompletos = vm.DocumentosCompletos;

                    _DBContext.Expedientes.Update(existingExpediente);
                }
                else
                {
                    // Si no tiene expediente aún, créalo
                    var newExpediente = new Expediente
                    {
                        AlumnoId = vm.Alumno.AlumnoId,
                        ActaNacimiento = vm.ActaNacimiento,
                        Curp = vm.Curp,
                        ComprobanteDomicilio = vm.ComprobanteDomicilio,
                        Ine = vm.Ine,
                        CertificadoMedico = vm.CertificadoMedico,
                        ReciboPago = vm.ReciboPago,
                        Fotografias = vm.Fotografias,
                        DocumentosCompletos = vm.DocumentosCompletos
                    };
                    _DBContext.Expedientes.Add(newExpediente);
                }
            }

            _DBContext.SaveChanges();
            TempData["SuccessMessage"] = "✅ Alumno guardado exitosamente.";
            return RedirectToAction("Students");
        }

        // Eliminar alumno (sin cambios)
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

            _DBContext.ProgresoEstudiantils.RemoveRange(alumno.ProgresoEstudiantils);
            if (alumno.Expediente != null)
            {
                _DBContext.Expedientes.Remove(alumno.Expediente);
            }

            _DBContext.Alumnos.Remove(alumno);
            _DBContext.SaveChanges();

            return RedirectToAction("Students");
        }

        [HttpGet]
        public IActionResult AddToGroup(int id)
        {
            var alumno = _DBContext.Alumnos.Find(id);
            if (alumno == null)
            {
                return NotFound();
            }

            var vm = new StudentGroupVM
            {
                AlumnoId = alumno.AlumnoId,
                NombreAlumno = $"{alumno.Nombre} {alumno.ApellidoPaterno} {alumno.ApellidoMaterno}"
            };

            // Cargar solo los grupos "En curso"
            vm.GruposDisponibles = _DBContext.Grupos
                .Where(g => g.Estado == "En curso")
                .Select(g => new SelectListItem
                {
                    Value = g.GrupoId.ToString(),
                    Text = $"{g.NombreGrupo} - {g.Taller.NombreTaller} ({g.Horario})"
                }).ToList();

            return View(vm);
        }

        [HttpPost]
        public IActionResult Confirm_AddToGroup(StudentGroupVM vm)
        {
            if (!ModelState.IsValid)
            {
                // Recargar grupos si hay error
                vm.GruposDisponibles = _DBContext.Grupos
                    .Where(g => g.Estado == "En curso")
                    .Select(g => new SelectListItem
                    {
                        Value = g.GrupoId.ToString(),
                        Text = $"{g.NombreGrupo} - {g.Taller.NombreTaller} ({g.Horario})"
                    }).ToList();

                return View("AddToGroup", vm);
            }

            // Verificar si ya está inscrito en ese grupo
            bool existeProgreso = _DBContext.ProgresoEstudiantils
                .Any(p => p.AlumnoId == vm.AlumnoId && p.GrupoId == vm.GrupoId);

            if (existeProgreso)
            {
                TempData["ErrorMessage"] = "⚠️ El alumno ya está inscrito en este grupo.";
                return RedirectToAction("AddToGroup", new { id = vm.AlumnoId });
            }

            // Crear nuevo progreso estudiantil
            var progreso = new ProgresoEstudiantil
            {
                AlumnoId = vm.AlumnoId,
                GrupoId = vm.GrupoId,
                Estado = "Inscrito",
                Calificacion = null,
                Asistencia = null
            };

            _DBContext.ProgresoEstudiantils.Add(progreso);
            _DBContext.SaveChanges();

            TempData["SuccessMessage"] = "✅ Alumno inscrito exitosamente en el grupo.";

            return RedirectToAction("Details", new { id = vm.AlumnoId });
        }
    }
}