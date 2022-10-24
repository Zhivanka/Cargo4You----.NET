using System.ComponentModel.DataAnnotations;

namespace Cargo4You.Models
{
    public class InputDataModel
    {
        [Required(ErrorMessage = "*please enter width")]
        public int Width { get; set; }

        [Required(ErrorMessage = "*please enter width")]
        public int Height { get; set; }

        [Required(ErrorMessage = "*please enter height")]
        public int Depth { get; set; }

        [Required(ErrorMessage = "*please enter width")]
        public int Weight { get; set; }


    }
}
