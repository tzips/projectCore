using AutoMapper;
using PresenceProject.Core.DTOs;
using PresenceProject.Core.Models;
using PresenceProject.Core.DTOs;
using System;
using System.Collections.Generic;
using PresenceProject.Models; // אם צריך עבור List

namespace PresenceProject.Core
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // מיפוי משתמשים
            CreateMap<User, UserDto>()
                // הסרנו את ForMember עבור Duty כי שניהם EDuty
                .ForMember(dest => dest.Presences, opt => opt.MapFrom(src => src.Presence))
                .ReverseMap()
                // גם כאן, אין צורך ב-ForMember עבור Duty ב-ReverseMap אם שניהם EDuty
                .ForMember(dest => dest.Presence, opt => opt.MapFrom(src => src.Presences)); // אם יש מיפוי כזה

            // מיפוי UserPostModel ל-User (זה כבר היה קיים)
            CreateMap<UserPostModel, User>();

            // *** המיפוי המתוקן עבור RegisterModel ל-User ***
            CreateMap<RegisterModel, User>()
                .ForMember(dest => dest.Password, opt => opt.Ignore()) // מתעלם מהשדה Password
                .ForMember(dest => dest.Duty, opt => opt.MapFrom(src => src.Duty)) // ממופה ישירות EDuty ל-EDuty
                .ForMember(dest => dest.Id, opt => opt.Ignore()); // מתעלם מה-Id

            // מיפוי נוכחות
            CreateMap<PresenceCreateModel, Presence>();
            CreateMap<Presence, PresenceDto>() // למטרת Get
                .ForMember(dest => dest.TotalHours,
                    opt => opt.MapFrom(src => (float)(src.End.ToTimeSpan() - src.Start.ToTimeSpan()).TotalHours)).ReverseMap(); ;

            CreateMap<LeaveRequestCreateModel, LeaveRequest>()
    .ForMember(dest => dest.UserId, opt => opt.Ignore()) // אנחנו נגדיר את זה ידנית לפי הטוקן
    .ForMember(dest => dest.Id, opt => opt.Ignore());
            CreateMap<LeaveRequest, LeaveRequestDto>().ReverseMap(); // עבור GET / PUT / DELETE


        }
    }
}
