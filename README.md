# Digital Film

How to print a picture from a film negative?

- put the negative into the enlarger 
- project the negative on the paper

No! Here is the solution:

- scan the negative, invert colors and save it
- Find experimentaly the exposure time to have different values of gray on the paper
- put gray values and exposure time in an Excel file, draw the trend curve and get its formula
- generate 256 bitmaps from your scanned picture. One for each gray value, keep black all pixels lower than this value, put all others in white
- put the display on the paper and turn on the enlarger light
- display these bitmaps during the exposure time calculate with the trend formula onto a transparent black and white display panel

Tada!

DigitalFilm helps to transfer digital picture to analog print through a transparent screen. In a way, it's a digital film.

<img src="/img/labo1.jpg" width="300"><img src="/img/labo2.jpg" width="300"><img src="/img/labo3.jpg" width="300">

## The method

Basicly, negatives films have a gamma of arround 0.7 and are printed on photographic paper with a gamma of arround 1.4. 
0.7 × 1.4 = 1.0, that's why the picture looks like the original view.
So the idea if you want to use a screen as a film, is to apply the invert of the gamma of the paper and invert black and white.
Then, you have to put the screen directly on the paper and light it up.

## The screen

This type of screen is usually dedicated for resin 3D printers.
These tests are made with a 10.3", 8K (7680 x 4320) monochrome from <a href="https://wisecocodisplay.com/">Wisecoco</a>.
The datasheet is <a href="DisplayDatasheets/TOP103MONO8K01A  10.3 inch 黑白屏.pdf">here</a>.

<img src="/img/labo4.jpg" width="300">

## The paper

Papers have a caracteristic curve with density and relative log exposure.
Remember: more light = more density = darker.

<img src="/img/FomaspeedVariantIII.png" width="300">

So the idea is to extract the data of the curve and convert the density to 256 gray level.

Tests here are made with a Famaspeed Variant III RC paper : https://www.foma.cz/en/fomaspeed-variant-III

## The application

The application is made with different mode for diffrent tests.

### Mode 1 : Back & White

Just a black & White zone to check if the diplay panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2 : Find an exposure time for a specific gray

Generate a gray palette following parameters to compare with a specific value of gray.

<img src="/img/mode2.gif" width="300">

### Mode 3 : Test contrast

Draw shapes with different size to see the resolution of the system.

<img src="/img/mode3.bmp" width="300">

### Mode 4 : Gray vs B&W Linear

Draw a gray palette and a B&W scale with a linear exposure time.
Gamma parameter is available.

<img src="/img/mode4.gif" width="300">

### Mode 5 : Display a picture following parameters

#### Display mode : Direct, convert a colored picture in grayscale and diplay it
Convert the picture to graysclale and display it with inverted colors (like films) adapted for the specified paper and grade.

TODO : avec cette méthode on utilise pas l'ensemble de la palette de gris donc on se retrouve avec des artefacts. Une solution est d'utiliser le mode GrayToTime.

<img src="/img/mode5inverted.png" width="300">

#### Display mode : Display a picture with GrayToTime algorythm

Display a picture with GrayToTime algorythm based on the article of Pierre MUTH : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/

<img src="/img/mode6.gif" width="300">

### Mode 7 : Gray vs GrayToTime

Draw a gray palette and a B&W scale with GrayToTime algorythm to find the exposure time.

<img src="/img/mode7.gif" width="300">

### Mode 8 : Test band

Draw test band.

<img src="/img/mode8.gif" width="300">

### Mode 9 : Gray palette

Generate a gray palette with gamma parameter.

<img src="/img/mode9.bmp" width="300">

## Thanks

- Many thanks to http://ateliersuper8.com!
- Thanks to https://ezgif.com/maker for animated GIFs

---

## TODO list

https://tech.snmjournals.org/content/jnmt/20/2/62.full.pdf

- [ ] https://www.35mmc.com/07/02/2022/contrast-and-tonality-part-3-characteristic-curves-for-film-and-paper-by-sroyon/

https://www.covingtoninnovations.com/dslr/curves.html

### Minor
- [ ] faire affiche pour l'AS8
- [ ] frmMain : Supprimer Current time ?
- [ ] tester les courbes
- [ ] est-ce que l'on fait un filtre pour les papiers mats et satinés (réduire Dmax) ?

- [X] intégrer photos du labo

### Major
- [ ] Test parallel Programming
- [ ] Racadrer les images ?
- [ ] Add Images configuration system : add, remove with custom params on each steps
- [ ] Add GrayToTime configuration system
      can show the impact on picture on real time
- [ ] Add GrayToTime formula in parameter and evaluate it on the fly?
      if trend nedded https://stackoverflow.com/questions/40269793/replicate-excel-power-trendline-values-with-c-sharp
- [ ] Add mask managment

---

Thanks for your comments!
