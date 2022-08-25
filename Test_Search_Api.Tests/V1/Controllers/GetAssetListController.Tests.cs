using Test_Search_Api.V1.Boundary.Requests;
using Test_Search_Api.V1.Boundary.Response;
using Test_Search_Api.Controllers;
using Test_Search_Api.V1.UseCase.Interfaces;

namespace Test_Search_Api.Tests.V1.Controllers;

public class GetAssetListControllerTests {
    private readonly Mock<IGetAssetListUseCase> _mockGetAssetListUseCase;
    private readonly GetAssetListController _classUnderTest;

    public GetAssetListControllerTests()
        {
        
            _mockGetAssetListUseCase = new Mock<IGetAssetListUseCase>();
            _classUnderTest = new GetAssetListController(_mockGetAssetListUseCase.Object, _mockGetAssetListSetsUseCase.Object);
        }
        
         [Fact]
        public async Task GetAssetListShouldCallGetAssetListUseCase()
        {
            // given
            var request = new GetAssetListRequest();
            var response = new GetAssetListResponse();
            _mockGetAssetListUseCase.Setup(x => x.ExecuteAsync(request)).ReturnsAsync(response);

            // when
            await _classUnderTest.GetAssetList(request).ConfigureAwait(false);

            // then
            _mockGetAssetListUseCase.Verify(x => x.ExecuteAsync(request), Times.Once);
        }

}