using API.DTOs;
using API.Entities;
using API.Extensions;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API.Helpers
{
    //Job of AutoMapper is to map from one object to another
    public class AutoMapperProfiles:Profile
    {
        public AutoMapperProfiles()
        {
            //Specify - where we want to map from, where we want to map to
            CreateMap<AppUser, MemberDto>()
                //ForMember - which property do we wanna affect
                .ForMember(dest => dest.PhotoUrl, opt => opt.MapFrom
                (src => src.Photos.FirstOrDefault(x => x.IsMain).Url))
                .ForMember(dest => dest.Age, opt => opt.MapFrom(src => src.DateofBirth.CalculateAge())); ;
            CreateMap<Photo, PhotoDto>();
        }
    }
}
