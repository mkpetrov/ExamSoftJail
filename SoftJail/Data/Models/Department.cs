using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;

namespace SoftJail.Data.Models
{
    public class Department
    {
        public int Id { get; set; }

        [MinLength(3)]
        [MaxLength(25)]
        public string Name { get; set; }

        [XmlIgnore]
        public ICollection<Cell> Cells { get; set; } = new HashSet<Cell>();
    }
}
