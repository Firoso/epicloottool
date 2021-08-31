using System.Collections.Generic;
using System.IO;
using System.Linq;
using YamlDotNet.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using EpicLoot;
using static ItemDrop.ItemData;

namespace epicloottool
{
    class Program
    {
        static Deserializer deserializer = new Deserializer();
        static ISerializer serializer = new SerializerBuilder().WithIndentedSequences().ConfigureDefaultValuesHandling(DefaultValuesHandling.OmitDefaults | DefaultValuesHandling.OmitEmptyCollections | DefaultValuesHandling.OmitNull).Build();
        static void Main(string[] args)
        {
            var currentDirectory = new DirectoryInfo(@".\");

            var templates = new Dictionary<string, MagicItemEffectTemplate>();
            var overrides = new Dictionary<string, MagicItemEffectOverride>();

            // Load Templates
            foreach (var file in currentDirectory.GetFiles(@"configs\templates\*.yaml", SearchOption.AllDirectories))
            {
                ConsoleLogger.Log($"Processing Templates from {file.FullName}");
                var templateSet = deserializer.Deserialize<List<MagicItemEffectTemplate>>(File.ReadAllText(file.FullName));
                foreach (var t in templateSet)
                {
                    VerifyTemplate(t, file);
                    ConsoleLogger.Log($"Discovered Template {t.TemplateId}");
                    templates.Add(t.TemplateId, t);
                }
            }

            // Load Overrides
            foreach (var file in currentDirectory.GetFiles(@"configs\overrides\*.yaml", SearchOption.AllDirectories))
            {
                ConsoleLogger.Log($"Processing Overrides from {file.FullName}");
                var overrideSet = deserializer.Deserialize<List<MagicItemEffectOverride>>(File.ReadAllText(file.FullName)) ?? new List<MagicItemEffectOverride>();
                foreach (var o in overrideSet)
                {
                    VerifyOverride(o, file);
                    ConsoleLogger.Log($"Discovered Override {o.Id}");
                    overrides.Add(o.Id, o);
                }
            }

            // ReconcileTemplates
            // Get Root Templates
            var processedTemplates = new Dictionary<string, MagicItemEffectTemplate>();
            ReconcileTemplates(new Dictionary<string, MagicItemEffectTemplate>(templates), processedTemplates);

            var overrideValues = overrides.Values.ToList();
            var processedTemplateValues = processedTemplates.Values.ToList();
            var effects = BuildDefinitions(overrideValues, processedTemplateValues);

            var finalOutput = JsonConvert.SerializeObject(
               effects.OrderBy(e => e.Type).ThenBy(e => e.Id),
               new JsonSerializerSettings
               {
                   Formatting = Formatting.Indented,
                   NullValueHandling = NullValueHandling.Ignore,
                   DefaultValueHandling = DefaultValueHandling.Ignore,
                   ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
                   Converters = new List<JsonConverter> { new StringEnumConverter() }
               });

            ConsoleLogger.Log("--- WRITING TO OUTPUT FILE: magiceffects.json---");
            ConsoleLogger.Log(finalOutput);

            File.WriteAllText("magiceffects.json", finalOutput);
            ConsoleLogger.Log($"Wrote to {new FileInfo("magiceffects.json").FullName}");
        }

        private static IEnumerable<MagicItemEffectDefinition> BuildDefinitions(List<MagicItemEffectOverride> overrides, List<MagicItemEffectTemplate> processedTemplates)
        {
            foreach (var o in overrides)
            {
                ConsoleLogger.Log($"Applying effect override {o.Id} against template {o.TemplateId}");
                var parentTemplate = processedTemplates.Single(t => t.TemplateId.Equals(o.TemplateId));
                o.ApplyOverride(parentTemplate);

                var effect = new MagicItemEffectDefinition()
                {
                    Id = o.Id,
                    Type = o.Type,
                    Requirements = new MagicItemEffectRequirements
                    {
                        NoRoll = o.NoRoll ?? false,
                        ItemUsesStaminaOnAttack = o.ItemUsesStaminaOnAttack ?? false,
                        ItemHasBackstabBonus = o.ItemHasBackstabBonus ?? false,
                        ItemHasArmor = o.ItemHasArmor ?? false,
                        ItemHasNoParryPower = o.ItemHasNoParryPower ?? false,
                        ItemHasParryPower = o.ItemHasParryPower ?? false,
                        ItemHasBlockPower = o.ItemHasBlockPower ?? false,
                        ItemHasNegativeMovementSpeedModifier = o.ItemHasNegativeMovementSpeedModifier ?? false,
                        ItemUsesDurability = o.ItemUsesDurability ?? false,
                        ItemHasPhysicalDamage = o.ItemHasPhysicalDamage ?? false,
                        ItemHasElementalDamage = o.ItemHasElementalDamage ?? false,
                        AllowedItemNames = o.AllowedItemNames ?? new List<string>(),
                        ExcludedSkillTypes = o.ExcludedSkillTypes ?? new List<Skills.SkillType>(),
                        AllowedSkillTypes = o.AllowedSkillTypes ?? new List<Skills.SkillType>(),
                        ExcludedRarities = o.ExcludedRarities ?? new List<ItemRarity>(),
                        AllowedRarities = o.AllowedRarities ?? new List<ItemRarity>(),
                        ExcludedItemTypes = o.ExcludedItemTypes ?? new List<ItemType>(),
                        AllowedItemTypes = o.AllowedItemTypes ?? new List<ItemType>(),
                        ExclusiveEffectTypes = o.ExclusiveEffectTypes ?? new List<string>(),
                        ExcludedItemNames = o.ExcludedItemNames ?? new List<string>()
                    },
                    DisplayText = o.DisplayText,
                    Description = o.Description,
                    SelectionWeight = o.SelectionWeight ?? 1.0f,
                    ValuesPerRarity = o.ValuesPerRarity,
                    Prefixes = o.Prefixes ?? new List<string>(),
                    Suffixes = o.Suffixes ?? new List<string>(),
                    EquipFx = o.EquipFx ?? "",
                    EquipFxMode = FxAttachMode.None
                };
                VerifyDefinition(effect);
                yield return effect;
            }
        }

        private static void ReconcileTemplates(Dictionary<string, MagicItemEffectTemplate> unprocessedTemplates, Dictionary<string, MagicItemEffectTemplate> processedTemplates)
        {
            // process any Roots
            var rootTemplates = unprocessedTemplates.Values.Where(t => t.IsRootTemplate);
            if (rootTemplates.Any())
            {
                foreach (var rootTemplate in rootTemplates)
                {
                    ConsoleLogger.Log($"Reconciling Root Template {rootTemplate.TemplateId}");
                    if (rootTemplate.ReconcileTemplate(null) == true)
                    {
                        unprocessedTemplates.Remove(rootTemplate.TemplateId);
                        processedTemplates.Add(rootTemplate.TemplateId, rootTemplate);
                    }
                    else
                    {
                        ConsoleLogger.Warn($"Failed to reconcile {rootTemplate.TemplateId}");
                    }
                }
            }

            bool done = false;
            // iteratively process any unprocessedTemplates we can satisfy roots for
            while (!done && unprocessedTemplates.Count != 0)
            {
                var processedTemplateIds = processedTemplates.Values.Select(pt => pt.TemplateId).ToArray();
                var candidateTemplates = unprocessedTemplates.Values.Where(t => t.ParentTemplateIds.All(ptid => processedTemplateIds.Contains(ptid))).ToArray();
                ConsoleLogger.Log($"Reconciling {candidateTemplates.Count()} candidate templates");

                if (candidateTemplates.Any())
                {
                    foreach (var candidateTemplate in candidateTemplates)
                    {
                        ConsoleLogger.Log($"Reconciling Template {candidateTemplate.TemplateId}");
                        if (candidateTemplate.ReconcileTemplate(processedTemplates) == true)
                        {
                            unprocessedTemplates.Remove(candidateTemplate.TemplateId);
                            processedTemplates.Add(candidateTemplate.TemplateId, candidateTemplate);
                        }
                        else
                        {
                            ConsoleLogger.Warn($"Failed to reconcile {candidateTemplate.TemplateId}");
                        }
                    }
                }
                else
                {
                    done = true;
                }
            }
        }

        private static void VerifyTemplate(MagicItemEffectTemplate t, FileInfo file)
        {
            if (String.IsNullOrWhiteSpace(t.TemplateId))
            {
                throw new InvalidDataException($"TemplateId was null or whitespace when processing a template from {file.FullName}");
            }
        }

        private static void VerifyOverride(MagicItemEffectOverride o, FileInfo file)
        {
            if (String.IsNullOrWhiteSpace(o.TemplateId))
            {
                throw new InvalidDataException($"TemplateId was null or whitespace when processing {o.Id} from {file.FullName}");
            }

            if (String.IsNullOrWhiteSpace(o.Id))
            {
                throw new InvalidDataException($"Id was null or whitespace when processing an override from {file.FullName}");
            }
        }

        private static void VerifyDefinition(MagicItemEffectDefinition d)
        {
            if (String.IsNullOrWhiteSpace(d.Description))
            {
                throw new InvalidDataException($"Description was null or whitespace for definition {d.Id}");
            }

            if (String.IsNullOrWhiteSpace(d.DisplayText))
            {
                throw new InvalidDataException($"DisplayText was null or whitespace for definition {d.Id}");
            }

            if (String.IsNullOrWhiteSpace(d.Type))
            {
                throw new InvalidDataException($"Type was null or whitespace for definition {d.Id}");
            }
        }

        //private static MagicItemEffectDefinition OverrideApplyOverride(MagicItemEffectDefinition baseDefinition, MagicItemEffectOverride @override)
        //{
        //   var effect = new MagicItemEffectDefinition();
        //   effect.Id = $"{baseDefinition.Id}.{@override.Id}";
        //   effect.Type = baseDefinition.Type;
        //   effect.Requirements.NoRoll = @override.NoRoll ?? baseDefinition.Requirements.NoRoll;
        //   effect.Requirements.ItemUsesStaminaOnAttack = @override.ItemUsesStaminaOnAttack ?? baseDefinition.Requirements.ItemUsesStaminaOnAttack;
        //   effect.Requirements.ItemHasBackstabBonus = @override.ItemHasBackstabBonus ?? baseDefinition.Requirements.ItemHasBackstabBonus;
        //   effect.Requirements.ItemHasArmor = @override.ItemHasArmor ?? baseDefinition.Requirements.ItemHasArmor;
        //   effect.Requirements.ItemHasNoParryPower = @override.ItemHasNoParryPower ?? baseDefinition.Requirements.ItemHasNoParryPower;
        //   effect.Requirements.ItemHasParryPower = @override.ItemHasParryPower ?? baseDefinition.Requirements.ItemHasParryPower;
        //   effect.Requirements.ItemHasBlockPower = @override.ItemHasBlockPower ?? baseDefinition.Requirements.ItemHasBlockPower;
        //   effect.Requirements.ItemHasNegativeMovementSpeedModifier = @override.ItemHasNegativeMovementSpeedModifier ?? baseDefinition.Requirements.ItemHasNegativeMovementSpeedModifier;
        //   effect.Requirements.ItemUsesDurability = @override.ItemUsesDurability ?? baseDefinition.Requirements.ItemUsesDurability;
        //   effect.Requirements.ItemHasPhysicalDamage = @override.ItemHasPhysicalDamage ?? baseDefinition.Requirements.ItemHasPhysicalDamage;
        //   effect.DisplayText = @override.DisplayText ?? baseDefinition.DisplayText;
        //   effect.Description = @override.Description ?? baseDefinition.Description;
        //   effect.SelectionWeight = @override.SelectionWeight ?? baseDefinition.SelectionWeight;
        //   effect.Requirements.AllowedItemNames = @override.AllowedItemNames ?? baseDefinition.Requirements.AllowedItemNames;
        //   effect.Requirements.ExcludedSkillTypes = @override.ExcludedSkillTypes ?? baseDefinition.Requirements.ExcludedSkillTypes;
        //   effect.Requirements.AllowedSkillTypes = @override.AllowedSkillTypes ?? baseDefinition.Requirements.AllowedSkillTypes;
        //   effect.Requirements.ExcludedRarities = @override.ExcludedRarities ?? baseDefinition.Requirements.ExcludedRarities;
        //   effect.Requirements.AllowedRarities = @override.AllowedRarities ?? baseDefinition.Requirements.AllowedRarities;
        //   effect.Requirements.ExcludedItemTypes = @override.ExcludedItemTypes ?? baseDefinition.Requirements.ExcludedItemTypes;
        //   effect.Requirements.AllowedItemTypes = @override.AllowedItemTypes ?? baseDefinition.Requirements.AllowedItemTypes;
        //   effect.Requirements.ExclusiveEffectTypes = @override.ExclusiveEffectTypes ?? baseDefinition.Requirements.ExclusiveEffectTypes;
        //   effect.Requirements.ExcludedItemNames = @override.ExcludedItemNames ?? baseDefinition.Requirements.ExcludedItemNames;
        //   effect.ValuesPerRarity = @override.ValuesPerRarity ?? baseDefinition.ValuesPerRarity;
        //   return effect;
        //}
    }
}
