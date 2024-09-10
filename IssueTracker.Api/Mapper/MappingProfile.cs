using System.Linq.Expressions;
using AutoMapper;
using IssueTracker.Domain.Models;
using IssueTracker.Api.DTOs;

namespace IssueTracker.Mapping;

public static class ExtensionMapping
{
    public static IMappingExpression<T, L> Ignore<T, L>(this IMappingExpression<T, L> createmap, params Expression<Func<L, object?>>[] destinationMember)
    {
        return destinationMember.Aggregate(createmap, (current, includeMember) => current.ForMember(includeMember, opt => opt.Ignore()));
    }
}

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<ExecutionList, ExecutionList>();
        CreateMap<ExecutionListDto, ExecutionList>();
        CreateMap<ExecutionList, ExecutionListDto>()
            .ForMember(m => m.UserName, opt => opt.MapFrom(srs => srs.User.UserName));

        CreateMap<WatchList, WatchList>();
        CreateMap<WatchListDto, WatchList>();
        CreateMap<WatchList, WatchListDto>()
            .ForMember(m => m.UserName, opt => opt.MapFrom(srs => srs.User.UserName));

        CreateMap<TicketComment, TicketCommentDto>().ReverseMap();

        CreateMap<Ticket, Ticket>();

        CreateMap<TicketDto, Ticket>()
            .ForMember(dest => dest.Space, opt => opt.Ignore())
            .ForMember(dest => dest.Creator, opt => opt.Ignore())
            .ForMember(dest => dest.Executor, opt => opt.Ignore())
            .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => (typeOfIssue)src.IssueType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (statusOfTask)src.Status))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (priorityOfTask)src.Priority))
            .ForMember(dest => dest.ExecutionList, opt => opt.MapFrom(src => src.ExecutionList))
            .ForMember(dest => dest.WatchList, opt => opt.MapFrom(src => src.WatchList))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));

        CreateMap<Ticket, TicketDto>()
            .ForMember(dest => dest.Creator, opt => opt.MapFrom(src => src.Creator.UserName))
            .ForMember(dest => dest.Executor, opt => opt.MapFrom(src => src.Executor != null ? src.Executor.UserName : string.Empty))
            .ForMember(dest => dest.IssueType, opt => opt.MapFrom(src => (short)src.IssueType))
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => (short)src.Status))
            .ForMember(dest => dest.Priority, opt => opt.MapFrom(src => (short)src.Priority))
            .ForMember(dest => dest.ExecutionList, opt => opt.MapFrom(src => src.ExecutionList))
            .ForMember(dest => dest.WatchList, opt => opt.MapFrom(src => src.WatchList))
            .ForMember(dest => dest.Comments, opt => opt.MapFrom(src => src.Comments));
    }
}