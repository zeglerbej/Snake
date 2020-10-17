using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WpfApp1.Model
{
    public enum Direction
    {
        Up,
        Right,
        Down,
        Left
    } 
    public class Snake : NotifiableObject
    {
        private List<Point> _segments;

        public List<Point> Segments
        {
            get
            {
                return _segments;
            }
            set
            {
                _segments = value;
                OnPropertyChanged("Segments");
            }
        }

        private Direction _direction;

        public Direction Direction
        {
            get
            {
                return _direction;
            }
            set
            {
                _direction = value;
                OnPropertyChanged("Direction");
            }
        }

        public Snake(int size)
        {
            Direction = Direction.Up;
            Segments = new List<Point>();
            Segments.Add(new Point(size/2, size/2));
        }

        public void Move(bool eat)
        {
            switch (_direction)
            {
                case Direction.Up:
                    ChangeCoords(0, -1, eat);
                    break;
                case Direction.Right:
                    ChangeCoords(1, 0, eat);
                    break;
                case Direction.Down:
                    ChangeCoords(0, 1, eat);
                    break;
                case Direction.Left:
                    ChangeCoords(-1, 0, eat);
                    break;
            }
        }
        
        private void ChangeCoords(int hor, int ver, bool eat)
        {
            if (Segments.Count == 0) return;
            Point last = new Point(Segments[Segments.Count - 1].X, Segments[Segments.Count - 1].Y);
            for (int i = Segments.Count - 1; i >= 1; --i)
            {
                Segments[i] = Segments[i - 1];
            }
            Segments[0] = new Point(Segments[0].X + hor, Segments[0].Y + ver);
            if(eat == true)
            {
                Segments.Add(last);
            }
        }

        public void ChangeDirections(Direction newDir)
        {
            switch (Direction)
            {
                case Direction.Up:
                    if (newDir == Direction.Down) return;
                    break;
                case Direction.Right:
                    if (newDir == Direction.Left) return;
                    break;
                case Direction.Down:
                    if (newDir == Direction.Up) return;
                    break;
                case Direction.Left:
                    if (newDir == Direction.Right) return;
                    break;
            }
            Direction = newDir;
        }

    }
}
