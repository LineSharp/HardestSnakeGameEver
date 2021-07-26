using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{ 
    class Mine//mine model
    {
        public PictureBox mine = new PictureBox();
        static Image mineImage = Image.FromFile(@"../../sprites/Mine.png");

        public Mine(Form activeForm)
        {
            Random rnd = new Random();

            mine.Height = 20;
            mine.Width = 20;
            //i am doing * 21 so the mines will be in a grid like format 
            mine.Location = new Point(rnd.Next(1,20) * 21,rnd.Next(1,20) * 21);
            mine.BackColor = Color.Transparent;
            mine.Image = mineImage;
            activeForm.Controls.Add(mine);
        }
    }
}
