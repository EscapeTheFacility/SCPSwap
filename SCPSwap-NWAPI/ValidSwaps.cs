using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using PlayerRoles;
using PluginAPI.Core;
using SCPSwap_NWAPI.Models;

namespace SCPSwap_NWAPI
{
    /// <summary>
    /// Handles queries that expose the names of swappable classes and config blacklists.
    /// </summary>
    public static class ValidSwaps
    {
        private static readonly List<string> NamesValue = new List<string>();
        private static readonly List<RoleTypeId> DefaultSwapsValue = new List<RoleTypeId>();
        private static readonly Dictionary<string, RoleTypeId> TranslatableSwapsValue = new Dictionary<string, RoleTypeId>();

        /// <summary>
        /// Gets a collection of all available swap names.
        /// </summary>
        public static ReadOnlyCollection<string> Names => NamesValue.AsReadOnly();
        

        /// <summary>
        /// Gets a collection of all default swaps.
        /// </summary>
        public static ReadOnlyCollection<RoleTypeId> DefaultSwaps => DefaultSwapsValue.AsReadOnly();

        /// <summary>
        /// Gets a collection of all translatable swaps.
        /// </summary>
        public static ReadOnlyDictionary<string, RoleTypeId> TranslatableSwaps => new ReadOnlyDictionary<string, RoleTypeId>(TranslatableSwapsValue);

        /// <summary>
        /// Attempts to get a <see cref="RoleTypeId"/> from <see cref="Translation.TranslatableSwaps"/> or from directly parsing a request.
        /// </summary>
        /// <param name="request">The query to get the <see cref="RoleTypeId"/>.</param>
        /// <returns>The found <see cref="RoleTypeId"/>.</returns>
        public static RoleTypeId Get(string request)
        {
            if (TranslatableSwaps.TryGetValue(request, out RoleTypeId roleType))
                return roleType;

            if (Enum.TryParse(request, true, out roleType) && DefaultSwaps.Contains(roleType))
                return roleType;

            return RoleTypeId.None;
        }
        

        /// <summary>
        /// Clears and adds all available names of roles to swap to the cache.
        /// </summary>
        public static void Refresh()
        {
            NamesValue.Clear();
            
            RefreshTranslatableSwaps();
            RefreshDefaultSwaps();
        }

        private static void RefreshTranslatableSwaps()
        {
            TranslatableSwapsValue.Clear();
            if (Plugin.Instance.Messages.TranslatableSwaps == null)
                return;

            foreach (KeyValuePair<string, RoleTypeId> kvp in Plugin.Instance.Messages.TranslatableSwaps)
            {
                if ((Plugin.Instance.Config.BlacklistedScps != null && Plugin.Instance.Config.BlacklistedScps.Contains(kvp.Value))
                    || kvp.Value.GetTeam() != Team.SCPs)
                    continue;

                if (NamesValue.Contains(kvp.Key, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug($"Failed to add a translation that was a duplicate of another swap with the name of {kvp.Key}.", Plugin.Instance.Config.Debug);
                    continue;
                }

                TranslatableSwapsValue.Add(kvp.Key, kvp.Value);
                NamesValue.Add(kvp.Key);
            }
        }

        private static void RefreshDefaultSwaps()
        {
            DefaultSwapsValue.Clear();
            foreach (RoleTypeId role in Enum.GetValues(typeof(RoleTypeId)))
            {
                if ((Plugin.Instance.Config.BlacklistedScps != null && Plugin.Instance.Config.BlacklistedScps.Contains(role))
                    || role.GetTeam() != Team.SCPs)
                    continue;

                string roleText = role.ToString();
                if (NamesValue.Contains(roleText, StringComparison.OrdinalIgnoreCase))
                {
                    Log.Debug($"Failed to add a translation that was a duplicate of another swap with the name of {roleText}.", Plugin.Instance.Config.Debug);
                    continue;
                }

                DefaultSwapsValue.Add(role);
                NamesValue.Add(roleText);
            }
        }
    }
}