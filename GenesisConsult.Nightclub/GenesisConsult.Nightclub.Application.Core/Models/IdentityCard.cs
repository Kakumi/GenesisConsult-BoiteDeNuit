using GenesisConsult.Nightclub.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Text.Json.Serialization;

namespace GenesisConsult.Nightclub.Application.Core.Models
{
    public class IdentityCard
    {
        [Key]
        public int CardNumber { get; set; }
        public string Lastname { get; set; }
        public string Firstname { get; set; }
        public DateTime Birthdate { get; set; }
        public string NationalNumber { get; set; }
        public DateTime ValidFrom { get; set; }
        public DateTime ValidTo { get; set; }

        //For Relationship
        public int MemberId { get; set; }
        [JsonIgnore]
        public Member Member { get; set; }
    }
}
