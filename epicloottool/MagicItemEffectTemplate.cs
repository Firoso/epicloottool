using System;
using System.Collections.Generic;
using System.Linq;
using static EpicLoot.MagicItemEffectDefinition;

namespace epicloottool
{
    public class MagicItemEffectTemplate : MagicItemEffectBase
    {
        public List<string> ParentTemplateIds { get; set; } = null;
        public string TemplateId { get; set; }

        [NonSerialized]
        public bool IsReconciled = false;

        [NonSerialized]
        public List<MagicItemEffectTemplate> ParentTemplates = null;

        public virtual ValuesPerRarityDef ValuesPerRarityAdditive { get; set; } = null;
        public virtual ValuesPerRarityDef ValuesPerRarityMultiplicative { get; set; } = null;

        public bool IsRootTemplate => ParentTemplateIds == null || ParentTemplateIds.Count == 0;

        public bool ReconcileTemplate(Dictionary<string, MagicItemEffectTemplate> knownTemplates)
        {
            // no reconciliation needed, this is a root template.
            if (this.IsRootTemplate)
            {
                this.IsReconciled = true;
                return true;
            }

            // if we have all the required parent templates
            List<MagicItemEffectTemplate> confirmedParents = new List<MagicItemEffectTemplate>();

            foreach (var parentId in ParentTemplateIds)
            {
                if (knownTemplates.ContainsKey(parentId))
                {
                    confirmedParents.Add(knownTemplates[parentId]);
                }
                else
                {
                    ConsoleLogger.Error($"Could not find a TemplateId {parentId} specified as a parent of {TemplateId}");
                    return false;
                }
            }

            this.Type = ReconcileValue(this.Type, confirmedParents, p => p.Type, nameof(Type));
            this.NoRoll = ReconcileValue(this.NoRoll, confirmedParents, p => p.NoRoll, nameof(NoRoll));
            this.ItemUsesStaminaOnAttack = ReconcileValue(this.ItemUsesStaminaOnAttack, confirmedParents, p => p.ItemUsesStaminaOnAttack, nameof(ItemUsesStaminaOnAttack), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasBackstabBonus = ReconcileValue(this.ItemHasBackstabBonus, confirmedParents, p => p.ItemHasBackstabBonus, nameof(ItemHasBackstabBonus), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasArmor = ReconcileValue(this.ItemHasArmor, confirmedParents, p => p.ItemHasArmor, nameof(ItemHasArmor), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasNoParryPower = ReconcileValue(this.ItemHasNoParryPower, confirmedParents, p => p.ItemHasNoParryPower, nameof(ItemHasNoParryPower), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasParryPower = ReconcileValue(this.ItemHasParryPower, confirmedParents, p => p.ItemHasParryPower, nameof(ItemHasParryPower), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasBlockPower = ReconcileValue(this.ItemHasBlockPower, confirmedParents, p => p.ItemHasBlockPower, nameof(ItemHasBlockPower), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasNegativeMovementSpeedModifier = ReconcileValue(this.ItemHasNegativeMovementSpeedModifier, confirmedParents, p => p.ItemHasNegativeMovementSpeedModifier, nameof(ItemHasNegativeMovementSpeedModifier), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemUsesDurability = ReconcileValue(this.ItemUsesDurability, confirmedParents, p => p.ItemUsesDurability, nameof(ItemUsesDurability), allowConflict: true, conflictResolver: BooleanMerge);
            this.ItemHasPhysicalDamage = ReconcileValue(this.ItemHasPhysicalDamage, confirmedParents, p => p.ItemHasPhysicalDamage, nameof(ItemHasPhysicalDamage), allowConflict: true, conflictResolver: BooleanMerge);
            this.DisplayText = ReconcileValue(this.DisplayText, confirmedParents, p => p.DisplayText, nameof(DisplayText));
            this.Description = ReconcileValue(this.Description, confirmedParents, p => p.Description, nameof(Description));
            this.SelectionWeight = ReconcileValue(this.SelectionWeight, confirmedParents, p => p.SelectionWeight, nameof(SelectionWeight), allowConflict: true, conflictResolver: SelectionWeightMultiplier);
            this.AllowedItemNames = ReconcileValue(this.AllowedItemNames, confirmedParents, p => p.AllowedItemNames, nameof(AllowedItemNames), allowConflict: true, conflictResolver: ListMerge);
            this.ExcludedSkillTypes = ReconcileValue(this.ExcludedSkillTypes, confirmedParents, p => p.ExcludedSkillTypes, nameof(ExcludedSkillTypes), allowConflict: true, conflictResolver: ListMerge);
            this.AllowedSkillTypes = ReconcileValue(this.AllowedSkillTypes, confirmedParents, p => p.AllowedSkillTypes, nameof(AllowedSkillTypes), allowConflict: true, conflictResolver: ListMerge);
            this.ExcludedRarities = ReconcileValue(this.ExcludedRarities, confirmedParents, p => p.ExcludedRarities, nameof(ExcludedRarities), allowConflict: true, conflictResolver: ListMerge);
            this.AllowedRarities = ReconcileValue(this.AllowedRarities, confirmedParents, p => p.AllowedRarities, nameof(AllowedRarities), allowConflict: true, conflictResolver: ListMerge);
            this.ExcludedItemTypes = ReconcileValue(this.ExcludedItemTypes, confirmedParents, p => p.ExcludedItemTypes, nameof(ExcludedItemTypes), allowConflict: true, conflictResolver: ListMerge);
            this.AllowedItemTypes = ReconcileValue(this.AllowedItemTypes, confirmedParents, p => p.AllowedItemTypes, nameof(AllowedItemTypes), allowConflict: true, conflictResolver: ListMerge);
            this.ExclusiveEffectTypes = ReconcileValue(this.ExclusiveEffectTypes, confirmedParents, p => p.ExclusiveEffectTypes, nameof(ExclusiveEffectTypes), allowConflict: true, conflictResolver: ListMerge);
            this.RequiredEffectTypes = ReconcileValue(this.RequiredEffectTypes, confirmedParents, p => p.RequiredEffectTypes, nameof(RequiredEffectTypes), allowConflict: true, conflictResolver: ListMerge);
            this.ExcludedItemNames = ReconcileValue(this.ExcludedItemNames, confirmedParents, p => p.ExcludedItemNames, nameof(ExcludedItemNames), allowConflict: true, conflictResolver: ListMerge);
            this.ValuesPerRarity = ReconcileValue(this.ValuesPerRarity, confirmedParents, p => p.ValuesPerRarity, nameof(ValuesPerRarity), allowConflict: true, conflictResolver: ValuesPerRarityMerge);
            this.Prefixes = ReconcileValue(this.Prefixes, confirmedParents, p => p.Prefixes, nameof(Prefixes), allowConflict: true, conflictResolver: ListMerge);
            this.Suffixes = ReconcileValue(this.Suffixes, confirmedParents, p => p.Suffixes, nameof(Suffixes), allowConflict: true, conflictResolver: ListMerge);
            this.EquipFx = ReconcileValue(this.EquipFx, confirmedParents, p => p.EquipFx, nameof(EquipFx));
            this.IsReconciled = true;
            this.ParentTemplates = new List<MagicItemEffectTemplate>(confirmedParents);
            
            return true;
        }

        private T ReconcileValue<T>(T currentValue, IEnumerable<MagicItemEffectTemplate> parents, Func<MagicItemEffectTemplate, T> parentValueResolver, string propertyName, bool allowConflict = false, Func<IEnumerable<T>, T, T> conflictResolver = null)
        {
            // Check if no reconciliation required.
            if (parents.Count() == 0)
            {
                return currentValue;
            }

            var parentValues = parents.Select(parentValueResolver).Where(v => v != null && !v.Equals(default(T))).ToArray();

            //We have values to reconcile.
            if (parentValues.Count() > 1)
            {
                if (allowConflict == true)
                {
                    ConsoleLogger.Warn($"A conflict was found while attempting to resolve {propertyName}.  Found multiple values [{String.Join(", ", parentValues.Select(v => v?.ToString()).Append(currentValue?.ToString()))}]");
                    if (conflictResolver == null)
                    {
                        ConsoleLogger.Error($"A conflict resolver does not exist for {propertyName}.  This is undefined behavior and the template cannot be resolved.  Using current value, but this may be in error.");
                        return currentValue;
                    }

                    var result = conflictResolver(parentValues, currentValue);
                    ConsoleLogger.Warn($"Conflict was resolved to use '{result}'");
                    return result;
                }
                else
                {
                    ConsoleLogger.Error($"A conflict was found while attempting to resolve {propertyName}.  Found multiple values [{String.Join(", ", parentValues.Select(v => v?.ToString()).Append(currentValue?.ToString()))}].  This is undefined behavior and the template cannot be resolved.'");
                    return currentValue;
                }
            }

            return currentValue != null && !currentValue.Equals(default(T)) ? currentValue : parentValues.SingleOrDefault();
        }
    }
}
