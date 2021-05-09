using GenesisConsult.Nightclub.Application.Core.Models;
using GenesisConsult.Nightclub.Application.Core.Services.Database.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Infrastructure.Implementations
{
    public class MemberDatabaseDataAccess : IMemberDataAccess
    {
        private readonly MemberContext _context;
        public MemberDatabaseDataAccess(MemberContext context)
        {
            _context = context;
        }

        public async Task<Member> GetMemberAsync(int id)
        {
            return await _context.Members
                .Include(member => member.IdentityCard)
                .Include(member => member.MemberCard)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Member> GetMemberAsync(string nationalNumber)
        {
            return await _context.Members
                .Include(member => member.IdentityCard)
                .Include(member => member.MemberCard)
                .FirstOrDefaultAsync(m => m.IdentityCard.NationalNumber == nationalNumber);
        }

        public async Task<IEnumerable<Member>> GetMembersAsync()
        {
            return await _context.Members
                .Include(member => member.IdentityCard)
                .Include(member => member.MemberCard)
                .ToListAsync();
        }

        public async Task RegisterMemberAsync(Member member)
        {
            _context.Members.Add(member);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateMemberAsync(Member member)
        {
            _context.Entry(member).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMemberAsync(Member member)
        {
            _context.Members.Remove(member);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Member>> GetMembersAsync(Member member, bool strict = false)
        {
            if (strict)
            {
                return await _context.Members
                    .Include(member => member.IdentityCard)
                    .Include(member => member.MemberCard)
                    .Where(m => m.IdentityCard.CardNumber == member.IdentityCard.CardNumber && m.MemberCard.Id == member.MemberCard.Id).ToListAsync();
            }

            return await _context.Members
                    .Include(member => member.IdentityCard)
                    .Include(member => member.MemberCard)
                    .Where(m => m.IdentityCard.CardNumber == member.IdentityCard.CardNumber || m.MemberCard.Id == member.MemberCard.Id).ToListAsync();
        }
    }
}
