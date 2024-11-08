namespace wServer.realm.worlds
{
    public class BattlefortheNexus : World
    {
        public BattlefortheNexus()
        {
            Name = "Battle for the Nexus";
            ClientWorldName = "Battle for the Nexus";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.BattlefortheNexus.jm", MapType.Json);
        }
    }
}