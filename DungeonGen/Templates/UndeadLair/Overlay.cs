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

namespace DungeonGenerator.Templates.UndeadLair
{
    internal class Overlay : MapRender
    {
        public override void Rasterize()
        {
            var wall = new DungeonTile
            {
                TileType = UndeadLairTemplate.Composite,
                Object = new DungeonObject
                {
                    ObjectType = UndeadLairTemplate.CaveWall
                }
            };
            var space = new DungeonTile
            {
                TileType = UndeadLairTemplate.Space
            };

            int w = Rasterizer.Width, h = Rasterizer.Height;
            var buf = Rasterizer.Bitmap;
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    if (buf[x, y].TileType != UndeadLairTemplate.Composite)
                        continue;

                    bool notWall = false;
                    if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
                        notWall = false;
                    else if (buf[x + 1, y].TileType == UndeadLairTemplate.GreyClosed ||
                             buf[x - 1, y].TileType == UndeadLairTemplate.GreyClosed ||
                             buf[x, y + 1].TileType == UndeadLairTemplate.GreyClosed ||
                             buf[x, y - 1].TileType == UndeadLairTemplate.GreyClosed)
                    {
                        notWall = true;
                    }
                    if (!notWall)
                        buf[x, y] = wall;
                }

            var tmp = (DungeonTile[,])buf.Clone();
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    bool nearComp = false;
                    if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
                        nearComp = false;
                    else if (tmp[x + 1, y].TileType == UndeadLairTemplate.Composite ||
                             tmp[x - 1, y].TileType == UndeadLairTemplate.Composite ||
                             tmp[x, y + 1].TileType == UndeadLairTemplate.Composite ||
                             tmp[x, y - 1].TileType == UndeadLairTemplate.Composite)
                    {
                        nearComp = true;
                    }
                    if (nearComp)
                        buf[x, y] = wall;
                }

            tmp = (DungeonTile[,])buf.Clone();
            for (int x = 0; x < w; x++)
                for (int y = 0; y < h; y++)
                {
                    if (buf[x, y].TileType != UndeadLairTemplate.Composite)
                        continue;

                    bool allWall = false;
                    if (x == 0 || y == 0 || x + 1 == w || y + 1 == h)
                        allWall = true;
                    else
                    {
                        allWall = true;
                        for (int dx = -1; dx <= 1 && allWall; dx++)
                            for (int dy = -1; dy <= 1 && allWall; dy++)
                            {
                                if (tmp[x + dx, y + dy].TileType != UndeadLairTemplate.Composite)
                                {
                                    allWall = false;
                                    break;
                                }
                            }
                    }
                    if (allWall)
                        buf[x, y] = space;
                }
        }
    }
}
