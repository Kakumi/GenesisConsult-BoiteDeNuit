using GenesisConsult.Nightclub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Application.Core.Models.Dto
{
    public class MemberDto : IDto
    {
        public IdentityCardDto IdentityCard { get; set; }
        public MemberCardDto MemberCard { get; set; }
        public string ContactDetails { get; set; }

        public MemberDto Clone()
        {
            var identityCard = new IdentityCardDto()
            {
                CardNumber = IdentityCard.CardNumber,
                Lastname = IdentityCard.Lastname,
                Firstname = IdentityCard.Firstname,
                Birthdate = IdentityCard.Birthdate,
                NationalNumber = IdentityCard.NationalNumber,
                ValidFrom = IdentityCard.ValidFrom,
                ValidTo = IdentityCard.ValidTo,
            };

            var memberCard = new MemberCardDto()
            {
                Id = MemberCard.Id
            };

            return new MemberDto()
            {
                IdentityCard = identityCard,
                MemberCard = memberCard,
                ContactDetails = ContactDetails
            };
        }
    }
}
