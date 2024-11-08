/*
    Copyright (C) 2015 creepylava

    This file is part of RotMG Dungeon Generator.

    RotMG Dungeon Generator is free software: you can redistribute it and/or modify
    it under the terms of the GNU Affero General Public License as published by
    the Free Software Foundation, either version 3 of the License, or
    (at your option) any later version.

    This program is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
    GNU Affero General Public License for more details.

    You should have received a copy of the GNU Affero General Public License
    along with this program.  If not, see <http://www.gnu.org/licenses/>.

*/

using DungeonGenerator.Dungeon;
using RotMG.Common.Rasterizer;
using System;

namespace DungeonGenerator.Templates.UndeadLair
{
    internal class StartRoom : FixedRoom
    {
        private readonly int w;

        private readonly int h;

        public StartRoom(int w, int h)
        {
            this.w = w;
            this.h = h;
        }

        public override RoomType Type
        { get { return RoomType.Normal; } }

        public override int Width
        { get { return w; } }

        public override int Height
        { get { return h; } }

        private static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.East, 5),
            Tuple.Create(Direction.West, 5),
            Tuple.Create(Direction.South, 5),
            Tuple.Create(Direction.North, 5)
        };

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            rasterizer.FillRect(Bounds, new DungeonTile
            {
                TileType = UndeadLairTemplate.GreyClosed
            });

            int numPortal = 1;

            var buf = rasterizer.Bitmap;
            var bounds = Bounds;

            while (numPortal > 0)
            {
                int x = rand.Next(bounds.X, bounds.MaxX);
                int y = rand.Next(bounds.Y, bounds.MaxY);
                if (buf[x, y].Object != null)
                    continue;

                switch (rand.Next(3))
                {
                    case 0:
                        if (numPortal > 0)
                        {
                            buf[x, y].Region = "Spawn";
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = UndeadLairTemplate.CowardicePortal
                            };
                            numPortal--;
                        }
                        break;
                }
            }
        }
    }
}
