using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace FilterLists.Services.FilterList.Models
{
    [UsedImplicitly]
    public class ListIndexRecord
    {
        public uint Id { get; set; }
        public IReadOnlyCollection<int> LanguageIds { get; set; }
        public string Name { get; set; }
        public int? RuleCount { get; set; }
        public int? SyntaxId { get; set; }
        public IReadOnlyCollection<int> TagIds { get; set; }
        public DateTime? UpdatedDate { get; set; }
        public string ViewUrl { get; set; }
        public IReadOnlyCollection<string> ViewUrlMirrors { get; set; }
    }
}