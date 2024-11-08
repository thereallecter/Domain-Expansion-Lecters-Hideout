using DungeonGenerator;
using DungeonGenerator.Templates;
using DungeonGenerator.Templates.Abyss;
using DungeonGenerator.Templates.Lab;
using DungeonGenerator.Templates.PirateCave;
using DungeonGenerator.Templates.UndeadLair;
using log4net;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using wServer.logic.behaviors;
using wServer.realm.worlds;

namespace wServer
{
    public static class GeneratorCache
    {
        private static readonly ILog log = LogManager.GetLogger(typeof(GeneratorCache));

        private static Dictionary<string, List<string>> CachedMaps;

        public static void Init()
        {
            CachedMaps = new Dictionary<string, List<string>>();
            CreateCache("Abyss of Demons", new AbyssTemplate());
            CreateCache("Mad Lab", new LabTemplate());
            CreateCache("Pirate Cave", new PirateCaveTemplate());
            CreateCache("Undead Lair", new UndeadLairTemplate());
        }

        public static string NextAbyss(uint seed) => NextMap(seed, "Abyss of Demons", new AbyssTemplate());
        public static string NextLab(uint seed) => NextMap(seed, "Mad Lab", new LabTemplate());
        public static string NextPirateCave(uint seed) => NextMap(seed, "Pirate Cave", new PirateCaveTemplate());
        public static string NextUndeadLair(uint seed) => NextMap(seed, "Undead Lair", new UndeadLairTemplate());

        private static string NextMap(uint seed, string key, DungeonTemplate template)
        {
            var map = CachedMaps[key][0];
            CachedMaps[key].RemoveAt(0);
            log.Info($"Generating new map for dungeon: {key}");
            Task.Factory.StartNew(() => CachedMaps[key].Add(GenerateNext(seed, template)));
            return map;
        }

        private static string GenerateNext(uint seed, DungeonTemplate template)
        {
            var gen = new DungeonGen((int)seed, template);
            gen.GenerateAsync();
            return gen.ExportToJson();
        }

        private static void CreateCache(string key, DungeonTemplate template)
        {
            uint max = 12;
            Random rand = new Random();
            log.Info($"Generating cache for dungeon: {key}");
            CachedMaps.Add(key, new List<string>());
            for (var i = 0; i < max; i++) //Keep at least 12 maps in cache
                CachedMaps[key].Add(GenerateNext((uint)rand.Next(0, (int)max - 1), template));
        }
    }
}
