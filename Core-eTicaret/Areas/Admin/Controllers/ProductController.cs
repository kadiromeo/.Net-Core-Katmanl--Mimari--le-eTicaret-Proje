using DataAccess.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Models;
using Models.ViewModels;
using Others;
using System;
using System.IO;
using System.Linq;

namespace Core_eTicaret.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles =SD.Role_Admin)]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        IWebHostEnvironment _hostEnvironment;

        public ProductController(IUnitOfWork unitOfWork,IWebHostEnvironment hostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _hostEnvironment = hostEnvironment; 
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new ProductVM()
            {
                Product = new Product(),
                CategoryList=_unitOfWork.Category.GetAll().Select(i=>new SelectListItem
                {
                    Text=i.Name,
                    Value=i.Id.ToString()
                })

            };
            if (id==null)
            {
                return View(productVM);
            }
            productVM.Product = _unitOfWork.Product.Get(id.GetValueOrDefault());
            if (productVM.Product==null)
            {
                return NotFound();
            }
            return View(productVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM)
        {
            if (ModelState.IsValid)
            {
                string webRootPath = _hostEnvironment.WebRootPath;
                var files = HttpContext.Request.Form.Files;
                if (files.Count>0)
                {
                    string filename = Guid.NewGuid().ToString();
                    var upload=Path.Combine(webRootPath,@"image\product");
                    var extention = Path.GetExtension(files[0].FileName);
                    if (productVM.Product.ImageUrl!=null)
                    {
                        var imagePath = Path.Combine(webRootPath, productVM.Product.ImageUrl.TrimStart('\\'));
                        if (System.IO.File.Exists(imagePath))
                        {
                            System.IO.File.Exists(imagePath);
                        }
                    }
                    using (var filesStreams=new FileStream(Path.Combine(upload, filename + extention), FileMode.Create))
                    {
                        files[0].CopyTo(filesStreams);
                    }
                    productVM.Product.ImageUrl = @"\image\product\" + filename + extention;

                }
                else
                {
                    if (productVM.Product.Id!=0)
                    {
                        var nesne = _unitOfWork.Product.Get(productVM.Product.Id);
                        productVM.Product.ImageUrl=nesne.ImageUrl;
                    }
                }

                if (productVM.Product.Id==0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                _unitOfWork.Save();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(i => new SelectListItem
                {
                    Text = i.Name,
                    Value = i.Id.ToString()
                });
                if (productVM.Product.Id!=0)
                {
                    productVM.Product = _unitOfWork.Product.Get(productVM.Product.Id);
                }
            }
            return View(productVM.Product);
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var nesne = _unitOfWork.Product.Get(id);
            if (nesne==null)
            {
                return Json(new { success = false, message = "Silme İşlemi Başarısız" });
            }
            string webRootPath = _hostEnvironment.WebRootPath;
            var imagePath = Path.Combine(webRootPath, nesne.ImageUrl.TrimStart('\\'));
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Exists(imagePath);
            }
            _unitOfWork.Product.Remove(nesne);
            _unitOfWork.Save();
            return Json(new { success = true, message = "Silme İşlemi Başarılı" });
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var nesne=_unitOfWork.Product.GetAll(includeProperties:"Category");

            return Json(new {data=nesne});
        }
    }
}
