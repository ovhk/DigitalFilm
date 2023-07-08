# Digital Film

DigitalFilm helps to transfer digital picture to analog print through a transparent screen. In a way, it's a digital film.

<img src="/img/labo1.jpg" width="300"><img src="/img/labo2.jpg" width="300"><img src="/img/labo3.jpg" width="300">


## Method

Basically, negative films have a gamma of around 0.7 and are printed on photographic paper with a gamma of around 1.4.<br /> 
0.7 × 1.4 = 1.0, that's why the picture looks like the original view.<br />
So the idea if you want to use a screen as a film, is to apply the invert of the gamma of the paper and invert black and white.
Then, you have to put the screen directly on the paper and light it up.

## Screen

This type of screen is usually dedicated for resin 3D printers with UV light.<br />
These tests are made with a 10.3", 8K (7680 x 4320) monochrome from <a href="https://wisecocodisplay.com/">Wisecoco</a>.
The datasheet is <a href="DisplayDatasheets/TOP103MONO8K01A  10.3 inch 黑白屏.pdf">here</a>.
<br />
Why transparent? Just because it's actually the way to have a small screen with very high resolution.

<img src="/img/labo4.jpg" width="300">

## Papers

Papers have a characteristic curve with density and relative log exposure.<br />
Remember: more light = more density = darker.

<img src="/img/FomaspeedVariantIII.png" width="300">

So you need to extract the data of the curve and convert the density to 256 gray levels.<br />
<br />
Tests here are made with a Famaspeed Variant III RC paper (matt and glossy) : https://www.foma.cz/en/fomaspeed-variant-III
Thanks to Foma for getting me few usefull information.

## Application

The application is made with different mode for different tests.
<br />
<img src="/img/DigitalFimPrintScreen.png" width="500">

### Mode 1 : Back & White

Just a black & White zone to check if the display panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2 : Find an exposure time for a specific gray

Generate a gray palette following parameters to compare with a specific value of gray.

<img src="/img/mode2.gif" width="300">

### Mode 3 : Test contrast

Draw shapes with different size to see the resolution of the system.

<img src="/img/mode3.bmp" width="300">

### Mode 4 : Gray vs. B&W Linear

Draw a gray palette and a B&W scale with a linear exposure time.
The gamma parameter is available.

<img src="/img/mode4.gif" width="300">

### Mode 5 : Display a picture following parameters

#### Display mode : Direct, convert a colored picture in grayscale and display it
Convert the picture to grayscale and display it with inverted colors (like films), adapted for the specified paper and grade.
With this method, you don't use the entire gray palette (because of the extrapolation), so you end up with artifacts. One solution is to use the GrayToTime mode.

<img src="/img/mode5inverted.png" width="300">

#### Display mode : Display a picture with GrayToTime algorithm
Display a picture with GrayToTime algorithm based on the article of Pierre MUTH : https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/
In this mode, each value of gray is converted to an exposure time.
The function transfer gray to time is built by experimentation, it depends especially of your light source.

<img src="/img/mode6.gif" width="300">

### Mode 7 : Gray vs. GrayToTime

Draw a gray palette and a B&W scale with GrayToTime algorithm to find the exposure time.

<img src="/img/mode7.gif" width="300">

### Mode 8 : Test band

Draw test band.

<img src="/img/mode8.gif" width="300">

### Mode 9 : Gray palette

Generate a gray palette with the gamma parameter.

<img src="/img/mode9.bmp" width="300">

## Thanks

- Many thanks to http://ateliersuper8.com!

---

## TODO list

### Creative TODO list
- [ ] Multiple exposure : film + screen
    - [ ] With edge detection
    - [ ] digital texture background

### Next release
- [ ] tester les courbes
- [ ] Générer et imprimer une feuille A3 de référence de gris avec trou au milieu
- [/] Faire une image avec les 5 niveaux de grade
- [/] Faire un dérivé de l'external panel pour modifier le ratio
- [X] Faire un histogramme

- [ ] faire affiche pour l'AS8
- [ ] est-ce que l'on fait un filtre pour les papiers mats et satinés (réduire Dmax) ?

### Futur release
- [ ] Parallel Programming : in 4K or 8K, generate 256 bitmap take times. Thanks for the cache system.
- [ ] Reframe picture : maybe it has to be done in a true image manupulation program
- [ ] Add mask managment
- [ ] Add GrayToTime configuration system can show the impact on picture on real time
- [ ] Add GrayToTime formula in parameter and evaluate it on the fly?
      if trend nedded https://stackoverflow.com/questions/40269793/replicate-excel-power-trendline-values-with-c-sharp
- [ ] Add Images configuration system : add, remove with custom params on each steps

## Reference
- https://tech.snmjournals.org/content/jnmt/20/2/62.full.pdf
- https://www.35mmc.com/07/02/2022/contrast-and-tonality-part-3-characteristic-curves-for-film-and-paper-by-sroyon/
- https://www.covingtoninnovations.com/dslr/curves.html
- https://pierremuth.wordpress.com/2020/04/18/digital-picture-to-analog-darkroom-print/
- https://ezgif.com/maker

---

Thanks for your comments!
