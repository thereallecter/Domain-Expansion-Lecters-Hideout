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
        internal static readonly TileType GreyClosed = new TileType(0x05, "Grey Closed");
        internal static readonly TileType Composite = new TileType(0x00fd, "Composite");
        internal static readonly TileType Space = new TileType(0x00fe, "Space");

        internal static readonly ObjectType CaveWall = new ObjectType(0x01ce, "Cave Wall");
        internal static readonly ObjectType CowardicePortal = new ObjectType(0x0703, "Portal of Cowardice");
        internal static readonly ObjectType Septavius = new ObjectType(0x0d90, "Septavius the Ghost God");

        internal static readonly ObjectType[] Boss = {
            new ObjectType(0x0d95, "Lair Skeleton King"),
            new ObjectType(0x0d93, "Lair Skeleton Veteran"),

            new ObjectType(0x0d97, "Lair Mummy King"),

            new ObjectType(0x0da2, "Lair Burst Trap"),
            new ObjectType(0x0da3, "Lair Ghost Bat"),
        };

        internal static readonly ObjectType[] Minion = {
            new ObjectType(0x0d91, "Lair Skeleton"),
            new ObjectType(0x0d92, "Lair Skeleton Swordsman"),
            new ObjectType(0x0d93, "Lair Skeleton Veteran"),
            new ObjectType(0x0d94, "Lair Skeleton Mage"),

            new ObjectType(0x0d96, "Lair Mummy"),
            new ObjectType(0x0d98, "Lair Mummy Pharaoh"),
            new ObjectType(0x0d93, "Lair Skeleton Veteran"),
            new ObjectType(0x0d94, "Lair Skeleton Mage"),

            new ObjectType(0x0d9e, "Lair Big Black Slime"),
            new ObjectType(0x0d9c, "Lair Medium Black Slime"),

            new ObjectType(0x0d9e, "Lair Big Brown Slime"),
            new ObjectType(0x0d9f, "Lair Skeleton Mage")
        };

        internal static readonly ObjectType[] Pet = {
            new ObjectType(0x0d99, "Lair Construct Giant"),
            new ObjectType(0x0d9a, "Lair Construct Titan"),

            new ObjectType(0x0d9d, "Lair Little Black Slime"),
            new ObjectType(0x0d9f, "Lair Little Brown Slime"),

            new ObjectType(0x0da0, "Lair Brown Bat"),
            new ObjectType(0x0da1, "Lair Ghost Bat"),
        };

        public override int MaxDepth
        { get { return 12; } }

        private NormDist targetDepth;
        public override NormDist TargetDepth
        { get { return targetDepth; } }

        public override NormDist SpecialRmCount
        { get { return null; } }

        public override NormDist SpecialRmDepthDist
        { get { return null; } }

        public override Range RoomSeparation
        { get { return new Range(3, 7); } }

        public override int CorridorWidth
        { get { return 2; } }

        public override void Initialize()
        {
            targetDepth = new NormDist(2, 8, 10, 12, Rand.Next());
        }

        public override Room CreateStart(int depth)
        {
            return new StartRoom(10, 13);
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
            return new NormalRoom(Rand.Next(10, 13), Rand.Next(10, 13));
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
