using System.Linq.Expressions;
using AutoMapper;
using IssueTracker.Dal.Models;

namespace IssueTracker.Mapper;

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
            .ForMember(m => m.userName, opt => opt.MapFrom(srs => srs.User.UserName));

        CreateMap<WatchList, WatchList>();
        CreateMap<WatchListDto, WatchList>();
        CreateMap<WatchList, WatchListDto>()
            .ForMember(m => m.userName, opt => opt.MapFrom(srs => srs.User.UserName));

        CreateMap<TicketComment, TicketCommentDto>().ReverseMap();

        CreateMap<Ticket, Ticket>();

        CreateMap<TicketDto, Ticket>()
            .Ignore(s => s.creator, s => s.executor);

        CreateMap<Ticket, TicketDto>()
            .ForMember(dst => dst.creator, opt => opt.MapFrom(srs => srs.creator.UserName))
            .ForMember(dst => dst.creatorId, opt => opt.MapFrom(srs => srs.creator.Id))
            .ForMember(dst => dst.executor, opt => opt
                .MapFrom(srs => srs.executor != null ? srs.executor.UserName : "")
                )
            .ForMember(dst => dst.executorId, opt => opt
                .MapFrom(srs => srs.executor != null ? srs.executor.Id : new Guid())
                );
    }
}