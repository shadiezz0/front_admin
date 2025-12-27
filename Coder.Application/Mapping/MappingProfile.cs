using AutoMapper;
using Coder.Application.DTOs;
using Coder.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Coder.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CodeType Mappings
            CreateMap<CodeType, CodeTypeDto>().ForMember(dest=>dest.CreatedAt,opt=>opt.MapFrom(src=>DateTime.Now)).
                ReverseMap();
            CreateMap<CreateCodeTypeDto, CodeType>();
            CreateMap<UpdateCodeTypeDto, CodeType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeType Mappings
            CreateMap<CodeAttributeType, CodeAttributeTypeDto>().ReverseMap();
            CreateMap<CreateCodeAttributeTypeDto, CodeAttributeType>();
            CreateMap<UpdateCodeAttributeTypeDto, CodeAttributeType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeMain Mappings
            CreateMap<CodeAttributeMain, CodeAttributeMainDto>().ReverseMap();
            CreateMap<CreateCodeAttributeMainDto, CodeAttributeMain>();
            CreateMap<UpdateCodeAttributeMainDto, CodeAttributeMain>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeDetails Mappings
            CreateMap<CodeAttributeDetails, CodeAttributeDetailsDto>().ReverseMap();
            CreateMap<CreateCodeAttributeDetailsDto, CodeAttributeDetails>();
            CreateMap<UpdateCodeAttributeDetailsDto, CodeAttributeDetails>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeTypeSetting Mappings
            CreateMap<CodeTypeSetting, CodeTypeSettingDto>().ReverseMap();
            CreateMap<CreateCodeTypeSettingDto, CodeTypeSetting>();
            CreateMap<UpdateCodeTypeSettingDto, CodeTypeSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeTypeSequence Mappings
            CreateMap<CodeTypeSequence, CodeTypeSequenceDto>().ReverseMap();
            CreateMap<CreateCodeTypeSequenceDto, CodeTypeSequence>();
            CreateMap<UpdateCodeTypeSequenceDto, CodeTypeSequence>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Code Mappings
            CreateMap<Code, CodeDto>().ReverseMap();
            CreateMap<CreateCodeDto, Code>();
            CreateMap<UpdateCodeDto, Code>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
    