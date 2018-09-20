using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FilterLists.Data;
using FilterLists.Services.FilterList.Models;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Services.FilterList
{
    [UsedImplicitly]
    public class FilterListService : Service
    {
        public FilterListService(FilterListsDbContext dbContext, IConfigurationProvider mapConfig)
            : base(dbContext, mapConfig)
        {
        }

        public async Task<IEnumerable<ListSummaryDto>> GetAllSummariesAsync() =>
            await DbContext.FilterLists.OrderBy(l => l.Name).ProjectTo<ListSummaryDto>(MapConfig).ToListAsync();

        public async Task<IEnumerable<ListIndexRecord>> GetIndexAsync() =>
            await DbContext.FilterLists
                           .OrderBy(l => l.Name)
                           .ProjectTo<ListIndexRecord>(MapConfig)
                           //.Select(l => new ListIndexRecord
                           //{
                           //    Id = (int)l.Id,
                           //    LanguageIds = l.FilterListLanguages.Select(ll => (int)ll.LanguageId).ToList(),
                           //    Name = l.Name,
                           //    RuleCount = DbContext.Snapshots
                           //                         .Where(s => s.WasSuccessful && s.FilterList == l)
                           //                         .Select(s => (int?)s.SnapshotRules.Count)
                           //                         .FirstOrDefault(),
                           //    SyntaxId = (int)l.SyntaxId,
                           //    TagIds = l.FilterListTags.Select(lt => (int)lt.TagId).ToList(),
                           //    UpdatedDate = l.Snapshots.Count(s => s.WasSuccessful && s.WasUpdated) >= 2
                           //        ? l.Snapshots.Where(s => s.WasSuccessful && s.WasUpdated)
                           //           .Select(s => s.CreatedDateUtc)
                           //           .OrderByDescending(c => c)
                           //           .FirstOrDefault()
                           //        : null,
                           //    ViewUrl = l.ViewUrl,
                           //    ViewUrlMirrors = new List<string> {l.ViewUrlMirror1, l.ViewUrlMirror2}
                           //})
                           .ToListAsync();

        public async Task<ListDetailsDto> GetDetailsAsync(uint id) =>
            await DbContext.FilterLists.ProjectTo<ListDetailsDto>(MapConfig)
                           .FirstOrDefaultAsync(x => x.Id == id)
                           .FilterParentListFromMaintainerAdditionalLists();
    }
}