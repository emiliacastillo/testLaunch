
namespace webapi.DTO
{
    public class LaunchIdDTO
    {
        public int ID { get; set; }
        public DateTime DateCached { get; set; }
        public string? MissionName { get; set; }
        public DateTime DateLunch { get; set; }
        public string? RocketName { get; set; }
        public bool FirstRocketlaunch { get; set; }
        public int RateSucessRocket { get; set; }

    }

}
