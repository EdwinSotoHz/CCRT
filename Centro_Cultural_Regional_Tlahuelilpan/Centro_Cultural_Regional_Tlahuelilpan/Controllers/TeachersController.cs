using Microsoft.AspNetCore.Mvc;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;


using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Security.Cryptography;
using System.Text;
using System.Diagnostics;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class TeachersController : Controller
    {
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;

        public TeachersController(CentroCulturalRegionalTlahuelilpanContext context)
        {
            _DBContext = context;
        }

        public IActionResult Teachers()
        {
            List<Usuario> list = _DBContext.Usuarios.Include(u => u.Rol).Include(u => u.Docente).ToList();
            return View(list);
        }

        [HttpGet]
        public IActionResult CreateEditTeacherUser(int UsuarioId)
        {
            TeachersUsersVM vm = new TeachersUsersVM()
            {
                objUsuario = new Usuario(),
                objDocente = new Docente(),
                lstRoles = _DBContext.Roles.Select(r => new SelectListItem
                {
                    Text = r.Nombre,
                    Value = r.RolId.ToString()
                }).ToList()
            };

            if (UsuarioId != 0)
            {
                vm.objUsuario = _DBContext.Usuarios.Include(u => u.Docente).FirstOrDefault(u => u.UsuarioId == UsuarioId);

                if (vm.objUsuario != null)
                {
                    vm.objDocente = vm.objUsuario.Docente;
                    //vm.objUsuario.Contraseña = "";
                }
            }

            return View(vm);
        }

        [HttpPost]
        public IActionResult Confirm_CreateEditTeacherUser(TeachersUsersVM vm)
        {
            
            if (!string.IsNullOrEmpty(vm.objUsuario.Contraseña))
            {
                if (vm.objUsuario.Contraseña != vm.ConfirmarClave)
                {
                    Debug.WriteLine("\n\n Las contraseñas no coinciden \n\n");
                    vm.lstRoles = _DBContext.Roles.Select(r => new SelectListItem
                    {
                        Text = r.Nombre,
                        Value = r.RolId.ToString()
                    }).ToList();
                    return View("CreateEditTeacherUser", vm);
                }

                // Hash de la contraseña
                vm.objUsuario.Contraseña = Convert.ToHexString(
                    SHA256.HashData(Encoding.UTF8.GetBytes(vm.objUsuario.Contraseña)));
            }

            if (vm.objUsuario.UsuarioId == 0)
            {
                // Crear nuevo docente
                _DBContext.Docentes.Add(vm.objDocente);
                _DBContext.SaveChanges();

                // Asignar docente al usuario
                vm.objUsuario.DocenteId = vm.objDocente.DocenteId;
                _DBContext.Usuarios.Add(vm.objUsuario);
            }
            else
            {
                // Actualizar docente
                _DBContext.Docentes.Update(vm.objDocente);

                // Actualizar usuario (excepto contraseña si está vacía)
                var existingUser = _DBContext.Usuarios.Find(vm.objUsuario.UsuarioId);
                if (existingUser != null)
                {
                    existingUser.NombreUsuario = vm.objUsuario.NombreUsuario;
                    existingUser.RolId = vm.objUsuario.RolId;

                    if (!string.IsNullOrEmpty(vm.objUsuario.Contraseña))
                    {
                        existingUser.Contraseña = vm.objUsuario.Contraseña;
                    }
                }
            }

            _DBContext.SaveChanges();
            return RedirectToAction("Teachers");
        }


        [HttpGet]
        public IActionResult DeleteTeacher(int UsuarioId)
        {
            var usuario = _DBContext.Usuarios
                .Include(u => u.Docente)
                .FirstOrDefault(u => u.UsuarioId == UsuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            return View(usuario);
        }

        [HttpPost]
        public IActionResult Confirm_DeleteTeacher(int UsuarioId)
        {
            var usuario = _DBContext.Usuarios
                .Include(u => u.Docente)
                .FirstOrDefault(u => u.UsuarioId == UsuarioId);

            if (usuario == null)
            {
                return NotFound();
            }

            // Verificar si el docente tiene grupos asignados
            bool tieneGrupos = _DBContext.Grupos.Any(g => g.DocenteId == usuario.DocenteId);

            if (tieneGrupos)
            {
                TempData["ErrorMessage"] = "No se puede eliminar el docente porque tiene grupos asignados";
                return RedirectToAction("Teachers");
            }

            // Eliminar usuario y docente
            _DBContext.Usuarios.Remove(usuario);
            _DBContext.Docentes.Remove(usuario.Docente);
            _DBContext.SaveChanges();

            return RedirectToAction("Teachers");
        }
    }
}
