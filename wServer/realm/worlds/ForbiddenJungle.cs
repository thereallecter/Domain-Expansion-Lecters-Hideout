namespace wServer.realm.worlds
{
    public class ForbiddenJungle : World
    {
        public ForbiddenJungle()
        {
            Name = "Forbidden Jungle";
            ClientWorldName = "Forbidden Jungle";
            Background = 0;
            Difficulty = 2;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.jungle.jm", MapType.Json);
        }
    }
}
