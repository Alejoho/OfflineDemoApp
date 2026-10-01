using System.ComponentModel.DataAnnotations;

namespace OfflineDemo.Models;

public class UploadModel
{
	[Required]
	[StringLength(100, ErrorMessage = "Title must be less than 100 characters.")]
	public string Title { get; set; } = "";

	[Required]
	[StringLength(500, ErrorMessage = "Description must be less than 500 characters.")]
	public string Description { get; set; } = "";

	[StringLength(200, ErrorMessage = "Hashtags must be less than 200 characters.")]
	public string Hashtags { get; set; } = "#";
}
