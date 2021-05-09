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
    public class IdentityCardMapper : IMapper<IdentityCard, IdentityCardDto>
    {
        public IdentityCard MapDTOToModel(IdentityCardDto dto)
        {
            return new IdentityCard()
            {
                CardNumber = dto.CardNumber,
                Lastname = dto.Lastname,
                Firstname = dto.Firstname,
                Birthdate = dto.Birthdate,
                NationalNumber = dto.NationalNumber,
                ValidFrom = dto.ValidFrom,
                ValidTo = dto.ValidTo
            };
        }

        public IdentityCardDto MapModelToDTO(IdentityCard model)
        {
            return new IdentityCardDto()
            {
                CardNumber = model.CardNumber,
                Lastname = model.Lastname,
                Firstname = model.Firstname,
                Birthdate = model.Birthdate,
                NationalNumber = model.NationalNumber,
                ValidFrom = model.ValidFrom,
                ValidTo = model.ValidTo
            };
        }
    }
}
