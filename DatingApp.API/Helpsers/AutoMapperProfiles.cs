﻿using AutoMapper;
using DatingApp.API.Dtos;
using DatingApp.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DatingApp.API.Helpsers
{
    public class AutoMapperProfiles: Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>().
                ForMember(dest=>dest.PhotoUrl, options=>options.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url)).
                ForMember(dest=>dest.Age, options=>options.MapFrom(src=>src.DateOfBirth.CalculateAge()));

            CreateMap<User, UserForDetailedDto>().
                ForMember(dest=>dest.PhotoUrl, options=>options.MapFrom(src=>src.Photos.FirstOrDefault(p=>p.IsMain).Url)).
                ForMember(dest => dest.Age, options => options.MapFrom(src => src.DateOfBirth.CalculateAge())); 

            CreateMap<Photo, PhotoForDetailedDto>();

            CreateMap<UserForUpdateDto, User>();

            CreateMap<Photo, PhotoForReturnDto>();

            CreateMap<PhotoForCreationDto, Photo>();

            CreateMap<User, UserPhotoDto>().
                ForMember(dest => dest.PhotoUrl, options => options.MapFrom(src => src.Photos.FirstOrDefault(p => p.IsMain).Url));
        }
    }
}
