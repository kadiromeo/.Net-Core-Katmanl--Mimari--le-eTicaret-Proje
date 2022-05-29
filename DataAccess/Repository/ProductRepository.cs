using Core_eTicaret.DataAccess.Data;
using DataAccess.Repository.IRepository;
using Microsoft.EntityFrameworkCore;
using Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Repository
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ApplicationDbContext _db;
        public ProductRepository(ApplicationDbContext db):base(db)
        {
            _db = db;
        }

        public void Update(Product product)
        {
            var nesne =_db.Products.FirstOrDefault(i=>i.Id == product.Id);
            if (nesne!=null)
            {
                if (product.ImageUrl!=null)
                {
                    nesne.ImageUrl = product.ImageUrl;

                }
                nesne.Price = product.Price;
                nesne.Quantity = product.Quantity;
                nesne.Title = product.Title;
                nesne.Description = product.Description;  
                nesne.CategoryId = product.CategoryId;  

            }

        }

        public List<Product> GetProductByCategory(int id)
        {
            var product = _db.Products.AsQueryable();
            if (id!=null)
            {
                product = product.Include(i => i.Category).Where(a => a.CategoryId == id);
            }
            return product.ToList();
        }
    }
}
