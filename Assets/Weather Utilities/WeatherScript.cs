using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using SimpleJSON;
using UnityEngine.Events;

public class WeatherScript : MonoBehaviour
{

    [Tooltip("OpenWeatherMap API Key")]
    public string key;
    [Tooltip("Refresh rate in minutes. Only values higher that 10.")]
    public int refreshrate;

    private WeatherObject wo;
    private string oldweather;
    
    void Start()
    {

        if (key == "")
        {
            Debug.LogError("ERROR: Please use valid API key");
            key = "3fddc2962950afe300ee107b202bd736";
        }

        if(refreshrate < 10)
        {
            refreshrate = 10;
            Debug.LogWarning("WARNING: Use only refresh rates longer than 10 minutes.");
        }

        wo = new WeatherObject();
        oldweather = "";

        StartCoroutine("GetWeather");
        InvokeRepeating("GetWeather", 0, refreshrate * 60);


    }    

    IEnumerator GetWeather()
    {

        string url = "api.openweathermap.org/data/2.5/weather?lat=35&lon=139&APPID=" + key;

        WWW www = new WWW(url);
        yield return www;

        wo = wo.FromJSON(www.text);

        Debug.Log(wo.ToString());

        if (!wo.getWeather_main().Equals(oldweather))
        {
            BroadcastMessage("OnWeatherChanged", wo.getWeather_main());
        }

        oldweather = wo.getWeather_main();

    }

    /// <summary>
    /// Returns a WeatherObject containing all current weather data
    /// </summary>
    /// <returns>current weather object</returns>
    public WeatherObject GetCurrentWeatherObject()
    {
        return wo;
    }

}

[Serializable]
public class WeatherObject
{
    private float coord_lon;
    private float coord_lat;
    private string weather_id;
    private string weather_main;
    private string weather_description;
    private float main_temp_kelvin;
    private float main_pressure;
    private float main_humidity;
    private float main_temp_min_kelvin;
    private float main_temp_max_kelvin;
    private float main_sea_level;
    private float main_grnd_level;
    private float visibility;
    private float wind_speed_metersec;
    private float wind_deg;
    private float clouds_all;
    private float rain_3h;
    private float snow_3h;
    private int dt;
    private int sunrise;
    private int sunset;
    private string id;
    private string name;

    /// <summary>
    /// Initializes weather object with data from correct JSON data
    /// </summary>
    /// <param name="jsonobject">JSON object containing the data</param>
    /// <returns>the initialized weather object</returns>
    public WeatherObject FromJSON(string jsonobject)
    {
        var N = JSON.Parse(jsonobject);

        coord_lon = N["coord"]["lon"];
        coord_lat = N["coord"]["lat"];
        weather_id = N["weather"][0]["id"];
        weather_main = N["weather"][0]["main"];
        weather_description = N["weather"][0]["description"];
        main_temp_kelvin = N["main"]["temp"];
        main_pressure = N["main"]["pressure"];
        main_humidity = N["main"]["humidity"];
        main_temp_min_kelvin = N["main"]["temp_min"];
        main_temp_max_kelvin = N["main"]["temp_max"];
        main_sea_level = N["main"]["sea_level"];
        main_grnd_level = N["main"]["grnd_level"];
        visibility = N["visibility"];
        wind_speed_metersec = N["wind"]["speed"];
        wind_deg = N["wind"]["deg"];
        clouds_all = N["clouds"]["all"];
        rain_3h = N["rain"]["3h"];
        snow_3h = N["snow"]["3h"];
        dt = N["dt"];
        sunrise = N["sys"]["sunrise"];
        sunset = N["dt"]["sunset"];
        id = N["id"];
        name = N["name"];

        return this;
    }

