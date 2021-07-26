using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Text;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
/// <summary>
/// Todays end summary, tested adding everything
/// todo tmrw: make collider and everytime snake eats mouse increase size and if snake touches itself make it die
/// </summary>
/// 
///Todo: make snek move, done
///todo tmrw: make snake head rotate proper and add eating mechanism, done
///tode :make the rat die and respawn everytime the snake eats it
///done: made the snake move properly and the pieces get added properly
///next: make snake die when touched by head, make mouse move and add the othere difficulty items its easy going now, done
///.
///Todo : add main menu, moving saws and incrementally increasing mines and sounds
namespace Snake
{
    class MainForm : Form//our view/controller
    {
        //Declarations
        #region
        Image backGround = Image.FromFile(@"../../sprites/BG.png");
        private Snake snakeObj;

        public Form theForm;
        Control.ControlCollection FormAdder;
        private static Random rng;
        private Rat rat;
        PrivateFontCollection bitFont = new PrivateFontCollection();
        Label scoreBoard = new Label();

        List<Mine> Mines;
        Blade blade;

        //to count the number of  millerseconds that have passed 
        Thread TimerThread;


        //blade related
        #region
        bool respawnUB = true;
        bool respawnDB = true;
        bool respawnLB = true;
        bool respawnRB = true;

        PictureBox uB= new PictureBox();
        PictureBox dB = new PictureBox();
        PictureBox lB = new PictureBox();
        PictureBox rB = new PictureBox();

        #endregion
        #endregion
        public MainForm()
        {

            //lets initialize our font collection, private font collecttion it is a font family ie it is basically a list of ttf files
            bitFont.AddFontFile(@"../../fonts/bitFont.ttf");



            //define the forms properties
            this.Height = 500;
            this.Width = 500;
            this.BackgroundImage = backGround;
            this.Text = "Snake";
            this.Icon = new Icon(@"../../sprites/Icon.ico");
            this.KeyPreview = true;//allows us to read key strokes
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.DoubleBuffered = true;
            theForm = this;
            FormAdder = theForm.Controls;

            //initialize the scoreboard
            scoreBoard.SetBounds(200, 30, 100, 40);
            scoreBoard.Text = "0";
            scoreBoard.Image = Image.FromFile(@"../../sprites/ScoreBoard.png");
            scoreBoard.BackColor = Color.Transparent;
            scoreBoard.Font = new Font(bitFont.Families[0], 15);
            scoreBoard.TextAlign = ContentAlignment.MiddleCenter;

            //adding the border walls
            Walls wall = new Walls(theForm);

            //create the list of mines 
            Mines = new List<Mine>();

            //init the randomizer
            rng = new Random();


            //lets place the uninitiable objects off reach
            uB.Location = new Point(-100, -100);
            dB.Location = new Point(-100, -100);
            lB.Location = new Point(-100, -100);
            rB.Location = new Point(-100, -100);
            //instantiate the snake
            snakeObj = new Snake(this);//gets called and updated when ever the snake is called
            rat = new Rat(this);

            this.KeyDown += new KeyEventHandler(KeyPressed);

            //TimerThread = new Thread(TimerTick);
            //this.FormClosing += MainForm_FormClosing;
            //TimerThread.Start();

            FormAdder.Add(scoreBoard);            
        }

        /// <summary>
        /// IMPORTENT this is to stop the thread when the form closes to stop a memory leak 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            TimerThread.Abort();
        }

