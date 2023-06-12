

namespace webapi.Models;

public class Launch
{
    public int ID { get; set; }
    public DateTime DateLunch { get; set; }
    public int MissionID { get; set; }
    public int RocketID { get; set; }
    public bool FirstRocketlaunch { get; set; }
    public virtual Mission? Mission { get; set; }
    public virtual Rocket? Rocket { get; set; }
}
