using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GenesisConsult.Nightclub.Domain.Interfaces
{
    public interface IMapper<T, S> where S : IDto
    {
        T MapDTOToModel(S dto);
        S MapModelToDTO(T model);
    }
}
