using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.V1.UseCase.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Test_Search_Api.V1.Controllers;
using HousingSearchApi.V1.Boundary.Responses.Metadata;
using Amazon.SimpleNotificationService.Model;

namespace Test_Search_Api.Controllers
{
    [ApiVersion("1")]
    [Produces("application/json")]
    [Route("api/v1/search/assets")]
    [ApiController]
    public class GetAssetListController : BaseController
    {
        private readonly IGetAssetListUseCase _getAssetListUseCase;

        public GetAssetListController(IGetAssetListUseCase getAssetListUseCase)
        {
            _getAssetListUseCase = getAssetListUseCase;
        }

        [ProducesResponseType(typeof(APIResponse<GetAssetListResponse>), 200)]
        [ProducesResponseType(typeof(APIResponse<NotFoundException>), 404)]
        //[ProducesResponseType(typeof(APIResponse<BadRequestException>), 400)] TODO: HOW TO SOLVE THIS?
        [HttpGet, MapToApiVersion("1")]
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
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
