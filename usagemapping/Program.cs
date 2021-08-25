using System;
using System.Collections.Generic;
using System.Linq;

namespace UsageMapping
{
    class Program
    {
        private static List<string> Effects = new List<string>
        {
            "ModifyPhysicalDamage",
            "ModifyElementalDamage",
            "ModifyDurability",
            "ReduceWeight",
            "RemoveSpeedPenalty",
            "ModifyBlockPower",
            "ModifyParry",
            "ModifyArmor",
            "ModifyBackstab",
            "IncreaseHealth",
            "IncreaseStamina",
            "ModifyHealthRegen",
            "AddHealthRegen",
            "ModifyStaminaRegen",
            "AddBluntDamage",
            "AddSlashingDamage",
            "AddPiercingDamage",
            "AddFireDamage",
            "AddFrostDamage",
            "AddLightningDamage",
            "AddPoisonDamage",
            "AddSpiritDamage",
            "AddFireResistancePercentage",
            "AddFrostResistancePercentage",
            "AddLightningResistancePercentage",
            "AddPoisonResistancePercentage",
            "AddSpiritResistancePercentage",
            "AddElementalResistancePercentage",
            "AddBluntResistancePercentage",
            "AddSlashingResistancePercentage",
            "AddPiercingResistancePercentage",
            "AddPhysicalResistancePercentage",
            "ModifyMovementSpeed",
            "ModifySprintStaminaUse",
            "ModifyJumpStaminaUse",
            "ModifyAttackStaminaUse",
            "ModifyBlockStaminaUse",
            "Indestructible",
            "Weightless",
            "AddCarryWeight",
            "LifeSteal",
            "ModifyAttackSpeed",
            "Throwable",
            "Waterproof",
            "Paralyze",
            "DoubleJump",
            "WaterWalking",
            "ExplosiveArrows",
            "QuickDraw",
            "AddSwordsSkill",
            "AddKnivesSkill",
            "AddClubsSkill",
            "AddPolearmsSkill",
            "AddSpearsSkill",
            "AddBlockingSkill",
            "AddAxesSkill",
            "AddBowsSkill",
            "AddUnarmedSkill",
            "AddPickaxesSkill",
            "AddMovementSkills",
            "ModifyStaggerDuration",
            "QuickLearner",
            "FreeBuild",
            "RecallWeapon",
            "ReflectDamage",
            "AvoidDamageTaken",
            "StaggerOnDamageTaken",
            "FeatherFall",
            "ModifyDiscoveryRadius",
            "Comfortable",
            "ModifyMovementSpeedLowHealth",
            "ModifyHealthRegenLowHealth",
            "ModifyStaminaRegenLowHealth",
            "ModifyArmorLowHealth",
            "ModifyDamageLowHealth",
            "ModifyBlockPowerLowHealth",
            "ModifyParryLowHealth",
            "ModifyAttackSpeedLowHealth",
            "AvoidDamageTakenLowHealth",
            "LifeStealLowHealth",
            "Glowing",
            "Executioner",
            "Riches",
            "Opportunist",
            "Duelist",
            "Immovable",
            "ModifyStaggerDamage",
            "Luck",
            "ModifyParryWindow",
            "Slow"
        };

