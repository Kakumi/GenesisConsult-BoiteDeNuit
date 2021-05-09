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
    public class MemberCardMapper : IMapper<MemberCard, MemberCardDto>
    {
        public MemberCard MapDTOToModel(MemberCardDto dto)
        {
            return new MemberCard()
            {
                Id = dto.Id
            };
        }

        public MemberCardDto MapModelToDTO(MemberCard model)
        {
            return new MemberCardDto()
            {
                Id = model.Id
            };
        }
    }
}
