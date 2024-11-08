#region

using wServer.networking;

#endregion

namespace wServer.realm.worlds
{
    public class UndeadLair : World
    {
        public UndeadLair()
        {
            Name = "Undead Lair";
            ClientWorldName = "Undead Lair";
            Dungeon = true;
            Background = 0;
            AllowTeleport = true;
        }

        protected override void Init()
        {
            LoadMap(GeneratorCache.NextUndeadLair(Seed));
        }

        public override World GetInstance(Client psr) => Manager.AddWorld(new UndeadLair());
    }
}
