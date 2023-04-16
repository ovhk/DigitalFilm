# Digital Darkroom

How to print a picture from a film nagative?

- put the negative into the enlarger 
- project the negative into the paper

No! Here is the solution:

- scan the negative, invert colors and save it
- Find experimentaly the exposition duration to have different value of gray on the paper
- put gray and duration value in an Excel file, draw the trend curve and get its formula
- generate 256 bitmap from your scanned picture. One for each gray value, keep black all pixels lower than these value, put all others in white
- put the display panel on the paper and turn on the enlarger light
- display these bitmaps during the duration calculate with the trend formula onto a transparent black and white 8K display panel

Tada!

Digital Darkroom helps to transfer digital picture to analog print.

## Modes

### Mode 1 : Back & White

Just a black & White zone to check if the diplay panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2 : Find duration for a specific gray

<img src="/img/mode2.gif" width="300">

### Mode 3 : Test contrast

<img src="/img/mode3.bmp" width="300">

### Mode 4 : Gray vs B&W Linear"

<img src="/img/mode4.gif" width="300">

### Mode 5 : Convert a colored picture in grayscale and diplay it

<img src="/img/mode5.jpg" width="300">

### Mode 6 : Display a picture with GrayToTime algorythm

<img src="/img/mode6.gif" width="300">

### Mode 7 : Gray vs GrayToTime

<img src="/img/mode7.gif" width="300">

### Mode 8 : Test band

<img src="/img/mode8.gif" width="300">

(Thanks to https://ezgif.com/maker for GIFs)

---

## TODO list

### Minor
- [ ] frmMain manager time
- [ ] DisplayEngine: better use of engine status
- [ ] Test parallel Programming
- [ ] Cleanup code

### Major
- [ ] Add GrayToTime formula in parameter and evaluate it on the fly?
- [ ] Add mask managment

---

## Ideas

if trend nedded https://stackoverflow.com/questions/40269793/replicate-excel-power-trendline-values-with-c-sharp
