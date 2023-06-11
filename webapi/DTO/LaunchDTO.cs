
namespace webapi.DTO
{
    public class LaunchDTO
    {
        public DateTime DateCached { get; set; }
        public string? MissionName { get; set; }
        public DateTime DateLunch { get; set; }
        public string? RocketName { get; set; }
        public bool FirstRocketlaunch { get; set; }
        public int RateSucessRocket { get; set; }

    }

}
