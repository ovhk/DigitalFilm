# Digital Film

How to print a picture from a film negative?

- put the negative into the enlarger 
- project the negative into the paper

No! Here is the solution:

- scan the negative, invert colors and save it
- Find experimentaly the exposition duration to have different value of gray on the paper
- put gray value and duration in an Excel file, draw the trend curve and get its formula
- generate 256 bitmaps from your scanned picture. One for each gray value, keep black all pixels lower than this value, put all others in white
- put the display on the paper and turn on the enlarger light
- display these bitmaps during the duration calculate with the trend formula onto a transparent black and white display panel

Tada!

DigitalFilm helps to transfer digital picture to analog print.

## Modes

### Mode 1 : Back & White

Just a black & White zone to check if the diplay panel is able to block light.

<img src="/img/mode1.png" width="300">

### Mode 2 : Find duration for a specific gray

Generate a gray palette following parameters to compare with a specific value of gray.

<img src="/img/mode2.gif" width="300">

### Mode 3 : Test contrast

Draw shapes with different size to see the resolution of the system.

<img src="/img/mode3.bmp" width="300">

### Mode 4 : Gray vs B&W Linear"

Draw a gray palette and a B&W scale with a linear duration.
Gamma parameter is available.

<img src="/img/mode4.gif" width="300">

### Mode 5 : Convert a colored picture in grayscale and diplay it

Convert the picture to graysclale and display it with inverted colors (like films). 

<img src="/img/mode5.jpg" width="300"> <img src="/img/mode5inverted.jpg" width="300">

### Mode 6 : Display a picture with GrayToTime algorythm

Display a picture with GrayToTime algorythm.

<img src="/img/mode6.gif" width="300">

### Mode 7 : Gray vs GrayToTime

Draw a gray palette and a B&W scale with GrayToTime algorythm to find the duration.

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

### Minor
- [/] README : corriger le mode 5 -> image inverser
- [ ] README : fusionner Mode 5 et 6
- [ ] README : faire des pages sp�cifiques pour les filtres, films et papier ?
- [ ] int�grer photo du labo
- [ ] Manage parameter with cache : error if useCache=true with different parameter 
- [ ] Add IFilter ou IGradeFilter ?
- [ ] Add IFilm ---> NO, we want to render our picture as is on the selected paper
- [ ] Add IPaper
- [/] frmMain : Add Preview button
- [/] frmMain : Add combo pour selectionner les papiers
- [/] Tester TopMost
- [ ] Renommer github et le lien dans frmAbout

- [X] Renomer en DigitalFilm 
- [X] Bug sur les marges en mode stretched
- [X] BitmapTools add gamma
- [X] manage multiple GrayToTime algo
- [X] Algo graytotime OVH
- [X] Mode 2 : Find duration for a gray : manque l'initial value dans le texte
- [X] corriger les inversions de couleur N&B dans gray palette
- [X] Mode 5 : param�trage de la mise � l'�chelle (voir PictureBox : Stretch, Centered ?)
- [X] Mode 6 : param�trage de la mise � l'�chelle (voir PictureBox : Stretch, Centered ?)
- [X] Mode 5 : Ajouter des marges noir ou blanches param�trables
- [X] Mode 6 : Ajouter des marges noir ou blanches param�trables
- [X] Mode 5 : Ajouter rotation d'image
- [X] Mode 6 : Ajouter rotation d'image
- [X] Fusionner mode 5 et 6 avec un param�tre ?
- [X] Mode 2 : ajouter offset de temps
- [X] ajouter timer dans frmMain

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