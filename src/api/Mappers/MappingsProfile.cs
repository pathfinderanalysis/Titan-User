using AutoMapper;
using System;
using System.Linq;
using Titan.UFC.Users.WebAPI.Entities;
using Titan.UFC.Users.WebAPI.Models;

namespace Titan.UFC.Users.WebAPI.Mappers
{
    public class MappingsProfile : Profile
    {
        public MappingsProfile()
        {     
            CreateMap<User, UserEntity>()
                .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(o => o.Contacts.Email))
                .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(o => o.Contacts.Phone));

            CreateMap<UserUpdate, UserEntity>()
              .ForMember(dest => dest.UserAddresses, opt => opt.Ignore())
              .ForMember(dest => dest.UpdatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
              .ForMember(dest => dest.UserEmail, opt => opt.MapFrom(o => o.Contacts.Email))
              .ForMember(dest => dest.UserPhone, opt => opt.MapFrom(o => o.Contacts.Phone));

            CreateMap<Address, AddressEntity>()
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow));
            
            CreateMap<Address, UserAddressEntity>()
                 .ForMember(dest => dest.AddressID, opt => opt.MapFrom(x => x.AddressID))
                .ForMember(dest => dest.UserID, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(dest => dest.Address, src => src.MapFrom(o => o));

            CreateMap<AddressEntity, Address>();

            CreateMap<UserEntity, User>()
                .ForMember(dest => dest.Addresses, src => src.MapFrom(x => x.UserAddresses.Select(y => y.Address).ToList()))
                .ForMember(dest => dest.Contacts, opt => opt.MapFrom(o => o));

            CreateMap<UserEntity, Contacts>()
                .ForMember(dest => dest.Email, src => src.MapFrom(x => x.UserEmail)) 
                .ForMember(dest => dest.Phone, src => src.MapFrom(x => x.UserPhone));
            
            CreateMap<Contacts, UserEntity>()
                .ForMember(dest => dest.UserEmail, src => src.MapFrom(x => x.Email))
                .ForMember(dest => dest.UserPhone, src => src.MapFrom(x => x.Phone));

            CreateMap<Email, UserEmailEntity>()
                .ForMember(dest => dest.UserEmailID, src => src.MapFrom(x => x.Id))
                .ForMember(dest => dest.EmailAddress, opt => opt.MapFrom(o => o.Address))
                .ForMember(dest => dest.CreatedDate, opt => opt.MapFrom(o => DateTime.UtcNow))
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationKey, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());

            CreateMap<UserEmailEntity, Email>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.UserEmailID))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(o => o.EmailAddress))                
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(o => o.IsVerified));

            CreateMap<Phone, UserPhoneEntity>()
                .ForMember(dest => dest.UserPhoneID, opt => opt.MapFrom(o => o.Id))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(o => o.Number))
                .ForMember(dest => dest.CallingCode, opt => opt.MapFrom(o => o.CountryCode))
                .ForMember(dest => dest.IsVerified, opt => opt.Ignore())
                .ForMember(dest => dest.VerificationKey, opt => opt.Ignore())
                .ForMember(dest => dest.VerifiedDate, opt => opt.Ignore())
                .ForMember(dest => dest.UserID, opt => opt.Ignore());
            
            CreateMap<UserPhoneEntity, Phone>()
                .ForMember(dest => dest.Number, opt => opt.MapFrom(o => o.PhoneNumber))
                .ForMember(dest => dest.CountryCode, opt => opt.MapFrom(o => o.CallingCode))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(o => o.UserPhoneID))
                .ForMember(dest => dest.IsVerified, opt => opt.MapFrom(o => o.IsVerified));

            CreateMap<ForgotPasswordEntity, ForgotPassword>();
        }
    }
}
