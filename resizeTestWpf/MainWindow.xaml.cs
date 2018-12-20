using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

namespace resizeTestWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private bool _isDown;
        private bool _isDragging;

        private UIElement _originalElement;
        private double _originalLeft;
        private double _originalTop;
        private SimpleCircleAdorner _overlayElement;
        private ResizeAdorner _resizeAdorner;
        private Point _startPoint;

        public void OnPageLoad(object sender, RoutedEventArgs e)
        {
           

          _myCanvas.Width = 350;
            _myCanvas.Height = 350;

            _myCanvas.Background = new SolidColorBrush(Colors.PaleVioletRed);

            var rect1 = new Rectangle();
            rect1.Height = rect1.Width = 32;
            rect1.Fill = Brushes.Blue;

            Canvas.SetTop(rect1, 8);
            Canvas.SetLeft(rect1, 8);





            _myCanvas.Children.Add(rect1);


            _myCanvas.PreviewMouseLeftButtonDown += MyCanvas_PreviewMouseLeftButtonDown;
            _myCanvas.PreviewMouseMove += MyCanvas_PreviewMouseMove;
            _myCanvas.PreviewMouseLeftButtonUp += MyCanvas_PreviewMouseLeftButtonUp;
            PreviewKeyDown += window1_PreviewKeyDown;

            _resizeAdorner = new ResizeAdorner(btn1);
            Console.WriteLine($" adornder {_resizeAdorner.ToString()}  a");
            var layer = AdornerLayer.GetAdornerLayer(btn1);
            layer.Add(_resizeAdorner);





        }

        private void window1_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Escape && _isDragging)
            {
                DragFinished(true);
            }
        }

        private void MyCanvas_PreviewMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (_isDown)
            {
                DragFinished(false);
                e.Handled = true;
            }
        }

        private void DragFinished(bool cancelled)
        {
            Mouse.Capture(null);
            if (_isDragging)
            {
                AdornerLayer.GetAdornerLayer(_overlayElement.AdornedElement).Remove(_overlayElement);

                if (cancelled == false)
                {
                    Canvas.SetTop(_originalElement, _originalTop + _overlayElement.TopOffset);
                    Canvas.SetLeft(_originalElement, _originalLeft + _overlayElement.LeftOffset);
                }
                _overlayElement = null;
            }
            _isDragging = false;
            _isDown = false;
        }

        private void MyCanvas_PreviewMouseMove(object sender, MouseEventArgs e)
        {
            if (_isDown)
            {
                if ((_isDragging == false) &&
                    ((Math.Abs(e.GetPosition(_myCanvas).X - _startPoint.X) >
                      SystemParameters.MinimumHorizontalDragDistance) ||
                     (Math.Abs(e.GetPosition(_myCanvas).Y - _startPoint.Y) >
                      SystemParameters.MinimumVerticalDragDistance)))
                {
                    DragStarted();
                }
                if (_isDragging)
                {
                    DragMoved();
                }
            }
        }

        private void DragStarted()
        {
            _isDragging = true;
            _originalLeft = Canvas.GetLeft(_originalElement);
            _originalTop = Canvas.GetTop(_originalElement);

            _overlayElement = new SimpleCircleAdorner(_originalElement);
            var layer = AdornerLayer.GetAdornerLayer(_originalElement);
            layer.Add(_overlayElement);

        }


        private double getNextLeftOffset(SimpleCircleAdorner _ovElement, UIElement _orElement, Canvas _canv, Point startPoint)
        {
            var currentPosition = Mouse.GetPosition(_canv);
            var currentPositionMovedOverlay = Mouse.GetPosition(_ovElement);

            double clickDistToBorderOverlay = currentPositionMovedOverlay.X;
            if (clickDistToBorderOverlay > _ovElement.ActualWidth)
            {
                clickDistToBorderOverlay = _ovElement.ActualWidth;
            }

            if (clickDistToBorderOverlay < 0)
            {
                clickDistToBorderOverlay = 0;
            }

            var position = Canvas.GetLeft(_orElement) + _ovElement.LeftOffset;
            var rightDistance = Canvas.GetLeft(_orElement) + _ovElement.LeftOffset + _ovElement.ActualWidth;

            var maxLeftOfset = Canvas.GetLeft(_orElement);


            if (_ovElement.LeftOffset < 0)
            {
                if (position <= 0 && (currentPosition.X - clickDistToBorderOverlay) <= 0)
                {
                    return (0 - maxLeftOfset);
                }
            }
            else if (_ovElement.LeftOffset > 0)
            {
                if ((rightDistance >= _canv.ActualWidth) && ((currentPosition.X + clickDistToBorderOverlay) >= _canv.ActualWidth))
                {
                    return _canv.ActualWidth - maxLeftOfset - _ovElement.ActualWidth;
                }
            }
            else if (_ovElement.LeftOffset == 0)
            {
                if ((Canvas.GetLeft(_orElement) <= 0) && (currentPosition.X <= 0))
                {
                    return 0;
                }

                if (((Canvas.GetLeft(_orElement) + _ovElement.ActualWidth) >= _canv.ActualWidth) && (currentPosition.X >= _canv.ActualWidth))
                {
                    return 0;
                }
            }
            else
            {
                return currentPosition.X - startPoint.X;
            }
            return currentPosition.X - startPoint.X;
        }



        private double getNextTopOffset(SimpleCircleAdorner _ovElement, UIElement _orElement, Canvas _canv, Point startPoint)
        {
            var currentPosition = Mouse.GetPosition(_canv);
            var currentPositionMovedOverlay = Mouse.GetPosition(_ovElement);

            double clickDistToBorderOverlay = currentPositionMovedOverlay.Y;
            if (clickDistToBorderOverlay > _ovElement.ActualHeight)
            {
                clickDistToBorderOverlay = _ovElement.ActualHeight;
            }

            if (clickDistToBorderOverlay < 0)
            {
                clickDistToBorderOverlay = 0;
            }

            var position = Canvas.GetTop(_orElement) + _ovElement.TopOffset;
            var rightDistance = Canvas.GetTop(_orElement) + _ovElement.TopOffset + _ovElement.ActualHeight;

            var maxLeftOfset = Canvas.GetTop(_orElement);

            Console.WriteLine($"positionover {position} acthight {_canv.ActualHeight}");
            if (_ovElement.TopOffset < 0)
            {

                if (position <= 0 && (currentPosition.Y - clickDistToBorderOverlay) <= 0)
                {
                    Console.WriteLine($"1");
                    return (0 - maxLeftOfset);
                }
                Console.WriteLine($"0");
            }
            else if (_ovElement.TopOffset > 0)
            {
                if ((rightDistance >= _canv.ActualHeight) && ((currentPosition.Y + clickDistToBorderOverlay) >= _canv.ActualWidth))
                {
                    Console.WriteLine($"3");
                    return _canv.ActualHeight - maxLeftOfset - _ovElement.ActualHeight;
                }
                Console.WriteLine($"2");
            }
            else if (_ovElement.TopOffset == 0)
            {
                if ((Canvas.GetTop(_orElement) <= 0) && (currentPosition.Y <= 0))
                {
                    Console.WriteLine($"5");
                    return 0;
                }

                if (((Canvas.GetTop(_orElement) + _ovElement.ActualHeight) >= _canv.ActualHeight) && (currentPosition.Y >= _canv.ActualHeight))
                {
                    Console.WriteLine($"6");
                    return 0;
                }
                Console.WriteLine($"4");
            }
            else
            {
                Console.WriteLine($"7");
                return currentPosition.Y - startPoint.Y;
            }
            return currentPosition.Y - startPoint.Y;
        }

        private void DragMoved()
        {
            _overlayElement.LeftOffset = getNextLeftOffset(_overlayElement, _originalElement, _myCanvas, _startPoint);

            _overlayElement.TopOffset = getNextTopOffset(_overlayElement, _originalElement, _myCanvas, _startPoint);

        }
        private void MyCanvas_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.Source == _myCanvas)
            {
            }
            else
            {

                _isDown = true;
                _startPoint = e.GetPosition(_myCanvas);
                _originalElement = e.Source as UIElement;
                _myCanvas.CaptureMouse();

                e.Handled = true;
            }
        }
    }
}
