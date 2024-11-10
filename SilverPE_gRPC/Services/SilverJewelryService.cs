using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SilverPE_gRPC.Protos;
using SilverPE_Repository.Interfaces;

namespace SilverPE_gRPC.Services
{
    public class SilverJewelryService : SilverJewelryProtos.SilverJewelryProtosBase
    {
        private readonly IJewelryRepository _jewelryRepository;

        public SilverJewelryService(IJewelryRepository jewelryRepository)
        {
            _jewelryRepository = jewelryRepository;
        }

        [Authorize(Roles = "1")]
        public async override Task<GetAllJewelryResponse> GetAllJewelry(GetAllJewelryRequest request, ServerCallContext context)
        {
            var jewelries = await _jewelryRepository.GetJewelries();

            var response = new GetAllJewelryResponse();
            response.Jewelries.AddRange(jewelries.Select(jewelry => new SilverJewelry
            {
                SilverJewelryId = jewelry.SilverJewelryId,
                SilverJewelryName = jewelry.SilverJewelryName,
                SilverJewelryDescription = jewelry.SilverJewelryDescription ?? string.Empty,
                MetalWeight = (double)(jewelry.MetalWeight ?? 0),
                Price = (double)(jewelry.Price ?? 0),
                ProductionYear = jewelry.ProductionYear ?? 0,
                CreatedDate = jewelry.CreatedDate?.ToString("o") ?? string.Empty, // ISO 8601 format
                CategoryId = jewelry.CategoryId ?? string.Empty
            }));

            return response;
        }
    }
}
