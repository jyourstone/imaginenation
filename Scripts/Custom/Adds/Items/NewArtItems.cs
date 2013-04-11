namespace Server.Items
{
    #region furniture
    [Flipable(0x3859, 0x385A)]
    public class EngravedArmoire : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x4E; } }
        [Constructable]
        public EngravedArmoire()
            : base(0x3859)
        {
            Weight = 10.0;
            Name = "Engraved armoire";
        }

        public EngravedArmoire(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x385B, 0x385C)]
    public class GrandfatherClock : Item
    {
        [Constructable]
        public GrandfatherClock()
            : base(0x385B)
        {
            Weight = 5.0;
            Name = "Grandfather clock";
        }

        public GrandfatherClock(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CornerDisplay : Item
    {
        [Constructable]
        public CornerDisplay()
            : base(0x3858)
        {
            Weight = 10.0;
            Name = "Corner display case";
        }

        public CornerDisplay(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x3871, 0x3872)]
    public class FullStoneBookcase : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x4D; } }
        [Constructable]
        public FullStoneBookcase()
            : base(0x3871)
        {
            Weight = 10.0;
            Name = "Stone bookcase";
        }

        public FullStoneBookcase(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x3873, 0x3874)]
    public class EmptyStoneBookcase : BaseContainer
    {
        public override int DefaultGumpID { get { return 0x4D; } }
        [Constructable]
        public EmptyStoneBookcase()
                : base(0x3873)
        {
            Weight = 10.0;
            Name = "Stone bookcase";
        }

        public EmptyStoneBookcase(Serial serial)
                : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    #region pillars
    public class CandleTopPillar : Item
    {
        [Constructable]
        public CandleTopPillar()
            : base(0x3875)
        {
            Weight = 25.0;
            Name = "Candle top pillar";
        }

        public CandleTopPillar(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CandlePillar : Item
    {
        [Constructable]
        public CandlePillar()
            : base(0x3876)
        {
            Weight = 35.0;
            Name = "Candle pillar";
        }

        public CandlePillar(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LargeCrystalPillar : Item
    {
        [Constructable]
        public LargeCrystalPillar()
            : base(0x3897)
        {
            Weight = 15.0;
            Name = "Large crystal pillar";
        }

        public LargeCrystalPillar(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class CrystalPillar2 : Item
    {
        [Constructable]
        public CrystalPillar2()
            : base(0x3898)
        {
            Weight = 10.0;
            Name = "CrystalPillar";
        }

        public CrystalPillar2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class RoundStonePillar : Item
    {
        [Constructable]
        public RoundStonePillar()
            : base(0x3899)
        {
            Weight = 50.0;
            Name = "Round stone pillar";
        }

        public RoundStonePillar(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    #region tiles
    [Flipable(0x3877, 0x3878, 0x3879, 0x387A, 0x387B, 0x387C, 0x387D, 0x387E, 0x387F, 0x3880)]
    public class SteppingStones : Item
    {
        [Constructable]
        public SteppingStones()
            : base(0x3877)
        {
            Weight = 5.0;
            Name = "Stepping stones";
        }

        public SteppingStones(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SilverTile : Item
    {
        [Constructable]
        public SilverTile()
            : base(0x38881)
        {
            Weight = 5.0;
            Name = "Silver tile";
        }

        public SilverTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BronzeTile : Item
    {
        [Constructable]
        public BronzeTile()
            : base(0x3882)
        {
            Weight = 5.0;
            Name = "Bronze tile";
        }

        public BronzeTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class ArcaneTile : Item
    {
        [Constructable]
        public ArcaneTile()
            : base(0x3883)
        {
            Weight = 10.0;
            Name = "Arcane tile";
        }

        public ArcaneTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class EngravedSSTile : Item
    {
        [Constructable]
        public EngravedSSTile()
            : base(0x38CA)
        {
            Weight = 10.0;
            Name = "Engraved sandstone tile";
        }

        public EngravedSSTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class StoneCircleTile : Item
    {
        [Constructable]
        public StoneCircleTile()
            : base(0x38CB)
        {
            Weight = 10.0;
            Name = "Stone circle tile";
        }

        public StoneCircleTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x3859, 0x385A)]
    public class SlatePaverTile : Item
    {
        [Constructable]
        public SlatePaverTile()
            : base(0x38CC)
        {
            Weight = 10.0;
            Name = "Slate paver";
        }

        public SlatePaverTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class GravelTile : Item
    {
        [Constructable]
        public GravelTile()
            : base(0x38CD)
        {
            Weight = 10.0;
            Name = "Gravel tile";
        }

        public GravelTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SSDiamondTile : Item
    {
        [Constructable]
        public SSDiamondTile()
            : base(0x38CE)
        {
            Weight = 10.0;
            Name = "Sandstone pattern tile";
        }

        public SSDiamondTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LightSSTile : Item
    {
        [Constructable]
        public LightSSTile()
            : base(0x38CF)
        {
            Weight = 10.0;
            Name = "Light sandstone tile";
        }

        public LightSSTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LightGravelTile : Item
    {
        [Constructable]
        public LightGravelTile()
            : base(0x38D0)
        {
            Weight = 10.0;
            Name = "Light gravel tile";
        }

        public LightGravelTile(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    #region deco
    public class StarFountain : Item
    {
        [Constructable]
        public StarFountain()
            : base(0x3FEA)
        {
            Weight = 100.0;
            Name = "Star fountain";
        }

        public StarFountain(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class AnimGrave : Item
    {
        [Constructable]
        public AnimGrave()
            : base(0x389F)
        {
            Weight = 100.0;
            Name = "Haunted grave";
        }

        public AnimGrave(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WaterWheel : Item
    {
        [Constructable]
        public WaterWheel()
            : base(0x3FF4)
        {
            Weight = 100.0;
            Name = "Water wheel";
        }

        public WaterWheel(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BluePainting : Item
    {
        [Constructable]
        public BluePainting()
            : base(0x385D)
        {
            Weight = 2.0;
            Name = "Blue painting";
        }

        public BluePainting(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class WildernessPainting : Item
    {
        [Constructable]
        public WildernessPainting()
            : base(0x385E)
        {
            Weight = 2.0;
            Name = "Wilderness painting";
        }

        public WildernessPainting(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class SketchedPortait : Item
    {
        [Constructable]
        public SketchedPortait()
            : base(0x385F)
        {
            Weight = 2.0;
            Name = "Sketched portait";
        }

        public SketchedPortait(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x389B, 0x389C)]
    public class NewPokerMachine : Item
    {
        [Constructable]
        public NewPokerMachine()
            : base(0x389B)
        {
            Weight = 100.0;
            Name = "Poker machine";
        }

        public NewPokerMachine(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x389D, 0x389E)]
    public class NewSlotMachine : Item
    {
        [Constructable]
        public NewSlotMachine()
            : base(0x389D)
        {
            Weight = 100.0;
            Name = "Slot machine";
        }

        public NewSlotMachine(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    #region misc
    [Flipable(0x38E0, 0x38E1)]
    public class PlasterBayWindow : Item
    {
        [Constructable]
        public PlasterBayWindow()
            : base(0x38E0)
        {
            Weight = 50.0;
            Name = "Bay window";
            Movable = false;
        }

        public PlasterBayWindow(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class TeddyBear : Item
    {
        [Constructable]
        public TeddyBear()
            : base(0x38FC)
        {
            Weight = 2.0;
            Name = "Teddy bear";
            Movable = true;
        }

        public TeddyBear(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x38E6, 0x38E7)]
    public class DogHouse : Item
    {
        [Constructable]
        public DogHouse()
            : base(0x38E6)
        {
            Weight = 20.0;
            Name = "Dog house";
        }

        public DogHouse(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BearTrap : Item
    {
        [Constructable]
        public BearTrap()
            : base(0x38E8)
        {
            Weight = 5.0;
            Name = "Bear trap";
        }

        public BearTrap(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class Jacks : Item
    {
        [Constructable]
        public Jacks()
            : base(0x38E9)
        {
            Weight = 1.0;
            Name = "Jacks";
        }

        public Jacks(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class GoldTrophy : Item
    {
        [Constructable]
        public GoldTrophy()
            : base(0x38EA)
        {
            Weight = 3.0;
            Name = "Gold trophy";
        }

        public GoldTrophy(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class LongOpenScroll : Item
    {
        [Constructable]
        public LongOpenScroll()
            : base(0x38EB)
        {
            Weight = 2.0;
            Name = "Open scroll";
        }

        public LongOpenScroll(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x38EC, 0x38ED, 0x38EE, 0x38EF, 0x38F0, 0x38F1, 0x38F2, 0x38F3, 0x38F4)]
    public class FancyIronFence : Item
    {
        [Constructable]
        public FancyIronFence()
            : base(0x38EC)
        {
            Weight = 20.0;
            Name = "Fancy iron fence";
            Movable = false;
        }

        public FancyIronFence(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    #endregion

    #region boat
    [Flipable(0x38E2, 0x38E3)]
    public class ShipCaptain : Item
    {
        [Constructable]
        public ShipCaptain()
            : base(0x38E2)
        {
            Weight = 50.0;
            Name = "Ship captain";
            Movable = false;
        }

        public ShipCaptain(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    [Flipable(0x38F5, 0x38F6, 0x38F7, 0x38F8)]
    public class VikingSail : Item
    {
        [Constructable]
        public VikingSail()
            : base(0x38F5)
        {
            Weight = 50.0;
            Name = "Viking sail";
            Movable = false;
        }

        public VikingSail(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    #endregion

    #region plants

    public class BonsaiGreen1 : Item
    {
        [Constructable]
        public BonsaiGreen1()
            : base(0x38FD)
        {
            Weight = 5.0;
            Name = "Green bonsai tree";
            Movable = true;
        }

        public BonsaiGreen1(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BonsaiGreen2 : Item
    {
        [Constructable]
        public BonsaiGreen2()
            : base(0x38FE)
        {
            Weight = 5.0;
            Name = "Green bonsai tree";
            Movable = true;
        }

        public BonsaiGreen2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BonsaiGreen3 : Item
    {
        [Constructable]
        public BonsaiGreen3()
            : base(0x38FF)
        {
            Weight = 5.0;
            Name = "Green bonsai tree";
            Movable = true;
        }

        public BonsaiGreen3(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //
    public class BonsaiRed1 : Item
    {
        [Constructable]
        public BonsaiRed1()
            : base(0x3900)
        {
            Weight = 5.0;
            Name = "Red bonsai tree";
            Movable = true;
        }

        public BonsaiRed1(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BonsaiRed2 : Item
    {
        [Constructable]
        public BonsaiRed2()
            : base(0x3901)
        {
            Weight = 5.0;
            Name = "Red bonzai tree";
            Movable = true;
        }

        public BonsaiRed2(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class BonsaiRed3 : Item
    {
        [Constructable]
        public BonsaiRed3()
            : base(0x3904)
        {
            Weight = 5.0;
            Name = "Red bonsai tree";
            Movable = true;
        }

        public BonsaiRed3(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }
    //
    public class BonsaiOrange : Item
    {
        [Constructable]
        public BonsaiOrange()
            : base(0x3902)
        {
            Weight = 5.0;
            Name = "Orange bonsai tree";
            Movable = true;
        }

        public BonsaiOrange(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    public class DarkBonsai : Item
    {
        [Constructable]
        public DarkBonsai()
            : base(0x38FD)
        {
            Weight = 5.0;
            Name = "Dark bonsai tree";
            Movable = true;
        }

        public DarkBonsai(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);

            writer.Write(0); // version 
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);

            int version = reader.ReadInt();
        }
    }

    #endregion
    
}