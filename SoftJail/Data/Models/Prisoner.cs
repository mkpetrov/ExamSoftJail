using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SoftJail.Data.Models
{
    public class Prisoner
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(20)]
        [Required]
        public string FullName { get; set; }

        [Required]
        public string Nickname { get; set; }

        [Range(18, 65)]
        [Required]
        public int Age { get; set; }

        [Required]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime IncarcerationDate { get; set; }

        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public DateTime ReleaseDate { get; set; }

        [Range(0, double.MaxValue)]
        [JsonProperty(NullValueHandling = NullValueHandling.Ignore)]
        public decimal Bail { get; set; }

        [ForeignKey(nameof(Cell))]
        public int CellId { get; set; }

        public Cell Cell { get; set; }

        public ICollection<Mail> Mails { get; set; } = new HashSet<Mail>();

        public ICollection<OfficerPrisoner> PrisonerOfficers { get; set; } = new HashSet<OfficerPrisoner>();
    }
}