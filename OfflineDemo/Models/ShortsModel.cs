namespace OfflineDemo.Models;

public class ShortsModel
{
	public int Id { get; set; }
	public string Title { get; set; }
	public string Description { get; set; }
	public string Hashtags { get; set; }
	public string Mp4FileUrl { get; set; }
	public string ImageFileUrl { get; set; }
	public bool IsUploaded { get; set; } = false;
}
