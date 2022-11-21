using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Cargo4You.Models
{

    
    public class CargoModel
    {

        [Key]
        public int Id { get; set; }
        public string Name { get; set; }

        public string[] Validations { get; set; }

        public List<KeyValuePair<string, string>> CalculationsWeight { get; set; }
        public List<KeyValuePair<string, string>> CalculationsDimension { get; set; }

    }
}
