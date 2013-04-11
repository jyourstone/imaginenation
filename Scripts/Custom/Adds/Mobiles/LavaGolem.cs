using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("a lava golem corpse")]
    public class LavaGolem : BaseCreature
    {
        [Constructable]
        public LavaGolem()
            : base(AIType.AI_SphereMelee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Lava Golem";
            Body = 289;
            BaseSoundID = 268;

            SetStr(225, 250);
            SetDex(110, 135);
            SetInt(66, 90);

            SetHits(358, 372);
            SetMana(0);

            SetDamage(18, 35);

            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 20, 25);
            SetResistance(ResistanceType.Fire, 10, 20);
            SetResistance(ResistanceType.Cold, 5, 10);
            SetResistance(ResistanceType.Poison, 5, 10);
            SetResistance(ResistanceType.Energy, 10, 15);

            SetSkill(SkillName.MagicResist, 190.6, 205.0);
            SetSkill(SkillName.Tactics, 125.0, 150.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);

            Fame = 4500;

            VirtualArmor = 38;

            PackItem(new FireOre(Utility.RandomMinMax(1, 2)));
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems);
            PackGold(300);
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 1; } }

        public override Poison PoisonImmune { get { return Poison.Deadly; } }

        /*public override void OnCarve(Mobile from, Corpse corpse, Item item)
        {
            corpse.DropItem(new ()); //item for new shield recipes
            base.OnCarve(from, corpse, item);

        } */

        public LavaGolem(Serial serial)
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