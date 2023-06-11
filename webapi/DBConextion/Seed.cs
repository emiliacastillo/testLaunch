/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using webapi.Models;

namespace webapi.DBConextion
{
    public class Seed
    {
        public class SeedInitializer : System.Data.Entity.DropCreateDatabaseIfModelChanges<DBConexion>
        {
            protected override void Seed(DBConexion context)
            {
                var missions = new List<Mission>
            {
            new Mission{MissionName="Mission1"},
            new Mission{MissionName="Mission2"},
            new Mission{MissionName="Mission3"},
            new Mission{MissionName="Mission4"},
            new Mission{MissionName="Mission5"},
            new Mission{MissionName="Mission6"},
            new Mission{MissionName="Mission7"},
            new Mission{MissionName="Mission8"}
            };

                missions.ForEach(s => context.Missions?.Add(s));
                context.SaveChanges();
            }
        }
    }
}*/
