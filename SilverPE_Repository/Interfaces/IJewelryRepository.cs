using SilverPE_BOs.Models;
using SilverPE_DAO;
using SilverPE_Repository.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SilverPE_Repository.Interfaces
{
    public interface IJewelryRepository
    {
        public Task<List<SilverJewelryDTO>> GetJewelries();
        public Task<bool> AddJewelry(CreateSilverJewerlryRequest silverJewelry);
        public Task<bool> UpdateJewelry(string id, UpdateSilverJewerlyRequest silverJewelry);
        public Task<bool> DeleteJewelry(string id);
        public Task<List<SilverJewelryDTO>> SearchByNameOrWeight(string searchValue);
        public Task<SilverJewelry> GetSilverJewelryById(string id);
    }
}
