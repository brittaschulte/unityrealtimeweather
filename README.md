# Unity Real-Time Weather

## Usage

Add the *WeatherScript* to an empty GameObject in your scene. 

[Register an OpenWeatherMap API Key](https://openweathermap.org/api) and copy it into the corresponding field of the script.

Choose a refresh rate which specifies how often the weather data will be updated. 

You can then request every information offered by the OWM API individually or implement a *OnWeatherChanged(string)* method which will get called when the main weather type has changed.
