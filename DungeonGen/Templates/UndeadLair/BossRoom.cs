﻿/*
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
    internal class BossRoom : FixedRoom
    {
        private readonly int radius;

        public BossRoom(int radius)
        {
            this.radius = radius;
        }

        public override RoomType Type
        { get { return RoomType.Target; } }

        public override int Width
        { get { return radius * 2 + 1; } }

        public override int Height
        { get { return radius * 2 + 1; } }

        private static readonly Tuple<Direction, int>[] connections = {
            Tuple.Create(Direction.East, 5),
            Tuple.Create(Direction.West, 5),
            Tuple.Create(Direction.South, 5),
            Tuple.Create(Direction.North, 5)
        };

        public override Tuple<Direction, int>[] ConnectionPoints { get { return connections; } }

        public override void Rasterize(BitmapRasterizer<DungeonTile> rasterizer, Random rand)
        {
            var tile = new DungeonTile
            {
                TileType = UndeadLairTemplate.GreyClosed
            };

            var cX = Pos.X + radius + 0.5;
            var cY = Pos.Y + radius + 0.5;
            var bounds = Bounds;
            var r2 = radius * radius;
            var buf = rasterizer.Bitmap;

            for (int x = bounds.X; x < bounds.MaxX; x++)
            {
                for (int y = bounds.Y; y < bounds.MaxY; y++)
                {
                    if ((x - cX) * (x - cX) + (y - cY) * (y - cY) <= r2)
                        buf[x, y] = tile;
                }
            }

            int numKing = 1;
            int numBoss = new Range(4, 7).Random(rand);
            int numMinion = new Range(4, 7).Random(rand);

            r2 = (radius - 2) * (radius - 2);
            while (numKing > 0 || numBoss > 0 || numMinion > 0)
            {
                int x = rand.Next(bounds.X, bounds.MaxX);
                int y = rand.Next(bounds.Y, bounds.MaxY);

                if ((x - cX) * (x - cX) + (y - cY) * (y - cY) > r2)
                    continue;

                if (buf[x, y].Object != null || buf[x, y].TileType != UndeadLairTemplate.GreyClosed)
                    continue;

                switch (rand.Next(3))
                {
                    case 0:
                        if (numKing > 0)
                        {
                            buf[x, y].Object = new DungeonObject
                            {
                                ObjectType = UndeadLairTemplate.Septavius
                            };
                            numKing--;
                        }
                        break;

                    case 1:
                        if (numBoss > 0)
                        {
                            buf[x, y].Object = new DungeonObject
                            {
                                // ObjectType = UndeadLairTemplate.Boss[rand.Next(UndeadLairTemplate.Boss.Length)]
                            };
                            numBoss--;
                        }
                        break;

                    case 2:
                        if (numMinion > 0)
                        {
                            buf[x, y].Object = new DungeonObject
                            {
                                // ObjectType = UndeadLairTemplate.Minion[rand.Next(UndeadLairTemplate.Minion.Length)]
                            };
                            numMinion--;
                        }
                        break;
                }
            }
        }
    }
}
