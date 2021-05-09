using GenesisConsult.Nightclub.Application.Core.Mapper;
using GenesisConsult.Nightclub.Application.Core.Models;
using GenesisConsult.Nightclub.Application.Core.Models.Dto;
using GenesisConsult.Nightclub.Application.Core.Services.Database.Interfaces;
using GenesisConsult.Nightclub.Domain.Interfaces;
using GenesisConsult.Nightclub.Infrastructure.Implementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace GenesisConsult.Nightclub.Application.API.IoC
{
    public class ContainerConfiguration
    {
        public static void Initialize(IServiceCollection service)
        {
            // Dependency Injection
            service.AddDbContext<MemberContext>(opt => opt.UseInMemoryDatabase("members"));
            service.AddScoped<IMemberDataAccess, MemberDatabaseDataAccess>();

            service.AddSingleton<IMapper<IdentityCard, IdentityCardDto>, IdentityCardMapper>();
            service.AddSingleton<IMapper<MemberCard, MemberCardDto>, MemberCardMapper>();
            service.AddSingleton<IMapper<Member, MemberDto>, MemberMapper>();
        }
    }
}
