using Microsoft.EntityFrameworkCore;
using SilverPE_BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_DAO
{
    public class CategoryDAO
    {
        private SilverJewelry2023DbContext _context;
        private static CategoryDAO _instance;

        public static CategoryDAO Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new CategoryDAO();
                }
                return _instance;
            }

        }

        public CategoryDAO()
        {
            _context = new SilverJewelry2023DbContext();
        }

        public async Task<Category> GetCategory(string categoryId)
            => await _context.Categories.SingleOrDefaultAsync(c => c.CategoryId.Equals(categoryId));

        public async Task<List<Category>> GetCategories()
            => await _context.Categories.ToListAsync();
    }
}
