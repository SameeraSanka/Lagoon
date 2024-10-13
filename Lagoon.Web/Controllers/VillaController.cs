using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interface;
using Lagoon.Domain.Entities;
using Lagoon.Infrastructure.Data;
using Lagoon.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaService _villaService;
        public VillaController(IVillaService villaService)
        {
            _villaService = villaService;
        }

        public IActionResult Index()
        {
            var villas = _villaService.GetAllVillas();
            return View(villas);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Villa obj)
        {
            if (obj.Name == obj.Description)
            {
                ModelState.AddModelError("Description", "The Description can not exactly match the Name");
            }
            if (ModelState.IsValid)
            {
                _villaService.CreateVilla(obj);
                TempData["success"] = "The villa has been Created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa has been not created";
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);
            //we can use the find as well insterd of using above
            //Villa? obj = _db.Villas.Find(villaId);

            //we can add conditions like this to retirve data base on some conditions
            //var villaList = _db.Villas.Where(u => u.Price > 50 && u.Occupancy > 0);

            if (obj == null)
            {
                return RedirectToAction("Error","Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Update(Villa obj)
        {
            
            if (ModelState.IsValid && obj.id>0)
            {
                _villaService.UpdateVilla(obj);
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa has been not updated";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaService.GetVillaById(villaId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            bool deleted = _villaService.DeleteVilla(obj.id);
            if (deleted)
            {
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            else
            {
                TempData["error"] = "Faild to deletethe villa ";
            }
   
            return View();
        }
    }
}