        //blade related
        #region
        private async void UpBladeSpawner()
        {
            while (respawnUB)
            {
                blade = new Blade();
                (PictureBox,PictureBox) up = blade.InitUpBlade();
                PictureBox upShooter = up.Item2;
                PictureBox upBlade = up.Item1;
                theForm.Controls.Add(upShooter);
                theForm.Controls.Add(upBlade);
                respawnUB = false;
                theForm.Paint += new PaintEventHandler(blade.MoveForward);
                uB = upBlade;
                await Task.Delay(7000);
                theForm.Controls.Remove(upBlade);
                upBlade.Dispose();
                respawnUB = true;
            }
        }
        private async void DownBladeSpawner()
        {
            while (respawnDB)
            {
                blade = new Blade();
                (PictureBox, PictureBox) down = blade.InitDownBlade();
                PictureBox downShooter = down.Item2;
                PictureBox downBlade = down.Item1;
                theForm.Controls.Add(downShooter);
                theForm.Controls.Add(downBlade);
                respawnDB = false;
                theForm.Paint += new PaintEventHandler(blade.MoveBackward);
                dB = downBlade;
                await Task.Delay(7000);
                theForm.Controls.Remove(downBlade);
                downBlade.Dispose();
                respawnDB = true;
            }
        }
        private async void LeftBladeSpawner()
        {
            while (respawnLB)
            {
                blade = new Blade();
                (PictureBox, PictureBox) left = blade.InitLeftBlade();
                PictureBox leftShooter = left.Item2;
                PictureBox leftBlade = left.Item1;
                theForm.Controls.Add(leftShooter);
                theForm.Controls.Add(leftBlade);
                respawnLB = false;
                theForm.Paint += new PaintEventHandler(blade.MoveUp);
                lB = leftBlade;
                await Task.Delay(7000);
                theForm.Controls.Remove(leftBlade);
                leftBlade.Dispose();
                respawnLB = true;
            }
        }
        private async void RightBladeSpawner()
        {
            while (respawnRB)
            {
                blade = new Blade();
                (PictureBox, PictureBox) right = blade.InitRightBlade();
                PictureBox rightShooter = right.Item2;
                PictureBox rightBlade = right.Item1;
                theForm.Controls.Add(rightShooter);
                theForm.Controls.Add(rightBlade);
                respawnRB = false;
                theForm.Paint += new PaintEventHandler(blade.MoveDown);
                rB = rightBlade;
                await Task.Delay(7000);
                theForm.Controls.Remove(rightBlade);
                rightBlade.Dispose();
                respawnRB = true;
            }
        }
        private void BladeUpdater()//this method may look wierdd and funny but this way all of them will execute without having to type in extra 5 line, so fair is fair
        {
            int Score = snakeObj.Length - 3;
            if (Score >= 5)
            {
                UpBladeSpawner();
                if (Score >= 10)
                {
                    DownBladeSpawner();
                    if (Score >= 15)
                    {
                        LeftBladeSpawner();
                        if (Score >= 20)
                        {
                            RightBladeSpawner();
                        }
                    }
                }
            }
        }
        #endregion

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            string KeyCode = e.KeyCode.ToString();
            snakeObj.KeyPressed(KeyCode);
            IsRatEaten();
            CheckIfSnakeIsDead();

        }

        /// <summary>
        /// adds 1 to the score
        /// </summary>
        private void IncreceScore()
        {
            scoreBoard.Text = (snakeObj.Length - 4).ToString();
        }
        /// <summary>
        /// creates a new rat and adds 1 to the score
        /// </summary>
        private void RatHasBeanEaten()
        {
            ActiveForm.Controls.Remove(rat.ratImage);
            snakeObj.Eat();
            rat = new Rat(Form.ActiveForm);
            IncreceScore();
            
            //every time a rat is eaten new mines will spawn 
            MineSpawner();

            BladeUpdater();

        }


        /// <summary>
        /// checks if the snakes head and the rat are colliding and if so then the rat is eaten 
        /// </summary>
        public void IsRatEaten()
        {
            if (snakeObj.IsSnakeHeadColiding(rat.ratImage))
            {
                RatHasBeanEaten();
            }
        }


        //ignore just some random stuff i tested with 
        #region

        int time;
        /// <summary>
        /// increces time by 1 every millersecond 
        /// this will be usefull when updating the blades 
        /// </summary>
        /// 

        ///testing with threds 
        //private void TimerTick()
        //{
        //    while (TimerThread.IsAlive)
        //    {
        //        if (ActiveForm != null)
        //        {
        //            time++;
        //            Form.ActiveForm.BeginInvoke(new MethodInvoker(delegate () { TimeLoop(); }));
        //            Thread.Sleep(1);
        //        }

        //    }
        //}

        //private void TimeLoop()
        //{
        //    time++;
        //    if (time % 3000 == 0)
        //    {
        //        MessageBox.Show("Test");
        //    }
        //}


        #endregion
        private void MineSpawner()
        {
            //adding modifiers to this variable will change the number of mines that will spawn 
            int NumberOfMinesToAdd = snakeObj.Length;
            for (int i = 0; i < NumberOfMinesToAdd; i++)
            {
                Mines.Add(new Mine(ActiveForm));
            }
        }

        private void CheckIfSnakeIsDead()
        {
            //checks if the snakes head is in the same position as any body pieces 
            if (snakeObj.IsSnakeColidingWithItself())
            {
                GameOver();
            }

            //checks if the snake is touching a wall 
            if (snakeObj.IsSnakeOutOfBounds())
            {
                GameOver();
            }

            foreach (Mine mine in Mines)
            {
                if (mine.mine.Bounds.IntersectsWith(snakeObj.HeadBounds))
                {
                    GameOver();
                    break;
                }
            }

            if (dB.Bounds.IntersectsWith(snakeObj.HeadBounds) ||uB.Bounds.IntersectsWith(snakeObj.HeadBounds) || lB.Bounds.IntersectsWith(snakeObj.HeadBounds)|| rB.Bounds.IntersectsWith(snakeObj.HeadBounds))
            {
                GameOver();
            }
            

        }

        private void GameOver()
        {
            string message = $"You Died\nYou Scored: {snakeObj.Length - 4}\nDo you want to retry?";
            string title = "Oh No!";
            DialogResult result= MessageBox.Show(message, title, MessageBoxButtons.YesNo);
            switch (result)
            {
                case DialogResult.Yes:
                    Application.Restart();
                    break;
                case DialogResult.No:
                    Application.Exit();
                    break;
            }
        }
    }
}
