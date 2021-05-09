using GenesisConsult.Nightclub.Application.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Application.Core.Services.Database.Interfaces
{
    public interface IMemberDataAccess
    {
        Task<Member> GetMemberAsync(int id);
        Task<Member> GetMemberAsync(string nationalNumber);
        Task<IEnumerable<Member>> GetMembersAsync();
        Task<IEnumerable<Member>> GetMembersAsync(Member member, bool strict = false);
        Task RegisterMemberAsync(Member member);
        Task UpdateMemberAsync(Member member);
        Task DeleteMemberAsync(Member member);
    }
}
