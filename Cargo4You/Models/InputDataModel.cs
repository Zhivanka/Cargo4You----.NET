using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace Cargo4You.Models
{
    public class InputDataModel
    {
        [Required(ErrorMessage = "*please enter width")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Please enter decimal number with \",\"")]
        public string Width { get; set; }

        [Required(ErrorMessage = "*please enter width")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Please enter decimal number with \",\"")]
        public string Height { get; set; }

        [Required(ErrorMessage = "*please enter height")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage = "Please enter decimal number with \",\"")]
        public string Depth { get; set; }

        [Required(ErrorMessage = "*please enter width")]
        [RegularExpression(@"^\d+(\,\d{1,2})?$", ErrorMessage ="Please enter decimal number with \",\"")]
        public string Weight { get; set; }


     

    }
}
