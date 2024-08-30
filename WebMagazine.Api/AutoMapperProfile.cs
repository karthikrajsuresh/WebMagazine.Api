using AutoMapper;
using WebMagazine.Api.DTOs;
using WebMagazine.Api.Models;

namespace WebMagazine.Api
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserDTO>().ReverseMap();
            CreateMap<Role, RoleDTO>().ReverseMap();
            CreateMap<Article, ArticleDTO>().ReverseMap();
            CreateMap<Subscription, SubscriptionDTO>().ReverseMap();
            CreateMap<Notification, NotificationDTO>().ReverseMap();
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Comment, CommentDTO>().ReverseMap();
        }
    }
}
