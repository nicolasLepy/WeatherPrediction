Weather Prediction is a very simple stochastic algorithm for predicting weather from datasets, using probability distributions and single exponential smoothing.

## Main tasks

* Predicting temperature
* Predicting humidity
* Predicting weather

## Algorithm

Algorithm used is quite simple, use simple mathematicals concepts and take into consideration only a few parameters, so the efficiency is relative and only allows a globally consistent prediction but without extreme scenarios.

### Predicting the day's temperature of a city 

![Alt text](img/0.gif)
![Alt text](img/1.gif)
![Alt text](img/2.gif)
![Alt text](img/3.gif)
![Alt text](img/4.gif)
![Alt text](img/5.gif)

## Data

Currently, data used is only seasonal normal temperature for each day, for each city.

City is described by its coordinates.

A report is described by its minimal temperature (TMin), maximal temperature (TMax) and date.

## Example

### Tatooine dataset

Only temperature is actually implemented, planet Tatooine (Star Wars' planet with an arid climate) can be taken as an example (considerating that they is no clouds in this planet)

![Alt text](img/screen1.png?raw=true "Screenshot")

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