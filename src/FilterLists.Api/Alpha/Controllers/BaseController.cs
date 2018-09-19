using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace FilterLists.Api.Alpha.Controllers
{
    [ApiVersion("2")]
    //TODO: use versioning without needing to manually specify in swagger-ui (https://github.com/domaindrivendev/Swashbuckle.AspNetCore/issues/370)
    [Route("v{version:apiVersion}/[controller]")]
    [ResponseCache(Duration = 86400)]
    public class BaseController : Controller
    {
        protected static readonly TimeSpan MemoryCacheExpirationDefault = TimeSpan.FromDays(1);
        protected readonly IMemoryCache MemoryCache;

        public BaseController()
        {
        }

        protected BaseController(IMemoryCache memoryCache) => MemoryCache = memoryCache;
    }
}