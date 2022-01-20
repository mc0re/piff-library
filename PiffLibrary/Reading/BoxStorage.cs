using PiffLibrary.Boxes;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


namespace PiffLibrary.Infrastructure
{
    internal sealed class BoxStorage
    {
        #region Fields

        /// <summary>
        /// A box-name-to-box-types relation.
        /// Each box can have multiple names.
        /// And the same name can be used by more than one box
        /// (then children relationship comes to rescue).
        /// </summary>
        private readonly ConcurrentDictionary<string, List<Type>> sBoxNames = new ConcurrentDictionary<string, List<Type>>();


        /// <summary>
        /// Expected root-level box types.
        /// </summary>
        private readonly HashSet<Type> sRootBoxes = new HashSet<Type>();


        /// <summary>
        /// Mapping between box type and expected children types.
        /// Unexpected types are reported as warnings.
        /// </summary>
        private readonly Dictionary<Type, HashSet<Type>> sChildBoxes = new Dictionary<Type, HashSet<Type>>();

        #endregion


        #region API

        /// <summary>
        /// Traverse this assembly looking for box types.
        /// </summary>
        public void Collect()
        {
            foreach (var type in Assembly.GetExecutingAssembly().GetTypes())
            {
                var boxNameAttrs = type.GetCustomAttributes<BoxNameAttribute>();
                if (!boxNameAttrs.Any()) continue;

                if (!typeof(PiffBoxBase).IsAssignableFrom(type))
                    throw new ArgumentException($"A box must inherit from {nameof(PiffBoxBase)}. {type.Name} does not.");

                AddToBoxCollection(type, boxNameAttrs);
                ProcessChildren(type);
            }

            var rootTypes = typeof(PiffFile)
                .GetCustomAttributes<ChildTypeAttribute>()
                .Select(t => t.ChildType)
                .ToArray();

            foreach (var child in rootTypes)
            {
                if (!typeof(PiffBoxBase).IsAssignableFrom(child))
                    throw new ArgumentException($"A root box must inherit from {nameof(PiffBoxBase)}. {child.Name} does not.");
                sRootBoxes.Add(child);
            }

        }


        /// <summary>
        /// Find a box by its name.
        /// </summary>
        public FindBoxResults FindBox(Type parentType, string name, out Type boxType)
        {
            if (!sBoxNames.TryGetValue(name, out var matchingList))
            {
                boxType = typeof(PiffCatchAllBox);
                return FindBoxResults.Unrecognized;
            }

            var expectedChildren =
                parentType is null ? sRootBoxes :
                sChildBoxes.TryGetValue(parentType, out var childList) ? childList :
                Enumerable.Empty<Type>();
            var match = matchingList.Where(b => expectedChildren.Contains(b)).ToList();

            switch (match.Count)
            {
                case 0:
                    boxType = matchingList.First();
                    return FindBoxResults.Unexpected;

                case 1:
                    boxType = match.First();
                    return FindBoxResults.Found;

                default:
                    boxType = match.First();
                    return FindBoxResults.Ambiguous;
            }
        }

        #endregion


        #region Utility

        private void AddToBoxCollection(Type type, IEnumerable<BoxNameAttribute> boxNameAttrs)
        {
            foreach (var name in boxNameAttrs)
            {
                sBoxNames.AddOrUpdate(name.Name,
                                      _ => new List<Type> { type },
                                      (_, lst) => { lst.Add(type); return lst; });
            }
        }


        private void ProcessChildren(Type type)
        {
            var children = type.GetCustomAttributes<ChildTypeAttribute>();
            if (children.Any())
            {
                var childTypes = children.Select(t => t.ChildType).ToList();

                foreach (var child in childTypes)
                {
                    if (!typeof(PiffBoxBase).IsAssignableFrom(child))
                        throw new ArgumentException($"A child box must inherit from {nameof(PiffBoxBase)}. {child.Name} does not.");
                }

                sChildBoxes.Add(type, new HashSet<Type>(childTypes));
            }
        }

        #endregion
    }
}
