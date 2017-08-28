using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherDataManager : MonoBehaviour {

    public void OnWeatherChanged(string weather)
    {
        Debug.Log(weather);
    }


}
