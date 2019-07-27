Weather Prediction is a very simple stochastic algorithm for predicting weather from datasets, using probability distributions and single exponential smoothing.

## Main tasks

* Predicting temperature
* Predicting humidity
* Predicting weather
* Add indicators (Humidex, Windchill)

## Algorithm

Algorithm used is quite simple, use simple mathematicals concepts and take into consideration only a few parameters, so the efficiency is relative and only allows a globally consistent prediction but without extreme scenarios.

### Predicting the day's temperature of a city 

A normal distribution is used to determine gross variation in temperature from one day to the next.

![Alt text](img/5.gif)

Then, single exponential smoothing is used to smooth temperature to seasonal normal, to avoid inconsistent temperature.

The alpha parameter determines the importance of flattening.

* 1: the temperature is entirely influenced by the seasonal normal.
* 0 : the seasonal normal is ignored.

![Alt text](img/4.gif)

Finally, to remain consistent with other cities, temperature of a city (called after the station) is influenced by temperature of the other cities the day before. A city close to another must have a similar weather.

![Alt text](img/3.gif)

Beta parameter determine how this influence is big.

* 1 : total influence
* 0 : no influence

Influence temperature of other cities is the sum of temperature of other cities multipled by proximity of the city to the station. 

![Alt text](img/0.gif)

City influence is the ratio of its distance to the station to the sum of all distances

![Alt text](img/1.gif)

![Alt text](img/2.gif)

### Hourly forecasts

Polynomial interpolation is used to determine temperature and humidity at a specific time thanks to min and max values.

## Data

Currently, data used is only seasonal normal temperature for each day, for each city.

City is described by its coordinates.

A report is described by its minimal temperature (TMin), maximal temperature (TMax) and date.

### Data Format

```
City,City name,X coordinate, Y coordinate
Date,TMin,TMax
...
```

Example

```
City,Bestine,1174,611
01/06/0280,18,36
02/06/0280,18,37
03/06/0280,18,37
04/06/0280,18,37
City,Carnthout,1280,711
01/06/0280,17,34
02/06/0280,17,34
03/06/0280,17,35
04/06/0280,17,35
```

## Example

### Tatooine dataset

Only temperature is actually implemented, planet Tatooine (Star Wars' planet with an arid climate) can be taken as an example (considerating that they is no clouds in this planet) (https://www.researchgate.net/publication/326748913_Analysis_of_Weather_Data_Using_Forecasting_Algorithms_ICCI-2017)


![Alt text](img/screen1.png?raw=true "Screenshot")

Hourly temperature

![Alt text](img/screen2.png?raw=true "Screenshot")


## Authors
Nicolas Lépy

## Tools used
Visual Studio
C#
WPF

## Credits
Selvaraj, Poornima & Marudappa, Pushpalatha & Sujit Shankar, J. (2019). Analysis of Weather Data Using Forecasting Algorithms: ICCI-2017. 10.1007/978-981-13-1132-1_1.

Live Charts for WPF (https://lvcharts.net/)
MathNet.Numerics

## License

This project is licensed under the MIT License - see the [LICENSE.md](LICENSE.md) file for details