using SilverPE_BOs.Models;
using SilverPE_DAO;
using SilverPE_Repository.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<List<Category>> GetCategories()
            => await CategoryDAO.Instance.GetCategories(); 

        public async Task<Category> GetCategory(string id)
            => await CategoryDAO.Instance.GetCategory(id);
    }
}
