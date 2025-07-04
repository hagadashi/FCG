﻿using AutoMapper;
using FCG.Application.DTOs.Games;
using FCG.Application.DTOs.Users;
using FCG.Domain.Entities.Games;
using FCG.Domain.Entities.Users;

namespace FCG.Application.Mappers
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateUserMaps();
            CreateGameMaps();
            CreateSaleMaps();
            CreateLibraryMaps();
        }

        private void CreateUserMaps()
        {
            // User Entity <-> UserDto
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.RoleName, opt => opt.MapFrom(src => src.Role.Name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.FullName))
                .ReverseMap()
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore());

            // CreateUserDto -> User Entity
            CreateMap<CreateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // UpdateUserDto -> User Entity
            CreateMap<UpdateUserDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Role, opt => opt.Ignore())
                .ForMember(dest => dest.PasswordHash, opt => opt.Ignore())
                .ForMember(dest => dest.RoleId, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

            // Role Entity <-> RoleDto
            CreateMap<Role, RoleDto>()
                .ReverseMap();

            // Category Entity <-> CategoryDto
            CreateMap<Category, CategoryDto>()
                .ForMember(dest => dest.GameCount, opt => opt.MapFrom(src => src.Games.Count))
                .ReverseMap()
                .ForMember(dest => dest.Games, opt => opt.Ignore());
        }

        private void CreateGameMaps()
        {
            // Game Entity <-> GameDto
            CreateMap<Game, GameDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.IsOnSale, opt => opt.Ignore())
                .ForMember(dest => dest.SalePrice, opt => opt.Ignore())
                .ReverseMap()
                .ForMember(dest => dest.Category, opt => opt.Ignore());

            // CreateGameDto -> Game Entity
            CreateMap<CreateGameDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // UpdateGameDto -> Game Entity
            CreateMap<UpdateGameDto, Game>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Category, opt => opt.Ignore())
                .ForMember(dest => dest.CategoryId, opt => opt.Condition(src => src.CategoryId.HasValue))
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

            // Category Entity mapping
            CreateMap<Category, object>();
        }

        private void CreateSaleMaps()
        {
            // Sale Entity <-> SaleDto
            CreateMap<Sale, SaleDto>()
                .ReverseMap()
                .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game))
                .ForMember(dest => dest.CreatedByUser, opt => opt.Ignore());

            CreateMap<SaleDto, Sale>()
                .ForMember(dest => dest.Game, opt => opt.Ignore());

            // CreateSaleDto -> Sale Entity
            CreateMap<CreateSaleDto, Sale>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => true));

            // UpdateSaleDto -> Sale Entity
            CreateMap<UpdateSaleDto, Sale>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.GameId, opt => opt.Condition(src => src.GameId.HasValue))
                .ForMember(dest => dest.IsActive, opt => opt.Condition(src => src.IsActive.HasValue));

        }

        private void CreateLibraryMaps()
        {
            // Library Entity <-> LibraryDto
            CreateMap<Library, LibraryDto>()
                .ForMember(dest => dest.Game, opt => opt.MapFrom(src => src.Game));

            CreateMap<LibraryDto, Library>()
                .ForMember(dest => dest.Game, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore());
        }
    }
}