﻿using AutoMapper;
using FilterLists.Services.Language.Models;
using JetBrains.Annotations;

namespace FilterLists.Services.FilterList.MappingProfiles
{
    [UsedImplicitly]
    public class LanguageDtoMappingProfile : Profile
    {
        public LanguageDtoMappingProfile() =>
            CreateMap<Data.Entities.Language, LanguageDto>()
                .ForMember(dest => dest.Id,
                    opt => opt.MapFrom(src =>
                        (int)src.Id));
    }
}