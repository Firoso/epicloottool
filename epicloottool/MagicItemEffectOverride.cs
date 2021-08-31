using System;
using System.Collections.Generic;

namespace epicloottool
{
    public class MagicItemEffectOverride : MagicItemEffectBase
    {
        public string TemplateId { get; set; }
        public string Id { get; set; }

        public void ApplyOverride(MagicItemEffectTemplate parentTemplate)
        {
            this.Type = ReconcileValue(this.Type, parentTemplate, p => p.Type, nameof(Type));
            this.NoRoll = ReconcileValue(this.NoRoll, parentTemplate, p => p.NoRoll, nameof(NoRoll));
            this.ItemUsesStaminaOnAttack = ReconcileValue(this.ItemUsesStaminaOnAttack, parentTemplate, p => p.ItemUsesStaminaOnAttack, nameof(ItemUsesStaminaOnAttack));
            this.ItemHasBackstabBonus = ReconcileValue(this.ItemHasBackstabBonus, parentTemplate, p => p.ItemHasBackstabBonus, nameof(ItemHasBackstabBonus));
            this.ItemHasArmor = ReconcileValue(this.ItemHasArmor, parentTemplate, p => p.ItemHasArmor, nameof(ItemHasArmor));
            this.ItemHasNoParryPower = ReconcileValue(this.ItemHasNoParryPower, parentTemplate, p => p.ItemHasNoParryPower, nameof(ItemHasNoParryPower));
            this.ItemHasParryPower = ReconcileValue(this.ItemHasParryPower, parentTemplate, p => p.ItemHasParryPower, nameof(ItemHasParryPower));
            this.ItemHasBlockPower = ReconcileValue(this.ItemHasBlockPower, parentTemplate, p => p.ItemHasBlockPower, nameof(ItemHasBlockPower));
            this.ItemHasNegativeMovementSpeedModifier = ReconcileValue(this.ItemHasNegativeMovementSpeedModifier, parentTemplate, p => p.ItemHasNegativeMovementSpeedModifier, nameof(ItemHasNegativeMovementSpeedModifier));
            this.ItemUsesDurability = ReconcileValue(this.ItemUsesDurability, parentTemplate, p => p.ItemUsesDurability, nameof(ItemUsesDurability));
            this.ItemHasPhysicalDamage = ReconcileValue(this.ItemHasPhysicalDamage, parentTemplate, p => p.ItemHasPhysicalDamage, nameof(ItemHasPhysicalDamage));
            this.ItemHasElementalDamage = ReconcileValue(this.ItemHasElementalDamage, parentTemplate, p => p.ItemHasElementalDamage, nameof(ItemHasElementalDamage));
            this.DisplayText = ReconcileValue(this.DisplayText, parentTemplate, p => p.DisplayText, nameof(DisplayText));
            this.Description = ReconcileValue(this.Description, parentTemplate, p => p.Description, nameof(Description));
            this.SelectionWeight = ReconcileValue(this.SelectionWeight, parentTemplate, p => p.SelectionWeight, nameof(SelectionWeight), merge: true, mergeResolver: SelectionWeightMultiplier);
            this.AllowedItemNames = ReconcileValue(this.AllowedItemNames, parentTemplate, p => p.AllowedItemNames, nameof(AllowedItemNames), merge: true, mergeResolver: ListMerge);
            this.ExcludedSkillTypes = ReconcileValue(this.ExcludedSkillTypes, parentTemplate, p => p.ExcludedSkillTypes, nameof(ExcludedSkillTypes), merge: true, mergeResolver: ListMerge);
            this.AllowedSkillTypes = ReconcileValue(this.AllowedSkillTypes, parentTemplate, p => p.AllowedSkillTypes, nameof(AllowedSkillTypes), merge: true, mergeResolver: ListMerge);
            this.ExcludedRarities = ReconcileValue(this.ExcludedRarities, parentTemplate, p => p.ExcludedRarities, nameof(ExcludedRarities), merge: true, mergeResolver: ListMerge);
            this.AllowedRarities = ReconcileValue(this.AllowedRarities, parentTemplate, p => p.AllowedRarities, nameof(AllowedRarities), merge: true, mergeResolver: ListMerge);
            this.ExcludedItemTypes = ReconcileValue(this.ExcludedItemTypes, parentTemplate, p => p.ExcludedItemTypes, nameof(ExcludedItemTypes), merge: true, mergeResolver: ListMerge);
            this.AllowedItemTypes = ReconcileValue(this.AllowedItemTypes, parentTemplate, p => p.AllowedItemTypes, nameof(AllowedItemTypes), merge: true, mergeResolver: ListMerge);
            this.ExclusiveEffectTypes = ReconcileValue(this.ExclusiveEffectTypes, parentTemplate, p => p.ExclusiveEffectTypes, nameof(ExclusiveEffectTypes), merge: true, mergeResolver: ListMerge);
            this.RequiredEffectTypes = ReconcileValue(this.RequiredEffectTypes, parentTemplate, p => p.RequiredEffectTypes, nameof(RequiredEffectTypes), merge: true, mergeResolver: ListMerge);
            this.ExcludedItemNames = ReconcileValue(this.ExcludedItemNames, parentTemplate, p => p.ExcludedItemNames, nameof(ExcludedItemNames), merge: true, mergeResolver: ListMerge);
            this.ValuesPerRarity = ReconcileValue(this.ValuesPerRarity, parentTemplate, p => p.ValuesPerRarity, nameof(ValuesPerRarity));
            this.Prefixes = ReconcileValue(this.Prefixes, parentTemplate, p => p.Prefixes, nameof(Prefixes), merge: true, mergeResolver: ListMerge);
            this.Suffixes = ReconcileValue(this.Suffixes, parentTemplate, p => p.Suffixes, nameof(Suffixes), merge: true, mergeResolver: ListMerge);
            this.EquipFx = ReconcileValue(this.EquipFx, parentTemplate, p => p.EquipFx, nameof(EquipFx));
        }

        private T ReconcileValue<T>(T overrideValue, MagicItemEffectTemplate parentTemplate, Func<MagicItemEffectTemplate, T> parentValueResolver, string propertyName, bool merge = false, Func<IEnumerable<T>, T, T> mergeResolver = null)
        {
            var parentValue = parentValueResolver(parentTemplate);

            if (parentValue == null || parentValue.Equals(default(T)))
            {
                return overrideValue;
            }

            if (overrideValue == null || overrideValue.Equals(default(T)))
            {
                return parentValue;
            }

            if (merge == true)
            {
                if (mergeResolver == null)
                {
                    ConsoleLogger.Error($"A merge resolver does not exist for {propertyName}.  This is undefined behavior and the MagicItemEffect cannot be resolved.");
                    return overrideValue;
                }

                var result = mergeResolver(new[] { parentValue }, overrideValue);
                ConsoleLogger.Warn($"Merge was resolved to use '{result}'");
                return result;
            }
            else
            {
                return overrideValue;
            }
                        
        }
    }
}
