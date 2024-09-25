using AutoMapper;
using Forum.Core.Requests;
using Forum.Core.Responses;
using Forum.EF.Entities;

namespace Forum.Core.Mapper;

public class AutoMapperConfiguration : Profile
{
    public AutoMapperConfiguration()
    {
        CreateMap<Category, CategoryResponse>();
        CreateMap<CategoryResponse, Category>();
        CreateMap<CategoryRequest, Category>();
        CreateMap<Category, CategoryRequest>();
        CreateMap<Room, RoomResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.UserName));
        CreateMap<RoomResponse, Room>();
        CreateMap<UserResponse, User>();
        CreateMap<User, UserResponse>();
        CreateMap<CommentRequest, Comment>();
        CreateMap<Comment, CommentRequest>();
        CreateMap<CommentResponse, Comment>();
        CreateMap<Comment, CommentResponse>()
            .ForMember(dest => dest.Author, opt => opt.MapFrom(src => src.User.UserName))
            .ForMember(dest => dest.RoomTitle, opt => opt.MapFrom(src => src.Room.Title ?? ""));
        CreateMap<RoomRequest, Room>();
        CreateMap<Room, RoomRequest>();
        CreateMap<SingleUserResponse, User>();
        CreateMap<User, SingleUserResponse>();
        CreateMap<CommentInRoomRequest, Comment>();
        CreateMap<Comment, CommentInRoomRequest>();
        CreateMap<Permission, PermissionRequest>();
        CreateMap<Permission, PermissionResponse>()
            .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
        CreateMap<PermissionResponse, Permission>();
        CreateMap<PermissionRequest, Permission>();
        CreateMap<RoomRequest, RoomUpdateRequest>();
        CreateMap<RoomUpdateRequest, RoomRequest>();
    }
}
