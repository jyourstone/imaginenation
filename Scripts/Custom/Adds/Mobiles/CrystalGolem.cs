using System;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a crystalized corpse")]
    public class CrystalGolem : BaseCreature
    {
        [Constructable]
        public CrystalGolem(): base(AIType.AI_SphereMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Crystal Golem";
            Body = 354;
            //BaseSoundID = 0;

            SetStr(646, 670);
            SetDex(231, 250);
            SetInt(226, 240);

            SetHits(528, 642);

            SetDamage(35, 70);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 15, 20);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 5, 10);

            SetSkill(SkillName.MagicResist, 15.1, 40.0);
            SetSkill(SkillName.Tactics, 35.1, 50.0);
            SetSkill(SkillName.Wrestling, 35.1, 50.0);

            Fame = 600;

            VirtualArmor = 18;

        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Meager);
            PackGold(80);
        }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        public CrystalGolem(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