    /// <summary>
    /// Returns a well formatted string describing this object
    /// </summary>
    /// <returns>string</returns>
    public override string ToString()
    {
        return "coord_lon: " + coord_lon + Environment.NewLine +
            "coord_lat: " + coord_lat + Environment.NewLine +
            "weather_id: " + weather_id + Environment.NewLine +
            "weather_main: " + weather_main + Environment.NewLine +
            "weather_description: " + weather_description + Environment.NewLine +
            "main_temp_kelvin: " + main_temp_kelvin + Environment.NewLine +
            "main_pressure: " + main_pressure + Environment.NewLine +
            "main_humidity: " + main_humidity + Environment.NewLine +
            "main_temp_min_kelvin: " + main_temp_min_kelvin + Environment.NewLine +
            "main_temp_max_kelvin: " + main_temp_max_kelvin + Environment.NewLine +
            "main_sea_level: " + main_sea_level + Environment.NewLine +
            "main_grnd_level: " + main_grnd_level + Environment.NewLine +
            "visibility: " + visibility + Environment.NewLine +
            "wind_speed_metersec: " + wind_speed_metersec + Environment.NewLine +
            "wind_deg: " + wind_deg + Environment.NewLine +
            "clouds_all: " + clouds_all + Environment.NewLine +
            "rain_3h: " + rain_3h + Environment.NewLine +
            "snow_3h: " + snow_3h + Environment.NewLine +
            "dt: " + dt + Environment.NewLine +
            "sunrise: " + sunrise + Environment.NewLine +
            "sunset: " + sunset + Environment.NewLine +
            "id: " + id + Environment.NewLine +
            "name: " + name + Environment.NewLine;
    }

    /// <summary>
    /// Returns geo location, longitude
    /// </summary>
    /// <returns>geo location, longitude</returns>
    public float getCoord_lon()
    {
        return coord_lon;
    }

    /// <summary>
    /// Returns geo location, latitude
    /// </summary>
    /// <returns>geo location, latitude</returns>
    public float getCoord_lat()
    {
        return coord_lat;
    }

    /// <summary>
    /// Returns Weather condition id
    /// </summary>
    /// <returns>Weather condition id</returns>
    public string getWeather_id()
    {
        return weather_id;
    }

    /// <summary>
    /// Returns Group of weather parameters (Rain, Snow, Extreme etc.)
    /// </summary>
    /// <returns>Group of weather parameters (Rain, Snow, Extreme etc.)</returns>
    public string getWeather_main()
    {
        return weather_main;
    }

    /// <summary>
    /// Returns Weather condition within the group
    /// </summary>
    /// <returns>Weather condition within the group</returns>
    public string getWeather_description()
    {
        return weather_description;
    }

    /// <summary>
    /// Returns Temperature in Kelvin
    /// </summary>
    /// <returns>Temperature</returns>
    public float getTemp()
    {
        return main_temp_kelvin;
    }

