using GenesisConsult.Nightclub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json.Serialization;

namespace GenesisConsult.Nightclub.Application.Core.Models
{
    public class MemberCard
    {
        public int Id { get; set; }

        //For Relationship
        public int MemberId { get; set; }
        [JsonIgnore]
        public Member Member { get; set; }
    }
}
