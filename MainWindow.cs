// // Copyright (c) Microsoft. All rights reserved.
// // Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;

namespace DragDropObjects
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
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


        //this function is hard to uderstand because it take too many conditions to verify, the current position of the adorner, the current position of the mouse
        //to prevent from control get out of the bounds of the container, don't think that are a simple way to solve this problem
        private double getNextOffset( double startPosition, double OffsetRelativeToStartPos, double inner_size, double outtersize, double mousePosrelativeToInner, double MousePosRelativeToOutter, double OriginalDistanceToOutterCorner)
        {
            // that code bellow prevent from getting negative distance (when the mouse is out of the bounds) or far from the width/height (positive)
            if (mousePosrelativeToInner > inner_size)
                mousePosrelativeToInner = inner_size;
            else if (mousePosrelativeToInner < 0)
                mousePosrelativeToInner = 0;
            //
            var position = OriginalDistanceToOutterCorner + OffsetRelativeToStartPos;
            var rightDistance = OriginalDistanceToOutterCorner + OffsetRelativeToStartPos + inner_size;
            var maxLeftOfset = OriginalDistanceToOutterCorner;

            // caso esteja a esquerda/topo <0, a direita, ou na po
            if (OffsetRelativeToStartPos < 0){  // se estiver a esquerda/abaixo a da posicao original
                // se o objeto estiver dentro do outter, pelo canto esquerdo/topo, e a posicao arrastada do mouse estiver a direita/abaixo do canto esquerdo/topo  do outter
                if (position <= 0 && (MousePosRelativeToOutter - mousePosrelativeToInner) <= 0){ // se a posicao atual{
                    return (0 - maxLeftOfset);
                }
            }
            else if (OffsetRelativeToStartPos > 0){ //se estiver a direita/acima da posicao original
                // se o objeto estiver dentro do outter, pelo canto direito/top, e a posicao arrastada do mouse estiver a esquerd/acimaa do canto direito/topo do outter
                if ((rightDistance >= outtersize) && ((MousePosRelativeToOutter + mousePosrelativeToInner) >= outtersize)){
                        return outtersize - maxLeftOfset - inner_size;
                    }      
            }
            else if (OffsetRelativeToStartPos == 0)   {  //se estiver exatamente na posicao inicial
                // se o mouse estiver fora da area do outter retorna 0
                if ( (MousePosRelativeToOutter <= 0) || (MousePosRelativeToOutter >= outtersize)){
                    return 0;
                }
            }

            return MousePosRelativeToOutter - startPosition;
        }

        
        private void DragMoved()
        {
            _overlayElement.LeftOffset = getNextOffset( _startPoint.X, _overlayElement.LeftOffset, _overlayElement.ActualWidth, _myCanvas.ActualWidth, Mouse.GetPosition(_overlayElement).X, Mouse.GetPosition(_myCanvas).X, Canvas.GetLeft(_originalElement));
            _overlayElement.TopOffset = getNextOffset(_startPoint.Y, _overlayElement.TopOffset, _overlayElement.ActualHeight, _myCanvas.ActualHeight, Mouse.GetPosition(_overlayElement).Y, Mouse.GetPosition(_myCanvas).Y, Canvas.GetTop(_originalElement));

            //_overlayElement.TopOffset = getNextTopOffset(_overlayElement, _originalElement, _myCanvas, _startPoint);

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

    // Adorners must subclass the abstract base class Adorner.
}