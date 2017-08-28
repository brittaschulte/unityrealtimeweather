using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherDataManager : MonoBehaviour {

    public void Start()
    {
        Invoke("CheckWeather", 3.0f);
    }

    private void CheckWeather()
    {
        Debug.Log(GetComponent<WeatherScript>().GetCurrentWeatherObject().ToString());
    }

    public void OnWeatherChanged(string weather)
    {
        Debug.Log("Weather changed to: " + weather);
    }


}
