# Digital Film

DigitalFilm helps to transfer a digital picture to analog print through a transparent screen. In a way, it's a digital film.

<img src="/img/labo1.jpg" width="300"><img src="/img/labo2.jpg" width="300"><img src="/img/labo3.jpg" width="300">


## Method 1: Direct

Basically, negative films have a gamma of around 0.7 and are printed on photographic paper with a gamma of around 1.4.<br /> 
0.7 × 1.4 = 1.0, that's why the picture looks like the original view.<br />
So the idea if you want to use a screen as a film, is to apply the invert of the gamma of the paper and invert black and white.
Then, you have to put the screen directly on the paper and light it up.

In Direct mode, it converts the picture to grayscale and display it with inverted colors (like films), adapted for the specified paper and grade.
With this method, you don't use the entire gray palette (because of the extrapolation), so you end up with artifacts. One solution is to use the GrayToTime mode.

<img src="/img/mode direct synoptic.drawio.png" width="800"><br /><br />
<img src="/img/mode direct.drawio.png" width="400"><br /><br />
<img src="/img/mode5inverted.png" width="300">
<br />

## Method 2: GrayToTime

Display a picture with GrayToTime algorithm based on the article of Pierre MUTH : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/
In this mode, each value of gray is converted to an exposure time.
The function transfer gray to time is built by experimentation. It depends especially on your light source.

The other release (Custom) of the algorithm is based on the calibration performed with the mode 6. The C# formula is a parameter and it will be evaluated on the fly. 

<img src="/img/mode graytotime synoptic.drawio.png" width="1024"><br /><br />
<img src="/img/mode graytotime.drawio.png" width="600"><br /><br />
<img src="/img/mode6.gif" width="300">
<br />

## Screen

This type of screen is usually dedicated for resin 3D printers with UV light.<br />
These tests are made with a 10.3", 8K (7680 x 4320) monochrome from <a href="https://wisecocodisplay.com/">Wisecoco</a>.
The datasheet is <a href="DisplayDatasheets/TOP103MONO8K01A  10.3 inch 黑白屏.pdf">here</a>.
<br />
Why transparent? Just because it's actually the way to have a small screen with a very high resolution.

<img src="/img/labo4.jpg" width="300">

## Papers

Papers have a characteristic curve with density and relative log exposure.<br />
Remember: more light = more density = darker.

<img src="/img/FomaspeedVariantIII WPD.png" width="400">

So you need to extract the data of the curve and convert the density to 256 gray levels.<br />
Data are extracted with https://automeris.io/WebPlotDigitizer/<br />
<br />
Tests here are made with a Famaspeed Variant III RC paper (matt and glossy) : https://www.foma.cz/en/fomaspeed-variant-III
Thanks to Foma for getting me few usefull information.

## Application

The application is made with different modes for different tests.<br />
<br />
<img src="/img/DigitalFimPrintScreen.png" width="600">

### Mode 1: Back & White

Just a black & White zone to check if the display panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2: Find an exposure time for a specific gray.

Generate a gray palette following parameters to compare with a specific value of gray.

<img src="/img/mode2.gif" width="300">

### Mode 3: Test contrast

Draw shapes with different size to see the resolution of the system.

<img src="/img/mode3.bmp" width="300">

### Mode 4: Gray vs. B&W Linear

Draw a gray palette and a B&W scale with a linear exposure time.
The gamma parameter is available.

<img src="/img/mode4.gif" width="300">

### Mode 5: Display a picture following parameters

2 modes are available: Direct and GrayToTime. 

### Mode 6: GrayToTime calibration

The tricky thing is now to determine the exposure time to get each specific gray. The Mode 2 is very difficult to interpret, so the idea here is to generate a matrix of gray with different exposure from a minimum to a maximum value. The interval is calculated from the number of squares in the matrix.

With this matrix, we will use a RGB sensor (from https://atlas-scientific.com/probes/color-sensor/) and the Sensor Color form from the menu to measure the gray value.
Before starting, you will need to perform a calibration on a pure white on your paper.
With this data, you will be able to fill the GrayToTime Excel file, to draw the curve and to get the trend formula to put inside the algorithm.

<img src="/img/EZ-RGB use.jpg" width="400"> 
<img src="/img/sensor capture.png" width="400">
<img src="/img/Excel sensor.png" width="400">

### Mode 7: Gray vs. GrayToTime

Draw a gray palette and a B&W scale with GrayToTime algorithm to find the exposure time.

<img src="/img/mode7.gif" width="300">

### Mode 8: Test band

Draw test band.

<img src="/img/mode8.gif" width="300">

### Mode 9: Gray palette

Generate a gray palette with the gamma parameter.

<img src="/img/mode9.bmp" width="300">

## Thanks

- Many thanks to http://ateliersuper8.com!

---

## Reference
- https://tech.snmjournals.org/content/jnmt/20/2/62.full.pdf
- https://www.35mmc.com/07/02/2022/contrast-and-tonality-part-3-characteristic-curves-for-film-and-paper-by-sroyon/
- https://www.covingtoninnovations.com/dslr/curves.html
- https://nvlpubs.nist.gov/nistpubs/jres/7/jresv7n3p495_A2b.pdf
- https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/
- https://ezgif.com/maker

---

Thanks for your comments!
