using System.Linq.Expressions;
using AutoMapper;
using IssueTracker.Dal.Models;

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
            .Ignore(s => s.Creator, s => s.Executor);

        CreateMap<Ticket, TicketDto>()
            .ForMember(dst => dst.Creator, opt => opt.MapFrom(srs => srs.Creator.UserName))
            .ForMember(dst => dst.CreatorId, opt => opt.MapFrom(srs => srs.Creator.Id))
            .ForMember(dst => dst.Executor, opt => opt
                .MapFrom(srs => srs.Executor != null ? srs.Executor.UserName : "")
            )
            .ForMember(dst => dst.ExecutorId, opt => opt
                .MapFrom(srs => srs.Executor != null ? srs.Executor.Id : new Guid())
            );
    }
}