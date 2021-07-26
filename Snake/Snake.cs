using System;
using System.Collections.Generic;
using System.Drawing;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Snake
{
    class Snake
    {
        //SubClasses and enums 
        #region
        enum Direction
            {
                up,
                right,
                left,
                down
            }
        class BodyPiece
        {
            PictureBox bodyImage;
            public int Xpossition { get { return bodyImage.Location.X; } }
            public int Ypossition { get { return bodyImage.Location.Y; } }

            public BodyPiece(Form activeForm , Point startPossition)
            {
                bodyImage = new PictureBox();
                bodyImage.Width = 20;
                bodyImage.Height = 20;
                bodyImage.Image = Image.FromFile(@"../../sprites/Body.png");
                bodyImage.Location = startPossition;
                activeForm.Controls.Add(bodyImage);
            }
            public void SetPossition(Point point)
            {
                bodyImage.Location = point;
            }

        }
        class Head
        {

            public PictureBox headImage;
            public Direction Orientation;

            Image left = Image.FromFile(@"../../sprites/HeadLeft.png");
            Image right = Image.FromFile(@"../../sprites/HeadRight.png");
            Image up = Image.FromFile(@"../../sprites/HeadUp.png");
            Image down = Image.FromFile(@"../../sprites/HeadDown.png");

            public int Xpossition { get { return headImage.Location.X; } }
            public int Ypossition { get { return headImage.Location.Y; } }

            public Head(Form activeForm, Point startPossition)
            {
                headImage = new PictureBox();
                headImage.BackColor = Color.Transparent;
                headImage.Width = 20;
                headImage.Height = 20;
                headImage.Image = right;
                headImage.Location = startPossition;
                headImage.BackColor = Color.Transparent;
                activeForm.Controls.Add(headImage);
                headImage.Show();
                Orientation = Direction.right;
            }
            public void SetDirection(Direction direction)
            {
                this.Orientation = direction;
                //adjust image to fit movement 
                switch (Orientation)
                {
                    case Direction.up:
                        headImage.Image = up;
                        break;
                    case Direction.right:
                        headImage.Image = right;
                        break;
                    case Direction.left:
                        headImage.Image = left;
                        break;
                    case Direction.down:
                        headImage.Image = down;
                        break;
                    default:
                        break;
                }
            }
            public void SetPossition(Point point)
            {
                headImage.Location = point;
            }

            //the IntersectsWith methods is very useful when working with picture boxes 
            public bool IsColiding(PictureBox pictureBox)
            {
                return headImage.Bounds.IntersectsWith(pictureBox.Bounds);
            }

        }
        #endregion

        Head head;
        List<BodyPiece> bodyPieces;
        int speed;
        Form ActiveForm;
        int XpossitionForNewBodyPiece;
        int YpossitionForNewBodyPiece;
        public int Length { get { return bodyPieces.Count + 1; }  }

        public Rectangle HeadBounds { get { return head.headImage.Bounds; } }

        public Snake(Form ActiveForm)
        {
            this.ActiveForm = ActiveForm;
            head = new Head(ActiveForm, new Point(65, 200));
            bodyPieces = new List<BodyPiece>();
            speed = 21;
            CreateBody(4);

        }
        public bool IsSnakeHeadColiding(PictureBox pictureBox)
        {
            return head.IsColiding(pictureBox);
        }
        public bool IsSnakeColidingWithItself()
        {
            foreach (BodyPiece body in bodyPieces)
            {
                if (body.Xpossition == head.Xpossition && body.Ypossition == head.Ypossition)
                {
                    return true;
                }
            }
            return false;
        }
        public bool IsSnakeOutOfBounds()
        {
            return head.Xpossition > 466 || head.Xpossition < 20 || head.Ypossition > 445 || head.Ypossition < 20;
        }
        public void Eat()
        {
            bodyPieces.Add(new BodyPiece(ActiveForm, new Point(XpossitionForNewBodyPiece, YpossitionForNewBodyPiece)));
        }
        /// <summary>
        /// Moves the Snake in the direction that the head is facing
        /// </summary>
        private void MoveSnake()
        {
            //first we need to keep track of the position behind the snake so if we eat anything we know where to place a new body piece 
            XpossitionForNewBodyPiece = bodyPieces[bodyPieces.Count - 1].Xpossition;
            YpossitionForNewBodyPiece = bodyPieces[bodyPieces.Count - 1].Ypossition;
            //index of [bodyPieces.Count - 1] is the last item in the list 


            //moves each body piece to the location of the previous one in the list 
            for (int i = bodyPieces.Count -1; i > 0; i--)
            {
                bodyPieces[i].SetPossition(new Point( bodyPieces[i - 1].Xpossition, bodyPieces[i - 1].Ypossition));
            }

            //moves the first body piece to the previous location of the head
            bodyPieces[0].SetPossition(new Point(head.Xpossition, head.Ypossition));

            //finds witch direction to move the head next 
            switch (head.Orientation)
            {
                case Direction.up:
                    head.SetPossition(new Point(head.Xpossition, head.Ypossition - speed));
                    break;
                case Direction.right:
                    head.SetPossition(new Point(head.Xpossition + speed, head.Ypossition));
                    break;
                case Direction.left:
                    head.SetPossition(new Point(head.Xpossition - speed, head.Ypossition));
                    break;
                case Direction.down:
                    head.SetPossition(new Point(head.Xpossition, head.Ypossition + speed));
                    break;
                default:
                    break;
            }
        }

        private void CreateBody(int length)
        {
            for (int i = 1; i < length; i++)
            {
                bodyPieces.Add(new BodyPiece(ActiveForm, new Point(head.Xpossition - speed * i, head.Ypossition)));
            }
        }

        /// <summary>
        /// Changes the direction the snake is facing and moves the snake in that direction
        /// </summary>
        /// <param name="key"></param>
        public void KeyPressed(string key)
        {
            if (key.Equals("W") && !(head.Orientation == Direction.down))
            {
                head.SetDirection(Direction.up);
            }
            else if (key.Equals("S") && !(head.Orientation == Direction.up))
            {
                head.SetDirection(Direction.down);
            }
            else if (key.Equals("A") && !(head.Orientation == Direction.right))
            {
                head.SetDirection(Direction.left);
            }
            else if (key.Equals("D") && !(head.Orientation == Direction.left))
            {
                head.SetDirection(Direction.right);
            }
            else
            {
                //if th pressed key dosnt make any of the cases true then we dont want to move the snake 
                //so we return 
                return;
            }
            MoveSnake();
        }
    }
}
