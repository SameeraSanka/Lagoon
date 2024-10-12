using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Lagoon.Infrastructure.Data;
using Lagoon.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IVillaRepository _villaRepository;
        public VillaController(IVillaRepository villaRepository)
        {
           _villaRepository = villaRepository;
        }
        public IActionResult Index()
        {
            var villas = _villaRepository.GetAll();
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
                _villaRepository.Add(obj);
                _villaRepository.Save();
                return RedirectToAction("Index");
            }
           return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _villaRepository.Get(u => u.id == villaId);
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
                _villaRepository.Update(obj);
                _villaRepository.Save();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _villaRepository.Get(u => u.id == villaId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _villaRepository.Get(u => u.id == obj.id);
            if (objFromDb is not null)
            {
                _villaRepository.Remove(objFromDb);
                _villaRepository.Save();
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
