using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using WpfApp1.Model;

namespace WpfApp1.ViewModel
{
    public class SnakeViewModel : NotifiableObject
    {
        public Snake snake;
        public System.Windows.Point apple;
        public List<int> freeSpots;
        public Queue<Direction> directionQueue;
        public Bitmap bitmap;

        private string[] urls = new string[] {
            "https://cdn.pixabay.com/photo/2015/02/28/15/25/snake-653639__340.jpg",
            "https://cdn.pixabay.com/photo/2018/04/06/11/49/snake-3295605__340.jpg",
            "https://cdn.pixabay.com/photo/2012/10/10/05/07/grass-snake-60546__340.jpg",
            "https://cdn.pixabay.com/photo/2011/07/25/23/40/green-tree-python-8343__340.jpg",
            "https://cdn.pixabay.com/photo/2016/12/08/17/32/rattlesnake-1892417__340.jpg"};
            

        private string _length;
        public string Length
        {
            get
            {
                return _length;
            }
            set
            {
                _length = value;
                OnPropertyChanged("Length");
            }
        }
        public SnakeViewModel()
        {
            snake = new Snake(Params.fields);
            Length = "Length: " + snake.Segments.Count.ToString();
            InitializeGame();
            GetBitmap();
        }

        public void InitializeGame()
        {
            snake = null;
            snake = new Snake(Params.fields);
            directionQueue = null;
            directionQueue = new Queue<Direction>();
            InitializeFreeSpots();
            PlaceApple();
        }

        public bool CheckEnd()
        {
            if (snake.Segments[0].X < 0 || snake.Segments[0].X >= Params.fields) return true;
            if (snake.Segments[0].Y < 0 || snake.Segments[0].Y >= Params.fields) return true;

            //int hor = 0, ver = 0; // diff between front and other segments
            //switch (snake.Direction)
            //{
            //    case Direction.Up:
            //        hor = 0;
            //        ver = 1;
            //        break;
            //    case Direction.Right:
            //        hor = -1;
            //        ver = 0;
            //        break;
            //    case Direction.Down:
            //        hor = 0;
            //        ver = -1;
            //        break;
            //    case Direction.Left:
            //        hor = 1;
            //        ver = 0;
            //        break;
            //}
            for (int i = 1; i < snake.Segments.Count - 1; ++i)
            {
                //if (snake.Segments[0].X - snake.Segments[i].X == hor &&
                //    snake.Segments[0].Y - snake.Segments[i].Y == ver)
                //{
                //    return true;
                //}
                if (snake.Segments[0].X == snake.Segments[i].X &&
                   snake.Segments[0].Y == snake.Segments[i].Y)
                {
                    return true;
                }
            }
            return false;
        }

        public void SetLength()
        {
            Length = "Length: " + snake.Segments.Count.ToString();
        }

        public bool CheckEatingApple()
        {
            switch (snake.Direction)
            {
                case Direction.Up:
                    if (snake.Segments[0].Y - apple.Y == 1 && snake.Segments[0].X == apple.X)
                        return true;
                    break;
                case Direction.Right:
                    if (apple.X - snake.Segments[0].X == 1 && snake.Segments[0].Y == apple.Y)
                        return true;
                    break;
                case Direction.Down:
                    if (apple.Y - snake.Segments[0].Y == 1 && snake.Segments[0].X == apple.X)
                        return true;
                    break;
                case Direction.Left:
                    if (snake.Segments[0].X - apple.X == 1 && snake.Segments[0].Y == apple.Y)
                        return true;
                    break;
            }
            return false;
        }
        public void InitializeFreeSpots()
        {
            freeSpots = null;
            freeSpots = new List<int>();
            for (int i = 0; i < Params.fields; ++i)
            {
                for (int j = 0; j < Params.fields; ++j)
                {
                    if (i == Params.fields / 2 && j == Params.fields / 2) continue;
                    freeSpots.Add(Helpers.PointToInt(new System.Windows.Point(i, j)));
                }
            }
            PlaceApple();
        }

        private int DrawNumber(int bound)
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            return rand.Next(bound);
        }

        private void GetBitmap()
        {
            WebClient client = new WebClient();
            string url = urls[DrawNumber(urls.Length)];
            Stream stream = client.OpenRead(url);
            bitmap = null;
            bitmap = new Bitmap(stream);
            stream = null;
            client = null;
        }

        public void PlaceApple()
        {
            Random rand = new Random(Guid.NewGuid().GetHashCode());
            int ind = rand.Next() % freeSpots.Count;

            apple = Helpers.IntToPoint(freeSpots[DrawNumber(freeSpots.Count)]);
            GetBitmap();
        }
    }
}
