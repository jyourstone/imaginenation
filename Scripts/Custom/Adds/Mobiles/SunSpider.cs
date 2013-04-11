//loot and overall str revised
using Server.Items;

namespace Server.Mobiles

              {
              [CorpseName( " corpse of a Sun Spider" )]
              public class SunSpider : DreadSpider
              {
                  [Constructable]
                  public SunSpider()
                  {
                      Name = "Sun Spider";
                      Hue = 2519;
                      Body = 173;
                      BaseSoundID = 1170;
                      SetStr(800, 1000);
                      SetDex(800);
                      SetInt(800);
                      SetHits(1400, 2000);
                      SetDamage(12, 40);
                      SetDamageType(ResistanceType.Physical, 58);
                      SetDamageType(ResistanceType.Cold, 0);
                      SetDamageType(ResistanceType.Fire, 90);
                      SetDamageType(ResistanceType.Energy, 45);
                      SetDamageType(ResistanceType.Poison, 50);

                      SetSkill(SkillName.EvalInt, 115.1, 140.0);
                      SetSkill(SkillName.Magery, 105.1, 130.0);
                      SetSkill(SkillName.MagicResist, 80.2, 110.0);
                      SetSkill(SkillName.Tactics, 110.1, 140.0);
                      SetSkill(SkillName.Wrestling, 110.1, 155.0);
                      SetSkill(SkillName.Healing, 85.0);
                      SetSkill(SkillName.Parry, 60.0, 90.0);

                      SetResistance(ResistanceType.Physical, 50);
                      SetResistance(ResistanceType.Cold, 0);
                      SetResistance(ResistanceType.Fire, 100);
                      SetResistance(ResistanceType.Energy, 30);
                      SetResistance(ResistanceType.Poison, 70);
                      Fame = -100;
                      Karma = -100;
                      VirtualArmor = 42;
                  }
                                            public override void GenerateLoot()
                                            {
            	                               PackGold( 3000, 3750 );
                                               AddLoot(LootPack.UltraRich );
            	                               AddLoot( LootPack.Gems, Utility.Random( 1, 5));
                                               if (Utility.RandomDouble() <= 0.2)
                                                   AddItem(new RandomAccWeap(3));
                                            }
                                 public override bool BardImmune{ get{ return true; } }
                                 public override bool Unprovokable{ get{ return true; } }
                                 public override Poison HitPoison{ get{ return Poison. Deadly ; } }
                                 public override bool AlwaysMurderer{ get{ return true; } }
                                 public override bool Uncalmable { get { return Core.SE; } }
                                 public override bool HasBreath { get { return true; } }
                                 public override int BreathFireDamage { get { return 20; } }

                                 public override WeaponAbility GetWeaponAbility()
                                 {
                                     switch (Utility.Random(4))
                                     {
                                         default:
                                         case 0: return WeaponAbility.BleedAttack;
                                         case 1: return WeaponAbility.MortalStrike;
                                         case 2: return WeaponAbility.DoubleStrike;
                                         case 3: return WeaponAbility.Block;
                                     }
                                 }

public SunSpider( Serial serial ) : base( serial )
                      {
                      }

public override void Serialize( GenericWriter writer )
                      {
                                        base.Serialize( writer );
                                        writer.Write( 0 );
                      }

        public override void Deserialize( GenericReader reader )
                      {
                                        base.Deserialize( reader );
                                        int version = reader.ReadInt();
                      }
    }
}
