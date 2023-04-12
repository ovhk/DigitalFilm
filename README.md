# Digital Darkroom

How to print a picture from a film nagative.

- put the negative into the enlarger 
- project the negative into the paper

No! Here is the solution:

- scan the negative, invert colors and save it
- Find experimentaly the exposition duration to have different value of gray on the paper
- put gray and duration value in an Excel file, draw the trend curve and get its formula
- generate 256 bitmap from your scanned picture. One for each gray value, keep black all pixels lower than these value, put all others in white
- display these bitmaps during the duration calculate with the trend formula onto a transparent black and white 8K panel
- put the display on the paper and turn on the enlarger light

Tada!

Digital Darkroom helps to transfer digital picture to analog print.

---

## TODO list

### Minor
- [X] Create a cache system : use image checksum to put bmp in a folder and if checksum exists load existing bmp
- [X] Clean *.bmp when loading mode. usefull if cache management? -> this is a bug when you use diffrent mode in the same session
- [ ] Find a way to adapt Drawing Font following resolution
- [ ] frmMain manager time
- [ ] DisplayEngine: better use of engine status
- [X] Add in XLS file a minimum duration : 80ms ?
- [ ] Test parallel Programming
- [ ] Cleanup code
- [ ] Remove ImageLayerFile and create an ImageLayerCollection? 

### Major
- [ ] Add GrayToTime formula in parameter and evaluate it on the fly?
- [ ] Add mask managment

### Questions
- [ ] Keep menu or not?

---

## Ideas

if trend nedded https://stackoverflow.com/questions/40269793/replicate-excel-power-trendline-values-with-c-sharp
