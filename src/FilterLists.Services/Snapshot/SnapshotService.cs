﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using FilterLists.Data;
using FilterLists.Services.Snapshot.Models;
using Microsoft.EntityFrameworkCore;

namespace FilterLists.Services.Snapshot
{
    public class SnapshotService : Service
    {
        private readonly Expression<Func<Data.Entities.FilterList, bool>> ifLastSnapFailed =
            l => l.Snapshots
                  .OrderByDescending(s => s.CreatedDateUtc)
                  .Select(s => s.WasUpdated && !s.WasSuccessful)
                  .FirstOrDefault();

        private readonly Expression<Func<Data.Entities.FilterList, DateTime?>> lastSnapTimestamp =
            l => l.Snapshots
                  .Select(s => s.CreatedDateUtc)
                  .OrderByDescending(d => d)
                  .FirstOrDefault();

        private readonly Logger logger;
        private string uaString;

        public SnapshotService(FilterListsDbContext dbContext, IConfigurationProvider mapConfig, Logger logger)
            : base(dbContext, mapConfig) => this.logger = logger;

        public async Task CaptureAsync(int batchSize)
        {
            await CleanupFailedSnapshots();
            uaString = await UserAgentService.GetMostPopularString();
            var lists = await GetListsToCapture(batchSize);
            var snaps = await CreateAndSaveSnaps<Snapshot>(lists);
            var listsToRetry = snaps.Where(s => s.WebExcepted).Select(s => s.List);
            await CreateAndSaveSnaps<SnapshotWayback>(listsToRetry);
        }

        private async Task CleanupFailedSnapshots()
        {
            const string command =
                @"DELETE snapshots_rules
                  FROM snapshots_rules
                  JOIN snapshots ON snapshots.Id = snapshots_rules.SnapshotId
                  WHERE snapshots.WasSuccessful = 0;";
            await DbContext.Database.ExecuteSqlCommandAsync(command);
        }

        private async Task<IEnumerable<FilterListViewUrlDto>> GetListsToCapture(int batchSize) =>
            await DbContext
                  .FilterLists
                  .Where(l => l.Id != 526)  //~2 million rules, too big for SnapshotService currently
                  .OrderBy(l => l.Snapshots.Any())
                  .ThenByDescending(ifLastSnapFailed)
                  .ThenBy(lastSnapTimestamp)
                  .Take(batchSize)
                  .ProjectTo<FilterListViewUrlDto>(MapConfig)
                  .ToListAsync();

        private async Task<List<TSnap>> CreateAndSaveSnaps<TSnap>(IEnumerable<FilterListViewUrlDto> lists)
            where TSnap : Snapshot, new()
        {
            var snaps = CreateSnaps<TSnap>(lists).ToList();
            await SaveSnaps(snaps);
            return snaps;
        }

        private IEnumerable<TSnap> CreateSnaps<TSnap>(IEnumerable<FilterListViewUrlDto> lists)
            where TSnap : Snapshot, new() =>
            lists.Select(l => Activator.CreateInstance(typeof(TSnap), DbContext, l, logger, uaString) as TSnap);

        private static async Task SaveSnaps(IEnumerable<Snapshot> snaps)
        {
            foreach (var snap in snaps)
                await snap.TrySaveAsync();
        }
    }
}