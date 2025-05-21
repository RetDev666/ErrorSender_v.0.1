using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Domain.Models;
using ErrSendApplication.Mappings;

namespace ErrSendApplication.DTO
{
    public class ErrorMessageDto : IMapWith<ErrorMessage>
    {
        public required string Application { get; set; }
        public required string Version { get; set; }
        public required string Environment { get; set; }
        public required string Message { get; set; }
        public required string StackTrace { get; set; }
        public required string AdditionalData { get; set; }
        public DateTime Time { get; set; }
        public required object Timestamp { get; set; }

        public void Mapping (Profile profile)
        {
            profile.CreateMap<ErrorMessageDto, ErrorMessage>()
                .ForMember(dest => dest.Time, opt => opt.MapFrom(src => src.Timestamp))
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Timestamp));

            profile.CreateMap<ErrorMessage, ErrorMessageDto>()
                .ForMember(dest => dest.Timestamp, opt => opt.MapFrom(src => src.Time));
        }
    }
}
