using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using SilverPE_DAO;
using SilverPE_gRPC.Protos;
using SilverPE_Repository.Interfaces;
using System.Text.RegularExpressions;

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

        [Authorize(Roles = "1")]
        public override async Task<CreateSilverJewelryResponse> CreateSilverJewelry(CreateSilverJewelryRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.SilverJewelryId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "SilverJewelryId is required"));
            }

            var namePattern = @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$";
            if (!Regex.IsMatch(request.SilverJewelryName, namePattern))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "SilverJewelryName is invalid. Each word must start with a capital letter."));
            }

            if (request.MetalWeight == null || request.Price == null || request.ProductionYear == null || request.CategoryId == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "All fields must be provided."));
            }

            if (request.Price < 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Price must be greater than or equal to 0."));
            }

            if (request.ProductionYear < 1900)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ProductionYear must be greater than or equal to 1900."));
            }

            var jewelryToAdd = new SilverPE_Repository.Request.CreateSilverJewerlryRequest
            {
                SilverJewelryId = request.SilverJewelryId,
                SilverJewelryName = request.SilverJewelryName,
                SilverJewelryDescription = request.SilverJewelryDescription,
                MetalWeight = (decimal?)request.MetalWeight,
                Price = (decimal?)request.Price,
                ProductionYear = request.ProductionYear,
                CategoryId = request.CategoryId
            };

            var success = await _jewelryRepository.AddJewelry(jewelryToAdd);

            return new CreateSilverJewelryResponse
            {
                Success = success
            };
        }

        [Authorize(Roles = "1, 2")]
        public override async Task<SearchResponse> SearchByNameOrWeight(SearchRequest request, ServerCallContext context)
        {
            var jewelries = await _jewelryRepository.SearchByNameOrWeight(request.SearchValue ?? "");

            if (jewelries == null || jewelries.Count == 0)
            {
                return new SearchResponse
                {
                    Success = false,
                    Message = "No jewelry found matching the search criteria.",
                    Jewelries = { }
                };
            }

            var jewelryList = new List<SilverJewelry>();
            foreach (var jewelry in jewelries)
            {
                jewelryList.Add(new SilverJewelry
                {
                    SilverJewelryId = jewelry.SilverJewelryId,
                    SilverJewelryName = jewelry.SilverJewelryName,
                    SilverJewelryDescription = jewelry.SilverJewelryDescription,
                    MetalWeight = (double)jewelry.MetalWeight,
                    Price = (double)jewelry.Price,
                    ProductionYear = jewelry.ProductionYear ?? 1900,
                    CategoryId = jewelry.CategoryId
                });
            }

            return new SearchResponse
            {
                Success = true,
                Message = "Search successful",
                Jewelries = { jewelryList }
            };
        }

        [Authorize(Roles = "1")]
        public override async Task<UpdateSilverJewelryResponse> UpdateSilverJewelry(UpdateSilverJewelryRequest request, ServerCallContext context)
        {
            if (string.IsNullOrWhiteSpace(request.Id))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "SilverJewelryId is required"));
            }

            var namePattern = @"^[A-Z][a-zA-Z0-9]*(\s[A-Z][a-zA-Z0-9]*)*$";
            if (!Regex.IsMatch(request.SilverJewelryName, namePattern))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "SilverJewelryName is invalid. Each word must start with a capital letter."));
            }

            if (request.MetalWeight == null || request.Price == null || request.ProductionYear == null || request.CategoryId == null)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "All fields must be provided."));
            }

            if (request.Price < 0)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Price must be greater than or equal to 0."));
            }

            if (request.ProductionYear < 1900)
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "ProductionYear must be greater than or equal to 1900."));
            }

            var response = await _jewelryRepository.UpdateJewelry(request.Id, new SilverPE_Repository.Request.UpdateSilverJewerlyRequest
            {
                SilverJewelryName = request.SilverJewelryName,
                SilverJewelryDescription = request.SilverJewelryDescription,
                MetalWeight = (decimal?)request.MetalWeight,
                Price = (decimal?)request.Price,
                ProductionYear = request.ProductionYear,
                CategoryId = request.CategoryId
            });

            if (response)
            {
                return new UpdateSilverJewelryResponse
                {
                    Success = true,
                    Message = "Jewelry updated successfully"
                };
            }

            return new UpdateSilverJewelryResponse
            {
                Success = false,
                Message = "Failed to update jewelry"
            };
        }

        [Authorize(Roles = "1")]
        public override async Task<DeleteSilverJewelryResponse> DeleteSilverJewelry(DeleteSilverJewelryRequest request, ServerCallContext context)
        {
            var response = await _jewelryRepository.DeleteJewelry(request.Id);

            if (response)
            {
                return new DeleteSilverJewelryResponse
                {
                    Success = true,
                    Message = "Jewelry deleted successfully"
                };
            }

            return new DeleteSilverJewelryResponse
            {
                Success = false,
                Message = "Failed to delete jewelry"
            };
        }
    }
}
