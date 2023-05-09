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

DigitalFilm helps to transfer digital picture to analog print.

## Modes

### Mode 1 : Back & White

Just a black & White zone to check if the diplay panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2 : Find an exposure time for a specific gray

Generate a gray palette following parameters to compare with a specific value of gray.

<img src="/img/mode2.gif" width="300">

### Mode 3 : Test contrast

Draw shapes with different size to see the resolution of the system.

<img src="/img/mode3.bmp" width="300">

### Mode 4 : Gray vs B&W Linear"

Draw a gray palette and a B&W scale with a linear exposure time.
Gamma parameter is available.

<img src="/img/mode4.gif" width="300">

### Mode 5 : Display a picture following parameters

#### Display mode : Direct, convert a colored picture in grayscale and diplay it
Convert the picture to graysclale and display it with inverted colors (like films). 

<img src="/img/mode5.jpg" width="300"> <img src="/img/mode5inverted.jpg" width="300">

#### Display mode : Display a picture with GrayToTime algorythm

Display a picture with GrayToTime algorythm.

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

### Minor
- [/] README : corriger le mode 5 -> image inverser
- [ ] README : faire des pages spécifiques pour les filtres, films (non?) et papier ?
- [ ] intégrer photo du labo
- [/] Tester TopMost
- [X] Supprimer Mode6.cs ?
- [ ] frmMain : Supprimer Current time ?

- [X] Renomer duration ExposureTime
- [X] gérer les invertions en inversant le tableau dans Paper

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
