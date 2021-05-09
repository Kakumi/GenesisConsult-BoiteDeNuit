using GenesisConsult.Nightclub.Application.Core.Models;
using GenesisConsult.Nightclub.Application.Core.Models.Dto;
using GenesisConsult.Nightclub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Application.Core.Mapper
{
    public class MemberMapper : IMapper<Member, MemberDto>
    {
        private readonly IMapper<IdentityCard, IdentityCardDto> _identityCardMapper;
        private readonly IMapper<MemberCard, MemberCardDto> _memberCardMapper;

        public MemberMapper(IMapper<IdentityCard, IdentityCardDto> identityCardMapper, IMapper<MemberCard, MemberCardDto> memberCardMapper)
        {
            _identityCardMapper = identityCardMapper ?? throw new ArgumentNullException(nameof(identityCardMapper));
            _memberCardMapper = memberCardMapper ?? throw new ArgumentNullException(nameof(memberCardMapper));
        }

        public Member MapDTOToModel(MemberDto dto)
        {
            IdentityCard identityCard = _identityCardMapper.MapDTOToModel(dto.IdentityCard);
            MemberCard memberCard = _memberCardMapper.MapDTOToModel(dto.MemberCard);

            var member = new Member()
            {
                IdentityCard = identityCard,
                MemberCard = memberCard,
                ContactDetails = dto.ContactDetails
            };

            return member;
        }

        public MemberDto MapModelToDTO(Member model)
        {
            return new MemberDto()
            {
                IdentityCard = _identityCardMapper.MapModelToDTO(model.IdentityCard),
                MemberCard = _memberCardMapper.MapModelToDTO(model.MemberCard),
                ContactDetails = model.ContactDetails
            };
        }
    }
}
