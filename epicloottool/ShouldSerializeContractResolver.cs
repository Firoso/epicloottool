using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace epicloottool
{
    public class ShouldSerializeContractResolver : DefaultContractResolver
    {
        private static IComparer<string> comparer =new MagicItemEffectDefintionPropertyComparer();
        public static readonly ShouldSerializeContractResolver Instance = new ShouldSerializeContractResolver();

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            JsonProperty property = base.CreateProperty(member, memberSerialization);

            if (property.PropertyType != typeof(string))
            {
                if (property.PropertyType.GetInterface(nameof(IEnumerable)) != null)
                    property.ShouldSerialize =
                        instance => (instance?.GetType().GetProperty(property.UnderlyingName)?.GetValue(instance) as IEnumerable)?.OfType<object>().Count() > 0;
            }
            if (property.PropertyType.IsAssignableTo(typeof(IEnumerable)))
            {
                return property;
            }
            return base.CreateProperty(member, memberSerialization);
        }

        protected override System.Collections.Generic.IList<JsonProperty> CreateProperties(System.Type type, MemberSerialization memberSerialization)
        {
            return base.CreateProperties(type, memberSerialization).OrderBy(p => p.PropertyName, comparer).ToList();
        }
    }

    public class MagicItemEffectDefintionPropertyComparer : IComparer<string>
    {
        private static List<string> OrderedNames = new List<string> {
            "Id",
            "Type",
            "DisplayText",
            "Description",
            "CanBeAugmented",
            "Requirements",
            "ValuesPerRarity",
            "SelectionWeight",
            "Ability",
            "EquipFx",
            "Comment",
            "Prefixes",
            "Suffixes",
            "NoRoll",
            "ItemUsesStaminaOnAttack",
            "ItemHasBackstabBonus",
            "ItemHasArmor",
            "ItemHasNoParryPower",
            "ItemHasParryPower",
            "ItemHasBlockPower",
            "ItemHasNegativeMovementSpeedModifier",
            "ItemUsesDurability",
            "ItemHasPhysicalDamage",
            "ItemHasElementalDamage",
            "AllowedItemNames",
            "ExcludedSkillTypes",
            "AllowedSkillTypes",
            "ExcludedRarities",
            "AllowedRarities",
            "ExcludedItemTypes",
            "AllowedItemTypes",
            "ExclusiveEffectTypes",
            "ExclusiveSelf",
            "ExcludedItemNames"
        };

        public int Compare(string x, string y)
        {
            bool leftFound = OrderedNames.Contains(x);
            bool rightFound = OrderedNames.Contains(y);

            if (leftFound && rightFound)
            {
                return OrderedNames.IndexOf(x) - OrderedNames.IndexOf(y);
            }
            else if (leftFound)
            {
                return -1;
            }
            else if (rightFound)
            {
                return 1;
            }
            return 0;
        }
    }
}
