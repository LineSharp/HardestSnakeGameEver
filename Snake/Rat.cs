using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{ 
    class Rat//rat model
    {
        public PictureBox ratImage;
        Image Image = Image.FromFile(@"../../sprites/rat.png");
        Random rnd;
        public Rat(Form activeForm)
        {
            ratImage = new PictureBox();
            rnd = new Random();
            ratImage = new PictureBox();
            MoveRat();

            ratImage.Height = 20;
            ratImage.Width = 20;
            ratImage.BackColor = Color.Transparent;
            ratImage.Image = Image;
            activeForm.Controls.Add(ratImage);
        }
        public void MoveRat()
        {
            //the rat is randomly placed on the form 
            ratImage.Location = new Point(rnd.Next(100, 400), rnd.Next(100, 400));
        }
    }
}
