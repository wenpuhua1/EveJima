﻿using System;
using System.Collections.Generic;
using System.Linq;
using EveJimaCore.BLL.Navigator;
using EveJimaUniverse;
using log4net;

namespace EveJimaCore.BLL
{
    public class PathFinder
    {
        private static readonly ILog Log = LogManager.GetLogger("All");
        private readonly UniverseEntity _universeEntity;

        public PathFinder(UniverseEntity universeEntity)
        {
            _universeEntity = universeEntity;
        }

        public List<Path> GetPathes(List<Bookmark> bookmarks, string locationSystemName, int range)
        {
            var solarSystem = _universeEntity.GetSystemByName(locationSystemName);

            var linkedSolarSystems = GetSystems(_universeEntity, solarSystem.Id, range, 0, new List<Tuple<string, int>> { new Tuple<string, int>(solarSystem.Id, 0) });

            return GetBookmarks(bookmarks, linkedSolarSystems);
        }

        private List<Path> GetBookmarks(IEnumerable<Bookmark> bookmarks, List<Tuple<string, int>> systems)
        {
            var pathes = new List<Path>();

            foreach (var bookmark in bookmarks)
            {
                foreach(var system in systems)
                {
                    var systemName = system.Item1;
                    var jumps = system.Item2;

                    if (bookmark.SystemId != systemName) continue;

                    try
                    {
                        var path = new Path
                        {
                            Name = bookmark.Name,
                            Note = bookmark.Note,
                            SystemName = _universeEntity.GetSystemById(systemName).Name,
                            ShipKills = "0",
                            NpcKills = "0",
                            PodKills = "0",
                            Jumps = jumps
                        };

                        var kills = EsiApi.GetSystemKills(systemName);

                        if (kills != null)
                        {
                            path.ShipKills = kills.Item1;
                            path.NpcKills = kills.Item2;
                            path.PodKills = kills.Item3;
                        }

                        pathes.Add(path);
                    }
                    catch (Exception ex)
                    {
                        Log.ErrorFormat("[PathFinder.GetBookmarks] Critical error. Exception {0}", ex);
                    }
                }
            }

            return pathes;
        }

        public List<Tuple<string, int>> GetSystems(EveJimaUniverse.UniverseEntity universeEntity, string solarSystemId, int range, int currentRange, List<Tuple<string, int>> systems)
        {
            currentRange = currentRange + 1;

            if (currentRange > range) return systems;

            var linkedSystems = universeEntity.GetLinkedSystems(solarSystemId);

            if(linkedSystems == null) return systems;

            foreach (var system in linkedSystems.LinkedSystems)
            {
                var linkedSystem = universeEntity.GetSystemById(system);

                if(systems.Any(m => m.Item1 == linkedSystem.Id)) continue;

                systems.Add(new Tuple<string, int>(linkedSystem.Id, currentRange));

                GetSystems(universeEntity, linkedSystem.Id, range, currentRange, systems);
            }

            return systems;
        }
    }
}
