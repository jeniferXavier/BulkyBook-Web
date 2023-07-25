using BulkyBook.DataAccess.Repository.IRepository;
using BulkyBook.Models.Models;
using BulkyBook.Models.ViewModels;
using BulkyBook.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyBookWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class CompanyController:Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            IEnumerable<Company> objCompanylist = _unitOfWork.Company.GetAll();
            return View(objCompanylist);
        }


        public IActionResult Upsert(int? id)
        {
            if (id == 0 || id == null)
            {
                return View(new Company());
            }
            else
            {
              Company company = _unitOfWork.Company.Get(u => u.Id == id);
                return View(company);
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(Company company)
        {
            if (ModelState.IsValid)
            {
                if (company.Id == 0)
                {
                    _unitOfWork.Company.Add(company);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Created Successfully";
                }
                else
                {
                    _unitOfWork.Company.Update(company);
                    _unitOfWork.Save();
                    TempData["success"] = "Company Updated Successfully";
                }

                return RedirectToAction("Index");
            }
            else
            {
                return View(company);
            }
        }

        #region Api Call

        [HttpGet]
        public IActionResult GetAll()
        {
            var objCompanylist = _unitOfWork.Company.GetAll();
            return Json(new { data = objCompanylist });
        }

        [HttpDelete]
        public IActionResult Delete(int? id)
        {
            var companyToBeDeleted = _unitOfWork.Company.Get(u => u.Id == id);
            if (companyToBeDeleted == null)
            {
                return Json(new { success = false, message = "Error While Deleting" });
            }
            
            _unitOfWork.Company.Remove(companyToBeDeleted);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Company Deleted Successfully" });
        }

        #endregion
    }
}
