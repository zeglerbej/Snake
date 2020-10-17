using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using WpfApp1.Model;
using WpfApp1.ViewModel;

namespace WpfApp1
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DispatcherTimer gameTimer;
        private SnakeViewModel _svm;

        private Rectangle background;
       
        public MainWindow()
        {
            //Initialize
            InitializeComponent();
            background = new Rectangle();
            _svm = new SnakeViewModel();

            KeyDown += OnKeyDown;
            Field.Focus();

            gameTimer = new DispatcherTimer();
            DataContext = _svm;
            gameTimer.Tick += GameLogic;
            gameTimer.Interval = TimeSpan.FromMilliseconds(Params.frameDuration);

            DrawBackground();
            Field.Children.Add(background);
        }

        private void OnStart(object sender, EventArgs e)
        {
            gameTimer.Start();
        }

        private void InitializeGame()
        {
            gameTimer.Stop();
            _svm.InitializeGame();
        }
        private void OnEnd(object sender, EventArgs e)
        {
            InitializeGame();
            _svm.SetLength();
        }
                
        private void OnKeyDown(object sender, KeyEventArgs e)
        {
            if (_svm.directionQueue.Count > 1) return;
            switch (e.Key)
            {
                case Key.Up:
                    _svm.directionQueue.Enqueue(Direction.Up);
                    break;
                case Key.Right:
                    _svm.directionQueue.Enqueue(Direction.Right);
                    break;
                case Key.Down:
                    _svm.directionQueue.Enqueue(Direction.Down);
                    break;
                case Key.Left:
                    _svm.directionQueue.Enqueue(Direction.Left);
                    break;
            }
        }

        private void GameLogic(object sender, EventArgs e) 
        {
            //apples
            Field.Children.Clear();
            Field.Children.Add(background);
            if(_svm.directionQueue.Count > 0) _svm.snake.ChangeDirections(_svm.directionQueue.Dequeue());
            bool eaten = _svm.CheckEatingApple();
            if (!eaten)
            {
                _svm.freeSpots.Add(Helpers.PointToInt(_svm.snake.Segments[_svm.snake.Segments.Count - 1]));
            }
            _svm.snake.Move(eaten);
            _svm.freeSpots.Remove(Helpers.PointToInt(_svm.snake.Segments[0]));
            if (eaten)
            {
                _svm.PlaceApple();
                _svm.SetLength();
                DrawBackground();
            }

            //check end
            if (_svm.CheckEnd())
            {
                InitializeGame();
                _svm.SetLength();
            }

            //render
            RenderSnake();

        }

        private void DrawBackground()
        {
            ImageBrush img = new ImageBrush();
            BitmapImage bi = Helpers.BitmapToBitmapImage(_svm.bitmap);

            img.ImageSource = bi;
            background = null;
            background = new Rectangle();
            background.Width = Field.Width;
            background.Height = Field.Height;
            background.Fill = img;
        }

        private void RenderPoint(Point p, Color color)
        {
            Rectangle rect;
            rect = new Rectangle();
            rect.Width = Field.Width / Params.fields;
            rect.Height = Field.Height / Params.fields;
            rect.Stroke = new SolidColorBrush(color);
            rect.Fill = new SolidColorBrush(color);
            Canvas.SetLeft(rect, Field.Width * p.X / Params.fields);
            Canvas.SetTop(rect, Field.Height * p.Y / Params.fields);
            rect.StrokeThickness = 2;
            Field.Children.Add(rect);
            rect = null;
        }

        private void RenderSnake()
        {
            RenderPoint(_svm.apple, Colors.Red);

            foreach (Point s in _svm.snake.Segments)
            {
                RenderPoint(s, Colors.White);
            }           
        }
    }
}
