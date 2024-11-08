namespace wServer.realm.worlds
{
    public class DavyJonesLocker : World
    {
        public DavyJonesLocker()
        {
            Name = "Davy Jones's Locker";
            ClientWorldName = "dungeons.Davy_JonesAPOSs_Locker";
            Dungeon = true;
            Difficulty = 5;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.davy.jm", MapType.Json);



        }
    }
}
