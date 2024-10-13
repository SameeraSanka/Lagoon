using Lagoon.Application.Common.Interfaces;
using Lagoon.Domain.Entities;
using Lagoon.Infrastructure.Data;
using Lagoon.Infrastructure.Repository;
using Microsoft.AspNetCore.Mvc;

namespace Lagoon.Web.Controllers
{
    public class VillaController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            var villas = _unitOfWork.Villa.GetAll();
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
                //image uploading
                if (obj.Image != null)
                {
                    //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    //set the path in to wwwroot folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }
                else
                {
                    obj.ImageUrl = "https://placehold.co/600x400";
                }

                _unitOfWork.Villa.Add(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been Created successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa has been not created";
            return View();
        }

        public IActionResult Update(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.id == villaId);
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
                if (obj.Image != null)
                {
                    //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(obj.Image.FileName);
                    //set the path in to wwwroot folder
                    string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                    if (!string.IsNullOrEmpty(obj.ImageUrl))
                    {
                        var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, obj.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                    {
                        obj.Image.CopyTo(fileStream);
                    }
                    obj.ImageUrl = @"\images\VillaImages\" + fileName;
                }

                _unitOfWork.Villa.Update(obj);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been updated successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The villa has been not updated";
            return View();
        }

        public IActionResult Delete(int villaId)
        {
            Villa? obj = _unitOfWork.Villa.Get(u => u.id == villaId);

            if (obj == null)
            {
                return RedirectToAction("Error", "Home");
            }
            return View(obj);
        }

        [HttpPost]
        public IActionResult Delete(Villa obj)
        {
            Villa? objFromDb = _unitOfWork.Villa.Get(u => u.id == obj.id);
            if (objFromDb is not null)
            {
                //delete the image form the file
                if (!string.IsNullOrEmpty(objFromDb.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, objFromDb.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                _unitOfWork.Villa.Remove(objFromDb);
                _unitOfWork.Villa.Save();
                TempData["success"] = "The villa has been deleted successfully";
                return RedirectToAction("Index");
            }
            TempData["error"] = "The Villa can not be deleted";
            return View();
        }
    }
}
