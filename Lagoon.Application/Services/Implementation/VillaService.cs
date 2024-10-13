using Lagoon.Application.Common.Interfaces;
using Lagoon.Application.Services.Interface;
using Lagoon.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;


namespace Lagoon.Application.Services.Implementation
{
    public class VillaService : IVillaService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public VillaService(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
           _webHostEnvironment = webHostEnvironment;
        }
        public void CreateVilla(Villa villa)
        {
            //image uploading
            if (villa.Image != null)
            {
                //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                //set the path in to wwwroot folder
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                }
                villa.ImageUrl = @"\images\VillaImages\" + fileName;
            }
            else
            {
                villa.ImageUrl = "https://placehold.co/600x400";
            }

            _unitOfWork.Villa.Add(villa);
            _unitOfWork.Villa.Save();
        }

        public bool DeleteVilla(int id)
        {
            try
            {
                Villa? objFromDb = _unitOfWork.Villa.Get(u => u.id == id);
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

                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }

           
        }

        public IEnumerable<Villa> GetAllVillas()
        {
            return _unitOfWork.Villa.GetAll();
        }

        public Villa GetVillaById(int id)
        {
            return _unitOfWork.Villa.Get(u=>u.id == id);
        }

        public void UpdateVilla(Villa villa)
        {
            if (villa.Image != null)
            {
                //chage the name using guid and using GetExtension keep the imge type as it is (png/jpg etc). 
                string fileName = Guid.NewGuid().ToString() + Path.GetExtension(villa.Image.FileName);
                //set the path in to wwwroot folder
                string imagePath = Path.Combine(_webHostEnvironment.WebRootPath, @"images\VillaImages");

                if (!string.IsNullOrEmpty(villa.ImageUrl))
                {
                    var oldImagePath = Path.Combine(_webHostEnvironment.WebRootPath, villa.ImageUrl.TrimStart('\\'));

                    if (System.IO.File.Exists(oldImagePath))
                    {
                        System.IO.File.Delete(oldImagePath);
                    }
                }

                using (var fileStream = new FileStream(Path.Combine(imagePath, fileName), FileMode.Create))
                {
                    villa.Image.CopyTo(fileStream);
                }
                villa.ImageUrl = @"\images\VillaImages\" + fileName;
            }

            _unitOfWork.Villa.Update(villa);
            _unitOfWork.Villa.Save();
        }
    }
}
