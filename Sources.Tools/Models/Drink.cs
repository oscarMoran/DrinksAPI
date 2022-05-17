using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace Sources.Tools.Models
{
    [DataContract]
    public class Drink
    {
        [DataMember(Name = "drinkId")]
        public int DrinkId { get; set; }

        [DataMember(Name = "name")]
        [Required]
        public string Name { get; set; }

        [DataMember(Name = "description")]
        [Required]
        public string Description { get; set; }

        [DataMember(Name = "alcoholicGrade")]
        [Required]
        public decimal AlcoholicGrade { get; set; }

        [DataMember(Name = "originCountry")]
        [Required]
        public string OriginCountry { get; set; }

        [DataMember(Name = "history")]
        [Required]
        public string History { get; set; }

        [DataMember(Name = "worldWideRanking")]
        [Required]
        public decimal WorldWideRanking { get; set; }

        [DataMember(Name = "sizeBottle")]
        [Required]
        public string SizeBottle { get; set; }

        [DataMember(Name = "price")]
        [Required]
        public double Price { get; set; }


        [Required]
        public bool InStock { get; set; }
    }
}
