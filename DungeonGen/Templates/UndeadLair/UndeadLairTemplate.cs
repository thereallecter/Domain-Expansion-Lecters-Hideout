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
using RotMG.Common;
using System;

namespace DungeonGenerator.Templates.UndeadLair
{
    public class UndeadLairTemplate : DungeonTemplate
    {
        internal static readonly TileType LightSand = new TileType(0x00bd, "Light Sand");

        internal static readonly TileType GreyClosed = new TileType(0x05, "Grey Closed");

        internal static readonly TileType Composite = new TileType(0x00fd, "Composite");

        internal static readonly TileType Space = new TileType(0x00fe, "Space");

        internal static readonly ObjectType CaveWall = new ObjectType(0x01ce, "Cave Wall");

        internal static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");

        internal static readonly ObjectType PirateKing = new ObjectType(0x0927, "Dreadstump the Pirate King");

        internal static readonly ObjectType[] Boss = {
        };

        internal static readonly ObjectType[] Minion = {
        };

        internal static readonly ObjectType[] Pet = {
        };

        public override int MaxDepth
        { get { return 36; } }

        private NormDist targetDepth;
        public override NormDist TargetDepth
        { get { return targetDepth; } }

        public override NormDist SpecialRmCount
        { get { return null; } }

        public override NormDist SpecialRmDepthDist
        { get { return null; } }

        public override Range RoomSeparation
        { get { return new Range(5, 5); } }

        public override int CorridorWidth
        { get { return 5; } }

        public override void Initialize()
        {
            targetDepth = new NormDist(2, 24, 12, 36, Rand.Next());
        }

        public override Room CreateStart(int depth)
        {
            return new StartRoom(14, 14);
        }

        public override Room CreateTarget(int depth, Room prev)
        {
            return new BossRoom(14);
        }

        public override Room CreateSpecial(int depth, Room prev)
        {
            throw new InvalidOperationException();
        }

        public override Room CreateNormal(int depth, Room prev)
        {
            return new NormalRoom(Rand.Next(14, 14), Rand.Next(14, 14));
        }

        public override MapCorridor CreateCorridor()
        {
            return new Corridor();
        }

        public override MapRender CreateBackground()
        {
            return new Background();
        }

        public override MapRender CreateOverlay()
        {
            return new Overlay();
        }
    }
}
