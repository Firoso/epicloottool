using EpicLoot;
using System.Collections.Generic;
using System.Linq;
using static EpicLoot.MagicItemEffectDefinition;

namespace epicloottool
{
    public abstract class MagicItemEffectBase
    {
        public virtual string Type { get; set; } = null;
        public virtual bool? NoRoll { get; set; } = null;
        public virtual bool? ItemUsesStaminaOnAttack { get; set; } = null;
        public virtual bool? ItemHasBackstabBonus { get; set; } = null;
        public virtual bool? ItemHasArmor { get; set; } = null;
        public virtual bool? ItemHasNoParryPower { get; set; } = null;
        public virtual bool? ItemHasParryPower { get; set; } = null;
        public virtual bool? ItemHasBlockPower { get; set; } = null;
        public virtual bool? ItemHasNegativeMovementSpeedModifier { get; set; } = null;
        public virtual bool? ItemUsesDurability { get; set; } = null;
        public virtual bool? ItemHasPhysicalDamage { get; set; } = null;
        public virtual bool? ItemHasElementalDamage { get; set; } = null;
        public virtual List<string> AllowedItemNames { get; set; } = null;
        public virtual List<Skills.SkillType> ExcludedSkillTypes { get; set; } = null;
        public virtual List<Skills.SkillType> AllowedSkillTypes { get; set; } = null;
        public virtual List<ItemRarity> ExcludedRarities { get; set; } = null;
        public virtual List<ItemRarity> AllowedRarities { get; set; } = null;
        public virtual List<ItemDrop.ItemData.ItemType> ExcludedItemTypes { get; set; } = null;
        public virtual List<ItemDrop.ItemData.ItemType> AllowedItemTypes { get; set; } = null;
        public virtual List<string> ExclusiveEffectTypes { get; set; } = null;
        public virtual List<string> ExcludedItemNames { get; set; } = null;
        public virtual ValuesPerRarityDef ValuesPerRarity { get; set; } = null;
        public virtual string DisplayText { get; set; } = null;
        public virtual string Description { get; set; } = null;
        public virtual float? SelectionWeight { get; set; } = null;

        protected ValuesPerRarityDef ValuesPerRarityMerge(IEnumerable<ValuesPerRarityDef> values, ValuesPerRarityDef currentValue)
        {
            var mergeValues = currentValue == default(ValuesPerRarityDef) ? values : values.Append(currentValue);

            if (mergeValues.Count() == 0)
            {
                return default(ValuesPerRarityDef);
            }

            return new ValuesPerRarityDef
            {
                Magic = new ValueDef
                {
                    MinValue = mergeValues.Average(v => v.Magic.MinValue),
                    MaxValue = mergeValues.Average(v => v.Magic.MaxValue),
                    Increment = mergeValues.Average(v => v.Magic.Increment)
                },
                Rare = new ValueDef
                {
                    MinValue = mergeValues.Average(v => v.Rare.MinValue),
                    MaxValue = mergeValues.Average(v => v.Rare.MaxValue),
                    Increment = mergeValues.Average(v => v.Rare.Increment)
                },
                Epic = new ValueDef
                {
                    MinValue = mergeValues.Average(v => v.Epic.MinValue),
                    MaxValue = mergeValues.Average(v => v.Epic.MaxValue),
                    Increment = mergeValues.Average(v => v.Epic.Increment)
                },
                Legendary = new ValueDef
                {
                    MinValue = mergeValues.Average(v => v.Legendary.MinValue),
                    MaxValue = mergeValues.Average(v => v.Legendary.MaxValue),
                    Increment = mergeValues.Average(v => v.Legendary.Increment)
                },
            };
        }

        protected List<T> ListMerge<T>(IEnumerable<List<T>> values, List<T> currentValue)
        {
            var mergedList = currentValue ?? new List<T>();

            foreach (var v in values)
            {
                foreach (var e in v)
                {
                    if (!mergedList.Contains(e))
                    {
                        mergedList.Add(e);
                    }
                }
            }

            return mergedList;
        }

        protected float? SelectionWeightMultiplier(IEnumerable<float?> values, float? currentValue)
        {
            var aggregation = currentValue ?? 1;
            foreach (var v in values)
            {
                aggregation *= v.Value;
            }

            return aggregation;
        }

        protected bool? BooleanMerge(IEnumerable<bool?> values, bool? currentValue)
        {
            var allFalse = values.All(v => !v.HasValue || v.Value == false);
            var anyTrue = values.Any(v => v.HasValue && v.Value == true);

            return allFalse ? false : anyTrue ? true : currentValue;
        }
    }
}