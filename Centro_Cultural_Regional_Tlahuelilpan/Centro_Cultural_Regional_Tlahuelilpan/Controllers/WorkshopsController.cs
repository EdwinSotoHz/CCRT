using Microsoft.AspNetCore.Mvc;
using Centro_Cultural_Regional_Tlahuelilpan.Models.DBCRUDCORE;
using Centro_Cultural_Regional_Tlahuelilpan.Models.ViewModels;


using Microsoft.EntityFrameworkCore;

namespace Centro_Cultural_Regional_Tlahuelilpan.Controllers
{
    public class WorkshopsController : Controller
    {
        private readonly CentroCulturalRegionalTlahuelilpanContext _DBContext;
        private readonly IWebHostEnvironment _hostingEnvironment;

        public WorkshopsController(CentroCulturalRegionalTlahuelilpanContext context, IWebHostEnvironment hostingEnvironment)
        {
            _DBContext = context;
            _hostingEnvironment = hostingEnvironment;
        }

        public IActionResult Workshops()
        {
            List<Tallere> list = _DBContext.Talleres.ToList();
            return View(list);
        }

        /********************************************** CREATE - EDIT **********************************************/
        [HttpGet]
        public IActionResult CreateEditWorkshop(int TallerId)
        {
            
            WorkshopVM vm = new WorkshopVM()
            {
                objTaller = new Tallere()
            };
        
            if (TallerId != 0) {
                vm.objTaller = _DBContext.Talleres.Find(TallerId);
            }
            
            return View(vm);
        }

        [HttpPost]
        public async Task<IActionResult> Confirm_CreateEditWorkshop(WorkshopVM vm)
        {
            if (ModelState.IsValid)
            {
                /*************** Para manejar la imagen ***************/
                if (vm.ImagenArchivo != null && vm.ImagenArchivo.Length > 0)
                {
                    // Contar cuántos talleres (excluyendo el actual) usan esta imagen
                    bool imagenUsadaPorOtros = imagenUsadaPorOtros = await _DBContext.Talleres
                        .Where(t => t.UrlImagen == vm.objTaller.UrlImagen &&
                                   (vm.objTaller.TallerId == 0 || t.TallerId != vm.objTaller.TallerId))
                        .AnyAsync();

                    // Eliminar imagen anterior pero solo si existe
                    // Eliminar imagen anterior solo si no está siendo usada por otros talleres
                    if (!string.IsNullOrEmpty(vm.objTaller.UrlImagen) && !imagenUsadaPorOtros)
                    {
                        var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "src", "img", vm.objTaller.UrlImagen);

                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Delete(imagePath);
                        }
                    }

                    // Guardar imagen (la nueva imagen)
                    var uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, "src", "img");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    //var uniqueFileName = Guid.NewGuid().ToString() + "_" + vm.ImagenArchivo.FileName;
                    var fileName = Path.GetFileName(vm.ImagenArchivo.FileName);
                    var filePath = Path.Combine(uploadsFolder, fileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await vm.ImagenArchivo.CopyToAsync(fileStream);
                    }

                    vm.objTaller.UrlImagen = $"{fileName}";
                }

                if (vm.objTaller.TallerId == 0)
                {
                    _DBContext.Talleres.Add(vm.objTaller);
                }
                else
                {
                    _DBContext.Talleres.Update(vm.objTaller);
                }

                await _DBContext.SaveChangesAsync();
                return RedirectToAction("Workshops", "Workshops");
            }

            return RedirectToAction("Workshops", "Workshops");
        }

        /********************************************** DELETE **********************************************/
        [HttpGet]
        public IActionResult DeleteWorkshop(int TallerId)
        {
            Tallere taller = _DBContext.Talleres.Find(TallerId);
            if (taller == null)
            {
                return RedirectToAction("Workshops", "Workshops");
            }
            return View(taller);
        }

        [HttpPost]
        public IActionResult Confirm_DeleteWorkshop(int TallerId)
        {
            var taller = _DBContext.Talleres.Find(TallerId);
            if (taller == null)
            {
                return RedirectToAction("Workshops", "Workshops");
            }

            // Si existe la imagen 
            if (!string.IsNullOrEmpty(taller.UrlImagen))
            {
                // Verificar si otros talleres usan la misma imagen (versión síncrona)
                bool imagenUsadaPorOtros = _DBContext.Talleres
                    .Any(t => t.UrlImagen == taller.UrlImagen && t.TallerId != TallerId);

                // Solo eliminamos la imagen si no está siendo usada por otros talleres
                if (!imagenUsadaPorOtros)
                {
                    var imagePath = Path.Combine(_hostingEnvironment.WebRootPath, "src", "img", taller.UrlImagen);

                    //Solo borrar imagen si existe
                    if (System.IO.File.Exists(imagePath))
                    {
                        System.IO.File.Delete(imagePath);
                    }
                }
            }

            _DBContext.Talleres.Remove(taller);
            _DBContext.SaveChanges();

            return RedirectToAction("Workshops", "Workshops");
        }
    }
}
