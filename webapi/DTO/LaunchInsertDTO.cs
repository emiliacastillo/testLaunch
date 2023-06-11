

namespace webapi.Models;

public class LaunchInsertDTO
{
     public DateTime DateLunch { get; set; }
    //public DateTime DateCached { get; set; }
    public int MissionID { get; set; }
    public int RocketID { get; set; }
    public bool FirstRocketlaunch { get; set; }
}
