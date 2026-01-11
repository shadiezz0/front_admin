using AutoMapper;
using Coder.Application.DTOs;
using Coder.Domain.Entities;

namespace Coder.Application.Mapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // CodeType Mappings
            CreateMap<CreateCodeTypeDto, CodeType>();
            CreateMap<CodeType, CodeTypeDto>();

            CreateMap<CreateCodeTypeDto, CodeType>();
            CreateMap<UpdateCodeTypeDto, CodeType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeType Mappings
            CreateMap<CodeAttributeType, CodeAttributeTypeDto>().ReverseMap();
            CreateMap<CreateCodeAttributeTypeDto, CodeAttributeType>();
            CreateMap<UpdateCodeAttributeTypeDto, CodeAttributeType>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeMain Mappings
            CreateMap<CodeAttributeMain, CodeAttributeMainDto>();
            CreateMap<CreateCodeAttributeMainDto, CodeAttributeMain>();
            CreateMap<UpdateCodeAttributeMainDto, CodeAttributeMain>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeAttributeDetails Mappings
            CreateMap<CodeAttributeDetails, CodeAttributeDetailsDto>();
            CreateMap<CreateCodeAttributeDetailsDto, CodeAttributeDetails>();
            CreateMap<UpdateCodeAttributeDetailsDto, CodeAttributeDetails>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeTypeSetting Mappings
            CreateMap<CodeTypeSetting, CodeTypeSettingDto>();
            CreateMap<CreateCodeTypeSettingDto, CodeTypeSetting>();
            CreateMap<UpdateCodeTypeSettingDto, CodeTypeSetting>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // CodeTypeSequence Mappings
            CreateMap<CodeTypeSequence, CodeTypeSequenceDto>();
            CreateMap<CreateCodeTypeSequenceDto, CodeTypeSequence>();
            CreateMap<UpdateCodeTypeSequenceDto, CodeTypeSequence>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            // Code Mappings
            CreateMap<Code, CodeDto>().ReverseMap();

            CreateMap<CreateCodeDto, Code>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => "DRAFT"))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(src => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CodeGenerated, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CodeType, opt => opt.Ignore());

            CreateMap<UpdateCodeDto, Code>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CodeTypeId, opt => opt.Ignore())
                .ForMember(dest => dest.CodeGenerated, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedAt, opt => opt.Ignore())
                .ForMember(dest => dest.ApprovedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CodeType, opt => opt.Ignore());
        }
    }
}
    