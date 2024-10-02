using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ContactManager.Models
{
    public class Person
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public bool Married { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Salary { get; set; }
    }
}
