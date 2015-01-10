namespace DarklandsBusinessObjects.Objects
{
    // https://web.archive.org/web/20091111161830/http://wallace.net/darklands/formats/darkland.cty.html#enumeration-ruler
    //Type of city (free, ruled, capital)
    //This controls which message from $CITYE00.msg is given when you are approaching a city.

    public enum CityType
    {
        FreeCity = 0,
        RuledCity = 1,
        Capital = 2
    }
}