﻿using System.Collections.Generic;
using UnityEngine;
using Verse;
using RimWorld;

namespace D9Framework
{
    /// <summary>
    /// Only allows an object to be placed against a wall (Checks the cell behind this object)
    /// <para>Originally by CuproPanda, for Additional Joy Objects.</para>
    /// </summary>
    public class PlaceWorker_AgainstWall : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            IntVec3 c = loc - rot.FacingCell;                               // Get the tile behind this object
            Building edifice = c.GetEdifice(map);                           // Determine if the tile is an edifice            
            if (!c.InBounds(map) || !loc.InBounds(map)) return false;       // Don't place outside of the map
            if (!PlaceWorkerUtility.IsWall(edifice))// || (edifice.Faction != null || edifice.Faction != Faction.OfPlayer))    // Only allow placing on walls, and not if another faction owns the wall
                return new AcceptanceReport("D9F_MustBePlacedOnWall".Translate(checkingDef.LabelCap));
            return true;                                                    // Otherwise, accept placing
        }
    }
    /// <summary>
    /// Only allows an object to be placed on a wall, as long as there isn't the same def facing the same way.
    /// </summary>
    /// 
    public class PlaceWorker_OnWall : PlaceWorker
    {
        public override AcceptanceReport AllowsPlacing(BuildableDef checkingDef, IntVec3 loc, Rot4 rot, Map map, Thing thingToIgnore = null, Thing thing = null)
        {
            Building buil = loc.GetEdifice(map);
            if (!loc.InBounds(map)) return false;
            if (!PlaceWorkerUtility.IsWall(buil) || buil.Faction != Faction.OfPlayer) return new AcceptanceReport("D9F_MustBePlacedOnWall".Translate(checkingDef.LabelCap));
            if (PlaceWorkerUtility.ConflictingThing(checkingDef, loc, rot, map)) return new AcceptanceReport("IdenticalThingExists".Translate());
            return true;
        }

        public override bool ForceAllowPlaceOver(BuildableDef other)
        {
            return true;
        }
    }
}

