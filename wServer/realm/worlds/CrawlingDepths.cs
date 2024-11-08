﻿namespace wServer.realm.worlds
{
    public class TheCrawlingDepths : World
    {
        public TheCrawlingDepths()
        {
            Name = "The Crawling Depths";
            ClientWorldName = "The Crawling Depths";
            Background = 0;
            Difficulty = 5;
            AllowTeleport = true;
        }

        public override bool NeedsPortalKey => true;

        protected override void Init()
        {
            LoadMap("wServer.realm.worlds.maps.crawling.wmap", MapType.Wmap);
        }
    }
}
