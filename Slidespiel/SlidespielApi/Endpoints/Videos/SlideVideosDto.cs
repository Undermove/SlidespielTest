namespace SlidespielApi.Endpoints.Videos;

public class SlideVideosDto
{
    public int SlideNumber { get; set; }
    public List<VideoInfoDto> Videos { get; set; } = new List<VideoInfoDto>();
}