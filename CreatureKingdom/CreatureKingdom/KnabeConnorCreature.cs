﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Windows.Controls;
using System.Windows.Media.Imaging;

using System.Windows.Threading;
using System.Threading;
using System.Windows;

namespace CreatureKingdom {
    class KnabeConnorCreature :Creature {
        

        Image dogImage;
        BitmapImage leftBitmap;
        BitmapImage rightBitmap;
        private Thread posnThread = null;
        private Boolean goRight = true;
        double dogWidth = 356;
        double incrementSize = 2.0;
        double kingdomWidth = 0;

        public KnabeConnorCreature(Canvas kingdom, Dispatcher dispatcher, Int32 waitTime = 100)
            : base(kingdom, dispatcher, waitTime) {
            dogImage = new Image();
            leftBitmap = LoadBitmap(@"KnabeConnor\dogLeft.gif", dogWidth);
            rightBitmap = LoadBitmap(@"KnabeConnor\dogRight.gif", dogWidth);
        }

        public override void Shutdown(){
            if (posnThread != null) {
                posnThread.Abort();
            }
        }

        public override void Place(double x, double y){
            dogImage.Source = rightBitmap;
            goRight = true;

            this.x = x;
            this.y = y;
            kingdom.Children.Add(dogImage);
            dogImage.SetValue(Canvas.LeftProperty, this.x);
            dogImage.SetValue(Canvas.TopProperty, this.y);

            posnThread = new Thread(Position);
            posnThread.Start();
        }

        void Position() {
            while (true) {
                if (goRight && !Paused) {
                    x += incrementSize;
                    if (x > kingdomWidth) {
                        goRight = false;
                        SwitchBitmap(leftBitmap);
                    }
                } else if (!Paused) {
                    x -= incrementSize;
                    if (x < 0) {
                        goRight = true;
                        SwitchBitmap(rightBitmap);
                    }
                }
                if (kingdomWidth != kingdom.RenderSize.Width - dogWidth){
                    kingdomWidth = kingdom.RenderSize.Width - dogWidth;
                }
                UpdatePosition();
                Thread.Sleep(WaitTime);
            }
        }

        void UpdatePosition() {
            Action action = () => { dogImage.SetValue(Canvas.LeftProperty, x); dogImage.SetValue(Canvas.TopProperty, y); };
            dispatcher.BeginInvoke(action);
        }

        void SwitchBitmap(BitmapImage theBitmap) {
            Action action = () => { dogImage.Source = theBitmap; };
            dispatcher.BeginInvoke(action);
        }

        /*
        private void Size_Changed(SizeChangedEventArgs e) {
            Console.WriteLine("Window has been resized");
        }
         */
    }
}