    /// <summary>
    /// Returns Temperature in the unit specified
    /// </summary>
    /// <param name="unit">temperature unit</param>
    /// <returns>temperature</returns>
    public float getTemp(TempUnit unit)
    {
        float returnValue = main_temp_kelvin;

        switch (unit)
        {
            case TempUnit.CELSIUS:
                returnValue = (returnValue - 273);
                break;
            case TempUnit.FAHRENHEIT:
                returnValue = (1.8f * (returnValue - 273) + 32);
                break;
            case TempUnit.KELVIN:
                returnValue = main_temp_kelvin;
                break;

            default:
                returnValue = main_temp_kelvin;
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// Returns Atmospheric pressure (on the sea level, if there is no sea_level or grnd_level data), hPa
    /// </summary>
    /// <returns>Atmospheric pressure</returns>
    public float getPressure()
    {
        return main_pressure;
    }

    /// <summary>
    /// Returns Humidity, %
    /// </summary>
    /// <returns>Humidity</returns>
    public float getHumidity()
    {
        return main_humidity;
    }

    /// <summary>
    /// Returns Minimum temperature at the moment. This is deviation from current temp that is possible for large cities and megalopolises geographically expanded. In Kelvin.
    /// </summary>
    /// <returns>minimum temperature</returns>
    public float getTemp_min()
    {
        return main_temp_min_kelvin;
    }

    /// <summary>
    /// Returns Returns Minimum temperature at the moment. This is deviation from current temp that is possible for large cities and megalopolises geographically expanded. In the specified unit.
    /// </summary>
    /// <param name="unit">temperature unit</param>
    /// <returns>minimum temperature</returns>
    public float getTemp_min(TempUnit unit)
    {
        float returnValue = main_temp_min_kelvin;

        switch (unit)
        {
            case TempUnit.CELSIUS:
                returnValue = (returnValue - 273);
                break;
            case TempUnit.FAHRENHEIT:
                returnValue = (1.8f * (returnValue - 273) + 32);
                break;
            case TempUnit.KELVIN:
                returnValue = main_temp_min_kelvin;
                break;

            default:
                returnValue = main_temp_min_kelvin;
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// Returns Maximum temperature at the moment. This is deviation from current temp that is possible for large cities and megalopolises geographically expanded (use these parameter optionally). In Kelvin.
    /// </summary>
    /// <returns>maximum temperature</returns>
    public float getTemp_max()
    {
        return main_temp_max_kelvin;
    }

    /// <summary>
    /// Returns Maximum temperature at the moment. This is deviation from current temp that is possible for large cities and megalopolises geographically expanded (use these parameter optionally). In the specified unit.
    /// </summary>
    /// <param name="unit">temperature unit</param>
    /// <returns>maximum temperature</returns>
    public float getTemp_max(TempUnit unit)
    {
        float returnValue = main_temp_max_kelvin;

        switch (unit)
        {
            case TempUnit.CELSIUS:
                returnValue = (returnValue - 273);
                break;
            case TempUnit.FAHRENHEIT:
                returnValue = (1.8f * (returnValue - 273) + 32);
                break;
            case TempUnit.KELVIN:
                returnValue = main_temp_max_kelvin;
                break;

            default:
                returnValue = main_temp_max_kelvin;
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// Returns Atmospheric pressure on the sea level, hPa
    /// </summary>
    /// <returns>pressure</returns>
    public float getPressure_sea_level()
    {
        return main_sea_level;
    }

    /// <summary>
    /// Returns Atmospheric pressure on the ground level, hPa
    /// </summary>
    /// <returns>pressure</returns>
    public float getPressure_grnd_level()
    {
        return main_grnd_level;
    }

    /// <summary>
    /// Returns Visibility, meter
    /// </summary>
    /// <returns>visibility</returns>
    public float getVisibility()
    {
        return visibility;
    }

    /// <summary>
    /// Returns Wind speed, meter/sec.
    /// </summary>
    /// <returns>Wind speed</returns>
    public float getWind_speed()
    {
        return wind_speed_metersec;
    }

    /// <summary>
    /// Returns Wind speed, in the unit specified
    /// </summary>
    /// <param name="unit">speed unit</param>
    /// <returns>Wind speed</returns>
    public float getWind_speed(SpeedUnit unit)
    {
        float returnValue = wind_speed_metersec;

        switch (unit)
        {
            case SpeedUnit.METRIC:
                returnValue = wind_speed_metersec;
                break;
            case SpeedUnit.IMPERIAL:
                returnValue = (returnValue * 2.2369f);
                break;

            default:
                returnValue = wind_speed_metersec;
                break;
        }

        return returnValue;
    }

    /// <summary>
    /// Returns Wind direction, degrees (meteorological)
    /// </summary>
    /// <returns>Wind direction</returns>
    public float getWind_deg()
    {
        return wind_deg;
    }

    /// <summary>
    /// Returns Cloudiness, %
    /// </summary>
    /// <returns>cloudiness</returns>
    public float getClouds()
    {
        return clouds_all;
    }

    /// <summary>
    /// Returns Rain volume for the last 3 hours
    /// </summary>
    /// <returns>rain volume</returns>
    public float getRain_3h()
    {
        return rain_3h;
    }

    /// <summary>
    /// Returns Snow volume for the last 3 hours
    /// </summary>
    /// <returns>snow volume</returns>
    public float getSnow_3h()
    {
        return snow_3h;
    }

    /// <summary>
    /// Returns Time of data calculation, unix, UTC
    /// </summary>
    /// <returns>time</returns>
    public int getDt()
    {
        return dt;
    }

    /// <summary>
    /// Returns Sunrise time, unix, UTC
    /// </summary>
    /// <returns>sunrise time</returns>
    public int getSunrise()
    {
        return sunrise;
    }

    /// <summary>
    /// Returns Sunset time, unix, UTC
    /// </summary>
    /// <returns>sunset time</returns>
    public int getSunset()
    {
        return sunset;
    }

    /// <summary>
    /// Returns City ID
    /// </summary>
    /// <returns>City ID</returns>
    public string getId()
    {
        return id;
    }

    /// <summary>
    /// Returns City name
    /// </summary>
    /// <returns>City name</returns>
    public string getName()
    {
        return name;
    }
}

public enum TempUnit
{
    KELVIN,
    CELSIUS,
    FAHRENHEIT
}

public enum SpeedUnit
{
    METRIC,
    IMPERIAL
}

