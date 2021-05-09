using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace GenesisConsult.Nightclub.Application.Core.Models
{
    public class Member
    {
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Key]
        public int Id { get; set; }
        public IdentityCard IdentityCard { get; set; }
        public MemberCard MemberCard { get; set; }
        public string ContactDetails { get; set; }
        public DateTime? EndBlacklisted { get; set; }

        public bool IsBlacklisted()
        {
            return EndBlacklisted != null && DateTime.Compare(DateTime.Now, (DateTime) EndBlacklisted) < 0;
        }

        public Member Clone()
        {
            var identityCard = new IdentityCard()
            {
                CardNumber = IdentityCard.CardNumber,
                Lastname = IdentityCard.Lastname,
                Firstname = IdentityCard.Firstname,
                Birthdate = IdentityCard.Birthdate,
                NationalNumber = IdentityCard.NationalNumber,
                ValidFrom = IdentityCard.ValidFrom,
                ValidTo = IdentityCard.ValidTo,
            };

            var memberCard = new MemberCard()
            {
                Id = MemberCard.Id
            };

            return new Member()
            {
                Id = Id,
                IdentityCard = identityCard,
                MemberCard = memberCard,
                ContactDetails = ContactDetails,
                EndBlacklisted = EndBlacklisted
            };
        }
    }
}
