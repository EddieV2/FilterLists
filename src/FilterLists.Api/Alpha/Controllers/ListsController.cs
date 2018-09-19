using System.Threading.Tasks;
using FilterLists.Services.FilterList;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FilterLists.Api.Alpha.Controllers
{
    public class ListsController : BaseController
    {
        private readonly FilterListService filterListService;

        public ListsController(IMemoryCache memoryCache, FilterListService filterListService) :
            base(memoryCache) => this.filterListService = filterListService;

        [HttpGet]
        public async Task<IActionResult> Index() =>
            Json(await MemoryCache.GetOrCreate("alpha_ListsController_Index", entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = MemoryCacheExpirationDefault;
                return filterListService.GetIndexAsync();
            }));
    }
}