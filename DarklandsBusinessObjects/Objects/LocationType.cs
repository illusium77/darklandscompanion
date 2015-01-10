namespace DarklandsBusinessObjects.Objects
{
    public enum LocationType
    {
        City = 0x00,
        CastleLord = 0x01,
        CastleRaubritter = 0x02,
        Monastery = 0x03,
        Alter = 0x04,
        CaveA = 0x05,
        Mines = 0x06,
        VillageA = 0x08,
        VillageRuins = 0x09,
        VillageB = 0x0a,
        Tomb = 0x0d,
        DragonsLair = 0x0f,
        Spring = 0x10,
        Lake = 0x11,
        Shrine = 0x12,
        CaveB = 0x13,
        PaganAltar = 0x14,
        WitchSabbat = 0x15,
        TemplarCastle = 0x16,
        Hockkonig = 0x17,
        AlpineCave = 0x18,
        LadyOfTheLake = 0x19,
        RaubritterRuins = 0x1a
    }

    //Enumeration: location_icon

    //Map image for the location.

    //Darklands keeps the images and the menu options tied together, so the 'image' for a location is pretty much the indicator of the 'type' of location.
    //Any unlisted values are displayed as the standard 'castle' style image.
    //Data Value	Meaning
    //0x00	city
    //0x01	castle (lord or evil lord variety)
    //0x02	castle (Raubritter variety)
    //0x03	monastery
    //0x04	(looks like tomb or pagan altar) TODO: Teufelstein is one
    //0x05	cave (TODO: what kind?)
    //0x06	mines
    //0x08	village
    //0x09	ruins of a village
    //0x0a	village (more square than 0x08, and unused?)
    //0x0d	tomb
    //0x0f	dragon's lair (invisible; cannot interact?)
    //0x10	spring
    //0x11	lake
    //0x12	shrine
    //0x13	cave (TODO: what kind?)
    //0x14	pagan altar
    //0x15	witch sabbat
    //0x16	Templar castle (has a black top)
    //0x17	Hockkonig (the Baphomet castle; all gray)
    //0x18	alpine cave
    //0x19	lady of the lake (magician/astrologer)
    //0x1a	ruins of a Raubritter's castle
}