        private static Dictionary<string, List<string>> MappedGroups = new Dictionary<string, List<string>>
        {
            ["CHEST"] = new List<string>
            {
                "IncreaseHealth",
                "Comfortable",
                "AddCarryWeight",
                "AddUnarmedSkill",
                "AddBluntDamage",
                "AddSlashingDamage",
                "AddPiercingDamage",
                "IncreaseStamina",
                "ModifySprintStaminaUse",
                "ModifyJumpStaminaUse",
                "Luck",
                "Riches",
                "Glowing",
                "ModifyHealthRegen",
                "ModifyStaminaRegen",
                "AddElementalResistancePercentage",
                "Immovable",
                "ModifyMovementSpeedLowHealth",
                "ModifyHealthRegenLowHealth",
                "ModifyStaminaRegenLowHealth",
                "AvoidDamageTakenLowHealth",
                "ModifyArmor",
                "AddFireResistancePercentage",
                "AddFrostResistancePercentage",
                "AddLightningResistancePercentage",
                "AddPoisonResistancePercentage",
                "AddSpiritResistancePercentage",
                "AddBluntResistancePercentage",
                "AddSlashingResistancePercentage",
                "AddPiercingResistancePercentage"
            },
            ["HEAD"] = new List<string>
            {
                "IncreaseHealth",
                "Comfortable",
                "AddCarryWeight",
                "AddUnarmedSkill",
                "AddBluntDamage",
                "AddSlashingDamage",
                "AddPiercingDamage",
                "QuickLearner",
                "AddSwordsSkill",
                "AddKnivesSkill",
                "AddClubsSkill",
                "AddPolearmsSkill",
                "AddSpearsSkill",
                "AddBlockingSkill",
                "AddAxesSkill",
                "AddBowsSkill",
                "AddPickaxesSkill",
                "AddMovementSkills",
                "QuickDraw",
                "ModifyAttackSpeed",
                "ReflectDamage",
                "Glowing",
                "StaggerOnDamageTaken",
                "ModifyStaggerDuration",
                "LifeStealLowHealth",
                "ModifyArmorLowHealth",
                "ModifyDamageLowHealth",
                "ModifyBlockPowerLowHealth",
                "ModifyParryLowHealth",
                "ModifyAttackSpeedLowHealth",
                "FreeBuild",
                "AddFireDamage",
                "AddFrostDamage",
                "AddLightningDamage",
                "AddPoisonDamage",
                "AddSpiritDamage",
                "ModifyArmor",
                "AddBluntResistancePercentage",
                "AddSlashingResistancePercentage",
                "AddPiercingResistancePercentage",
                "AddPhysicalResistancePercentage",
                "Duelist",
                "ModifyParryWindow",
                "ModifyBlockStaminaUse",
                "Executioner",
                "Opportunist"
            },
            ["LEGS"] = new List<string>
            {
                "IncreaseHealth",
                "Comfortable",
                "AddCarryWeight",
                "AddUnarmedSkill",
                "AddBluntDamage",
                "AddSlashingDamage",
                "AddPiercingDamage",
                "AddCarryWeight",
                "Weightless",
                "IncreaseStamina",
                "ModifyStaminaRegen",
                "AvoidDamageTaken",
                "ModifyMovementSpeed",
                "ModifySprintStaminaUse",
                "ModifyJumpStaminaUse",
                "WaterWalking",
                "FeatherFall",
                "Waterproof",
                "AddCarryWeight",
                "Comfortable"
            },
            ["CLOAK"] = new List<string>
            {
                "Glowing",
                "ModifyArmor",
                "AddFireResistancePercentage",
                "AddFrostResistancePercentage",
                "AddLightningResistancePercentage",
                "AddPoisonResistancePercentage",
                "AddSpiritResistancePercentage",
                "AddBluntResistancePercentage",
                "AddSlashingResistancePercentage",
                "AddPiercingResistancePercentage",
                "ModifyDiscoveryRadius",
                "ModifyMovementSpeed",
                "ModifySprintStaminaUse",
                "ModifyJumpStaminaUse",
                "AddMovementSkills",
                "ModifyMovementSpeedLowHealth",
                "ModifyStaminaRegenLowHealth",
                "IncreaseStamina",
                "ModifyStaminaRegen",
                "ModifyBackstab",
                "Luck",
                "Riches",
                "Comfortable",
                "QuickLearner",
                "LifeStealLowHealth",
                "ReflectDamage",
                "AddFireDamage",
                "AddFrostDamage",
                "AddLightningDamage",
                "AddPoisonDamage",
                "AddSpiritDamage",
                "AddElementalResistancePercentage",
                "Slow",
                "Paralyze",
                "LifeSteal",
                "FeatherFall",
                "DoubleJump",
                "WaterWalking",
                "Waterproof"
            },
            ["UTILITY"] = new List<string>
            {
                "Waterproof",
                "AddBlockingSkill",
                "ModifyBlockPower",
                "ModifyBlockPowerLowHealth",
                "ModifyParryWindow",
                "ModifyParryLowHealth",
                "AvoidDamageTaken",
                "AvoidDamageTakenLowHealth",
                "AddElementalResistancePercentage",
                "AddPhysicalResistancePercentage",
                "IncreaseHealth",
                "IncreaseStamina",
                "ModifyMovementSpeed",
                "Luck",
                "ModifyDiscoveryRadius",
                "QuickLearner",
                "AddCarryWeight",
                "ReflectDamage",
                "Glowing",
                "AddFireDamage",
                "AddFrostDamage",
                "AddLightningDamage",
                "AddPoisonDamage",
                "AddSpiritDamage",
                "AddFireResistancePercentage",
                "AddFrostResistancePercentage",
                "AddLightningResistancePercentage",
                "AddPoisonResistancePercentage",
                "AddSpiritResistancePercentage",
                "Slow",
                "Paralyze",
                "LifeSteal",
                "FeatherFall",
                "DoubleJump",
                "WaterWalking",
                "Riches",
                "Comfortable",
                "LifeStealLowHealth",
                "AddUnarmedSkill",
                "AddBluntDamage",
                "AddSlashingDamage",
                "AddPiercingDamage",
                "ModifyBlockStaminaUse",
                "AddBluntResistancePercentage",
                "AddSlashingResistancePercentage",
                "AddPiercingResistancePercentage",
                "Weightless",
            }
        };

        static void Main(string[] args)
        {
            var counters = new Dictionary<string, int>();
            var unused = new List<string>();
         
            foreach (var effect in Effects)
            {
                counters.Add(effect, 0);

                List<string> mappings = new List<string>();
                foreach (var group in MappedGroups)
                {
                    if (group.Value.Contains(effect))
                    {
                        mappings.Add(group.Key);
                    }
                }
                Console.WriteLine($"{effect} - {mappings.Count}");
                foreach (var m in mappings)
                {
                    Console.WriteLine($"   ├-{m}");
                }

                counters[effect] = mappings.Count;
            }

            foreach (var c in counters)
            {
                if (c.Value == 0)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    unused.Add(c.Key);
                }

                Console.WriteLine("{0,-35}{1}", c.Key, c.Value);
                Console.ForegroundColor = ConsoleColor.Gray;
            }

            Console.WriteLine("------UNUSED EFFECTS---------");

            foreach (var u in unused)
            {
                Console.WriteLine(u);
            }
        }
    }
}
