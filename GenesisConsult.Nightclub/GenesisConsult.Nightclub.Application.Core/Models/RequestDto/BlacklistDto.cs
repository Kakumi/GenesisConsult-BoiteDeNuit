using GenesisConsult.Nightclub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Application.Core.Models.RequestDto
{
    public class BlacklistDto : IDto
    {
        public DateTime? EndDate { get; set; }
    }
}
