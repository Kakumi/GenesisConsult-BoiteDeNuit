using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GenesisConsult.Nightclub.Application.Core.Models;
using GenesisConsult.Nightclub.Infrastructure.Implementations;
using GenesisConsult.Nightclub.Domain.Interfaces;
using GenesisConsult.Nightclub.Application.Core.Models.Dto;
using GenesisConsult.Nightclub.Application.Core.Security;
using GenesisConsult.Nightclub.Application.Core.Models.RequestDto;
using GenesisConsult.Nightclub.Application.Core.Services.Database.Interfaces;

namespace GenesisConsult.Nightclub.Application.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MembersController : ControllerBase
    {
        #region Variables
        private readonly IMemberDataAccess _context;
        private readonly IMapper<Member, MemberDto> _memberMapper;
        #endregion

        #region Controller
        public MembersController(IMemberDataAccess context, IMapper<Member, MemberDto> memberMapper)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _memberMapper = memberMapper ?? throw new ArgumentNullException(nameof(memberMapper));
        }
        #endregion

        #region Routes

        // GET: api/Members
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Member>>> GetMembers()
        {
            var members = await _context.GetMembersAsync();

            return Ok(members);
        }

        // GET: api/Members/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Member>> GetMember(int id)
        {
            var member = await _context.GetMemberAsync(id);

            if (member == null)
            {
                return NotFound();
            }

            return Ok(member);
        }

        // POST: api/Members
        [HttpPost]
        public async Task<ActionResult<Member>> RegisterMember(MemberDto body)
        {
            var member = _memberMapper.MapDTOToModel(body);

            try
            {
                ValidateMember(member);

                //We check if someone used the same identity card and member card to avoid duplicating member
                //But it's possible to have same indentity card but another member card
                var searchedMember = (await _context.GetMembersAsync(member, strict: true)).FirstOrDefault();
                if (searchedMember != null)
                {
                    return BadRequest("These cards are already used by someone");
                }
            }
            catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                //Select persons that have id in common
                IEnumerable<Member> searchedMembers = (await _context.GetMembersAsync(member, strict: false)).Where(m =>
                {
                    return (m.IdentityCard.CardNumber == member.IdentityCard.CardNumber && m.MemberCard.Id != member.MemberCard.Id)
                        || (m.IdentityCard.CardNumber != member.IdentityCard.CardNumber && m.MemberCard.Id == member.MemberCard.Id);
                }).ToList();

                if (searchedMembers.Count() > 1)
                {
                    return BadRequest("Some people were find with one of these ID. It's impossible for a new member");
                }

                //reason:
                //  - Lost his member card
                //  - Identity Card has changed
                if (searchedMembers.Count() == 1)
                {
                    member.EndBlacklisted = searchedMembers.First().EndBlacklisted;

                    //Delete of the person because previous data are outdated
                    await _context.DeleteMemberAsync(searchedMembers.First());
                } else
                {
                    var searchedMemberNN = await _context.GetMemberAsync(member.IdentityCard.NationalNumber);
                    if (searchedMemberNN != null)
                    {
                        return BadRequest("this national number is already used");
                    }
                }

                await _context.RegisterMemberAsync(member);

                return CreatedAtAction(nameof(RegisterMember), member);
            } catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // PATCH: api/Members/5
        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateMember(int id, MemberDto body)
        {
            var member = _memberMapper.MapDTOToModel(body);

            try
            {
                ValidateMember(member);
            } catch(Exception e)
            {
                return BadRequest(e.Message);
            }

            try
            {
                var memberFromDatabase = await _context.GetMemberAsync(id);
                if (memberFromDatabase == null)
                {
                    return NotFound();
                }

                if (member.IdentityCard?.CardNumber != memberFromDatabase.IdentityCard?.CardNumber || member.MemberCard?.Id != memberFromDatabase.MemberCard?.Id)
                {
                    return BadRequest("Card Number for Identity Card or ID for Member Card is different. If new card is used, you must call 'POST' method");
                }

                var searchedMember = await _context.GetMemberAsync(member.IdentityCard.NationalNumber);
                if (searchedMember != null && searchedMember.Id != id)
                {
                    return BadRequest("this national number is already used");
                }

                memberFromDatabase.IdentityCard.Lastname = member.IdentityCard.Lastname;
                memberFromDatabase.IdentityCard.Firstname = member.IdentityCard.Firstname;
                memberFromDatabase.IdentityCard.NationalNumber = member.IdentityCard.NationalNumber;
                memberFromDatabase.IdentityCard.Birthdate = member.IdentityCard.Birthdate;
                memberFromDatabase.IdentityCard.ValidFrom = member.IdentityCard.ValidFrom;
                memberFromDatabase.IdentityCard.ValidTo = member.IdentityCard.ValidTo;
                memberFromDatabase.ContactDetails = member.ContactDetails;

                await _context.UpdateMemberAsync(memberFromDatabase);
            }
            catch(Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return NoContent();
        }

        // PATCH: api/Members/blacklist/5
        [HttpPatch, Route("blacklist/{id}")]
        public async Task<IActionResult> BlacklistMember(int id, BlacklistDto blacklistDto)
        {
            if (blacklistDto.EndDate != null && DateTime.Compare((DateTime) blacklistDto.EndDate, DateTime.Now) < 0)
            {
                return BadRequest("Blacklist date is invalid because already passed");
            }

            try
            {
                var member = await _context.GetMemberAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                member.EndBlacklisted = blacklistDto.EndDate;

                await _context.UpdateMemberAsync(member);
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }

            return NoContent();
        }

        // GET: api/Members/blacklist/5
        [HttpGet, Route("blacklist/{id}")]
        public async Task<IActionResult> IsBlacklistedMember(int id)
        {
            try
            {
                var member = await _context.GetMemberAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                return Ok(member.IsBlacklisted());
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // GET: api/Members/access/5
        [HttpGet, Route("access/{id}")]
        public async Task<IActionResult> CanEnter(int id)
        {
            try
            {
                var member = await _context.GetMemberAsync(id);
                if (member == null)
                {
                    return NotFound();
                }

                if (member.IsBlacklisted() || DateTime.Compare(member.IdentityCard.ValidTo, DateTime.Now) < 0)
                {
                    return Unauthorized();
                }

                return Ok();
            }
            catch (Exception e)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, e.Message);
            }
        }

        // DELETE: api/Members/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMember(int id)
        {
            var member = await _context.GetMemberAsync(id);
            if (member == null)
            {
                return NotFound();
            }

            await _context.DeleteMemberAsync(member);

            return NoContent();
        }

        #endregion

        #region Business

        #region Private Section

        private void ValidateMember(Member member)
        {
            Guards.IsNotNullOrEmpty(member.ContactDetails, nameof(member.ContactDetails));
            Guards.IsNotNull(member.IdentityCard.CardNumber, nameof(member.IdentityCard.CardNumber));
            Guards.IsNotNullOrEmpty(member.IdentityCard.Firstname, nameof(member.IdentityCard.Firstname));
            Guards.IsNotNullOrEmpty(member.IdentityCard.Lastname, nameof(member.IdentityCard.Lastname));
            Guards.IsNotNull(member.IdentityCard.Birthdate, nameof(member.IdentityCard.Birthdate));
            Guards.IsNotNullOrEmpty(member.IdentityCard.NationalNumber, nameof(member.IdentityCard.NationalNumber));
            Guards.IsNotNull(member.IdentityCard.ValidFrom, nameof(member.IdentityCard.ValidFrom));
            Guards.IsNotNull(member.IdentityCard.ValidTo, nameof(member.IdentityCard.ValidTo));
            Guards.IsNotNull(member.MemberCard.Id, nameof(member.MemberCard.Id));

            Guards.IsValidContactDetails(member.ContactDetails); //Check email or phone number format
            Guards.IsAdult(member.IdentityCard.Birthdate); //Check age > 18
            Guards.IsDateLessThan(member.IdentityCard.ValidFrom, member.IdentityCard.ValidTo, nameof(member.IdentityCard.ValidTo));
            Guards.IsDateStillValid(member.IdentityCard.ValidTo, nameof(member.IdentityCard.ValidTo));
            Guards.IsValidNationalNumber(member.IdentityCard.NationalNumber); //Check national number
        }

        #endregion

        #endregion
    }
}
