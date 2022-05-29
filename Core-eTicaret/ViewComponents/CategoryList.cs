using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repository.IRepository;

namespace Core_eTicaret.ViewComponents
{
    public class CategoryList:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryList(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke()
        {
            var category = _unitOfWork.Category.GetAll();
            return View(category);
        }
    }
}
