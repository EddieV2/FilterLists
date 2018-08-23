﻿using System.Linq;
using AutoMapper;
using FilterLists.Services.FilterList.Models;
using JetBrains.Annotations;

namespace FilterLists.Services.FilterList.MappingProfiles
{
    [UsedImplicitly]
    public class ListDetailsDtoMappingProfile : Profile
    {
        //TODO: fix UpdatedDate and RuleCount
        public ListDetailsDtoMappingProfile() =>
            CreateMap<Data.Entities.FilterList, ListDetailsDto>()
                .ForMember(d => d.Languages, c => c.MapFrom(l => l.FilterListLanguages.Select(la => la.Language.Name)))
                .ForMember(d => d.Maintainers, c => c.MapFrom(l => l.FilterListMaintainers.Select(m => m.Maintainer)))
                .ForMember(d => d.Tags, c => c.MapFrom(l => l.FilterListTags.Select(m => m.Tag)))
                .ForMember(d => d.RuleCount,
                    c => c.MapFrom(l => 0))
                .ForMember(d => d.UpdatedDate,
                    c => c.MapFrom(l =>
                        l.ModifiedDateUtc));
    }
}