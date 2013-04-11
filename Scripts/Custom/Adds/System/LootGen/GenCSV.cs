using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using Server.Commands;
using Server.Mobiles;

namespace Server.Scripts.Custom.Adds.System.LootGen
{
    public class GenCSV
    {
        private static readonly List<Type> m_BaseCreatures = new List<Type>();

        public static void Initialize()
        {
            CommandSystem.Register("GenCreatureCSV", AccessLevel.Administrator, Execute);
        }

        private static void Execute(CommandEventArgs e)
        {
            LoadBaseCreatureTypes();

            Console.WriteLine("*** Generating BaseCreature Info ***");
            e.Mobile.SendMessage("Generating BaseCreature Info...");
            for (int i = 1; i <= 1; i++) //TODO: Multiple tests? I don't think it needs to be perfect.
            {
                GenerateCSV("output/statistics/", "basecreatures_" + i + ".csv");
            }
            e.Mobile.SendMessage("Generating BaseCreature Info... Done.");
            Console.WriteLine("*** Done Generating BaseCreature Info ***");
        }

        private static void LoadBaseCreatureTypes()
        {
            foreach (Assembly asm in ScriptCompiler.Assemblies)
            {
                foreach (Type t in asm.GetTypes())
                {
                    if (t.BaseType != typeof (BaseCreature))
                        continue;

                    m_BaseCreatures.Add(t);
                }
            }
        }

        private static void GenerateCSV(string filePath, string fileName)
        {
            string fullFilePath = filePath + fileName;

            if (!Directory.Exists(filePath))
                Directory.CreateDirectory(filePath);

            if (File.Exists(fullFilePath))
                File.Delete(fullFilePath);

            Path.Combine(filePath, fileName);

            Horse horse = new Horse();
            StreamWriter writer = new StreamWriter(fullFilePath);
            writer.Write("Type,");
            writer.Write("AIType,");
            writer.Write("Weapon,");
            writer.Write("VirtualArmor,");
            writer.Write("DamageMin,");
            writer.Write("DamageMax,");
            writer.Write("Fame,");
            writer.Write("Karma,");
            writer.Write("FightMode,");
            writer.Write("RangePerception,");
            writer.Write("RangeFight,");
            writer.Write("ActiveSpeed,");
            writer.Write("PassiveSpeed,");
            writer.Write("Str,");
            writer.Write("Dex,");
            writer.Write("Int,");
            writer.Write("HitsMax,");
            writer.Write("StamMax,");
            writer.Write("ManaMax,");
            for (int i=0; i<horse.Skills.Length; i++)
            {
                writer.Write("{0},", horse.Skills[i].Name);
            }
            horse.Delete();

            foreach (Type t in m_BaseCreatures)
            {
                try
                {
                    object creatureObj = Activator.CreateInstance(t);
                    BaseCreature bc = creatureObj as BaseCreature;

                    writer.WriteLine();
                    writer.Write("{0},", t.Name);
                    writer.Write("{0},", bc.AI);
                    writer.Write("{0},", bc.Weapon.GetType().BaseType == null ? bc.Weapon.GetType().Name : bc.Weapon.GetType().BaseType.Name);
                    writer.Write("{0},", bc.VirtualArmor);
                    writer.Write("{0},", bc.DamageMin);
                    writer.Write("{0},", bc.DamageMax);
                    writer.Write("{0},", bc.Fame);
                    writer.Write("{0},", bc.Karma);
                    writer.Write("{0},", bc.FightMode);
                    writer.Write("{0},", bc.RangePerception);
                    writer.Write("{0},", bc.RangeFight);
                    writer.Write("{0},", bc.ActiveSpeed);
                    writer.Write("{0},", bc.PassiveSpeed);
                    writer.Write("{0},", bc.Str);
                    writer.Write("{0},", bc.Dex);
                    writer.Write("{0},", bc.Int);
                    writer.Write("{0},", bc.HitsMax);
                    writer.Write("{0},", bc.StamMax);
                    writer.Write("{0},", bc.ManaMax);
                    for (int i = 0; i < bc.Skills.Length; i++)
                    {
                        writer.Write(bc.Skills[i].Value + ",");
                    }
                    bc.Delete();
                }
                catch {
                    //Console.WriteLine(e);
                }
            }
            writer.Close();
        }
    }
}
