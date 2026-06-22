namespace Heliotracker.Services;

public static class SolarMathHelper
{
    /// <summary>
    /// Calcuates the avilable percentage of full sun using NOAA solar algorithms.
    /// </summary>
    /// <param name="lat">Latitude in decimal degrees</param>
    /// <param name="lon">Longitude in decimal degrees</param>
    /// <param name="utcTime">The UTC time of the log</param>
    /// <returns>A double between 0.0(no sun) and 1.0(peak sun)</returns>
    public static double GetAvailableSunPercentage(double lat, double lon, DateTime utcTime)
    {
        //To figure out how the earth is tilted at this time
        var dayOfYear = utcTime.DayOfYear;
        double hourFraction = utcTime.Hour + (utcTime.Minute / 60) + (utcTime.Second / 3600);

        // Gamma value
        var fractionalYear = (2 * Math.PI / 365.0) * (dayOfYear - 1 + (hourFraction - 12) / 24.0);
        
        //solar declination angle between -23.5 and 23.5 degrees converted to radians
        //(where the earth's axis is tilted at the current time of year)
        var decl = CalculateSolarDeclinationAngle(fractionalYear);
        
        //Note: Not accounting for eqtime or the effect of the elliptical orbit because it makes at most a 14-minute difference
        //Just a trade-off I have decided to make due to its minimal significance 
        var solarTimeOffset = 4.0 * lon;
        
        //Adjusting the clock to find out the exact minute the sun is overhead
        var trueSolarTime = (utcTime.Hour * 60) + utcTime.Minute + (utcTime.Second / 60.0) + solarTimeOffset;

        var ha = (trueSolarTime / 4.0) - 180.0;
        var haRad = ha * (Math.PI / 180.0);
        var latRad = lat * (Math.PI / 180.0);
        
        //NOAA Spherical Trigonometry
        var elevationAngle = CalculateSolarElevation(latRad, decl, haRad);

        if (elevationAngle <= 0)
        {
            return 0.0; //Sun is below the horizon
        }
        
        var radians = elevationAngle * (Math.PI / 180.0);
        return Math.Sin(radians);
    }

    private static double CalculateSolarElevation(double lat,  double decl, double ha)
    {
        //NOAA spherical trigonometry equation
        // final return is the angle of the sun in degrees above the horizon 0-90
        //cos(zenith angle) = sin(lat)sin(decl) + cos(lat)cos(decl)cos(ha)
        var cosZenith = (Math.Sin(lat) * Math.Sin(decl)) + (Math.Cos(lat) * Math.Cos(decl) * Math.Cos(ha));
        var zenithAngle = Math.Acos(cosZenith) * (180.0/Math.PI);
        var elevationAngle = 90.0 - zenithAngle;
        return elevationAngle;
    }

    private static double CalculateSolarDeclinationAngle(double fractionalYear)
    {
        var solarDeclinationAngle = 0.006918 - 0.399912 * Math.Cos(fractionalYear)
            + 0.070257 * Math.Sin(fractionalYear)
            - 0.006758 * Math.Cos(2 * fractionalYear);
        return solarDeclinationAngle;
    }
}