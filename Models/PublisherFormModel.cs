using System.ComponentModel.DataAnnotations;

namespace EpicGameWebAppStore.Models
{
    public class PublisherFormModel
    {
        [Required(ErrorMessage = "Name is required")]
		public string Name { get; set; }

		[Required(ErrorMessage = "Address is required")]
		public string Address { get; set; }

		[Required(ErrorMessage = "Email is required")]
		public string Email { get; set; }

		[Required(ErrorMessage = "Is this Phone?")]
		public string Phone { get; set; }

		[Required(ErrorMessage = "Website is required")]
		public string Website { get; set; }
    }
}