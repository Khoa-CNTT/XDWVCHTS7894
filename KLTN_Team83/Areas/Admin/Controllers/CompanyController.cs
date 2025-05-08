using KLTN_Team83.DataAccess.Repository.IRepository;
using KLTN_Team83.Models;
using KLTN_Team83.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace KLTN_Team83.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _db;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CompanyController(IUnitOfWork db, IWebHostEnvironment hostEnvironment)
        {
            _db = db;
            _hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _db.Company.GetAll().ToList();

            return View(objCompanyList);
        }

        // CHỨC NĂNG THÊM PRODUCT
        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                //create
                return View(new Company());
            }
            else
            {
                //update
                Company objCompany = _db.Company.Get(u => u.Id_Company == id);
                return View(objCompany);
            }
        }
        [HttpPost]
        public IActionResult Upsert(Company objCompany, IFormFile? file)
        {

            if (ModelState.IsValid)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string uploads = Path.Combine(wwwRootPath, @"images\company");
                    using (var fileStreams = new FileStream(Path.Combine(uploads, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStreams);
                    }
                    if (!string.IsNullOrEmpty(objCompany.Logo))
                    {
                        //delete old image
                        var oldImagePath =
                            Path.Combine(wwwRootPath, objCompany.Logo.TrimStart('\\'));
                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                    objCompany.Logo = @"\images\company\" + fileName;
                }
                if (objCompany.Id_Company == 0)
                {
                    _db.Company.Add(objCompany);
                }
                else
                {
                    _db.Company.Update(objCompany);
                }
                _db.Save();
                TempData["success"] = "Company created successfully!";
                return RedirectToAction("Index");
            }
            else
            {
                return View(objCompany);
            }
        }


        #region
        [HttpGet]
        public IActionResult GetAll()
        {
            List<Company> objCompanyList = _db.Company.GetAll().ToList();
            return Json(new { data = objCompanyList });
        }

        // CHỨC NĂNG XÓA PRODUCT
        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDelete = _db.Company.Get(u => u.Id_Company == id);
            if (companyToBeDelete == null)
            {
                return Json(new { success = false, message = "Error while Delete" });
            }
            //xóa ảnh cũ
            var oldImagePath =
                            Path.Combine(_hostEnvironment.WebRootPath,
                            companyToBeDelete.Logo.TrimStart('\\'));
            if (System.IO.File.Exists(oldImagePath))
            {
                System.IO.File.Delete(oldImagePath);
            }

            _db.Company.Remove(companyToBeDelete);
            _db.Save();

            List<Company> objCompanyList = _db.Company.GetAll().ToList();
            return Json(new { success = true, message = "Delete Successful" });
        }
        #endregion
    }
}
