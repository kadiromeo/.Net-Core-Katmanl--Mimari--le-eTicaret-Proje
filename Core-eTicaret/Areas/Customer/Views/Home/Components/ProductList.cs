using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataAccess.Repository.IRepository;

namespace Core_eTicaret.Areas.Customer.Views.Home.Components
{
    public class ProductList:ViewComponent
    {
        private readonly IUnitOfWork _unitOfWork;
        public ProductList(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IViewComponentResult Invoke(int? categoryId)
        {
            if (categoryId.HasValue)
            {
                return View(_unitOfWork.Product.GetProductByCategory((int)categoryId));
            }
            var product = _unitOfWork.Product.GetAll();
            return View(product);
        }
    }
}
