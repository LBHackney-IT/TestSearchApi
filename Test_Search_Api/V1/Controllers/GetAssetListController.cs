using Amazon.Lambda.Core;
using Hackney.Core.Logging;
using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.V1.Boundary.Response.Metadata;
using Test_Search_Api.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Test_Search_Api.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/assets")]
    [ApiController]
    public class GetAssetListController : BaseController
    {
        private readonly IGetAssetListUseCase _getAssetListUseCase;
        private readonly IGetAssetListSetsUseCase _getAssetListSetsUseCase;

        public GetAssetListController(IGetAssetListUseCase getAssetListUseCase, IGetAssetListSetsUseCase getAssetListSetsUseCase)
        {
            _getAssetListUseCase = getAssetListUseCase;
            _getAssetListSetsUseCase = getAssetListSetsUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAssetList([FromQuery] GetAssetListRequest request)
        {
            try
            {
                var assetsSearchResult = await _getAssetListUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIResponse<GetAssetListResponse>(assetsSearchResult);
                apiResponse.Total = assetsSearchResult.Total();

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }

        [ProducesResponseType(typeof(APIResponse<GetAllAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        [ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)]
        [Route("all")]
        [HttpGet, MapToApiVersion("1")]
        [LogCall(LogLevel.Information)]
        public async Task<IActionResult> GetAllAssetList([FromQuery] GetAllAssetListRequest request)
        {
            try
            {
                var assetsSearchResult = await _getAssetListSetsUseCase.ExecuteAsync(request).ConfigureAwait(false);
                var apiResponse = new APIAllResponse<GetAllAssetListResponse>(assetsSearchResult)
                {
                    Total = assetsSearchResult.Total(),
                    LastHitId = assetsSearchResult.LastHitId()
                };

                return new OkObjectResult(apiResponse);
            }
            catch (Exception e)
            {
                LambdaLogger.Log(e.Message + e.StackTrace);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
