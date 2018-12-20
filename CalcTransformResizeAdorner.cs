using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace DragDropObjects
{
    class CalcTransformResizeAdorner : ICalcTransformResize
    {
        public double CalcNextLeftOffset(/*SimpleCircleAdorner _ovElement, UIElement _orElement, Canvas _canv, Point startPoint*/)
        {
            //var currentPosition = Mouse.GetPosition(_canv);
            //var currentPositionMovedOverlay = Mouse.GetPosition(_ovElement);

            //double clickDistToBorderOverlay = currentPositionMovedOverlay.X;
            //if (clickDistToBorderOverlay > _ovElement.ActualWidth)
            //{
            //    clickDistToBorderOverlay = _ovElement.ActualWidth;
            //}

            //if (clickDistToBorderOverlay < 0)
            //{
            //    clickDistToBorderOverlay = 0;
            //}

            //var position = Canvas.GetLeft(_orElement) + _ovElement.LeftOffset;
            //var rightDistance = Canvas.GetLeft(_orElement) + _ovElement.LeftOffset + _ovElement.ActualWidth;

            //var maxLeftOfset = Canvas.GetLeft(_orElement);


            //if (_ovElement.LeftOffset < 0)
            //{
            //    if (position <= 0 && (currentPosition.X - clickDistToBorderOverlay) <= 0)
            //    {
            //        return (0 - maxLeftOfset);
            //    }
            //}
            //else if (_ovElement.LeftOffset > 0)
            //{
            //    if ((rightDistance >= _canv.ActualWidth) && ((currentPosition.X + clickDistToBorderOverlay) >= _canv.ActualWidth))
            //    {
            //        return _canv.ActualWidth - maxLeftOfset - _ovElement.ActualWidth;
            //    }
            //}
            //else if (_ovElement.LeftOffset == 0)
            //{
            //    if ((Canvas.GetLeft(_orElement) <= 0) && (currentPosition.X <= 0))
            //    {
            //        return 0;
            //    }

            //    if (((Canvas.GetLeft(_orElement) + _ovElement.ActualWidth) >= _canv.ActualWidth) && (currentPosition.X >= _canv.ActualWidth))
            //    {
            //        return 0;
            //    }
            //}
            //else
            //{
            //    return currentPosition.X - startPoint.X;
            //}
            //return currentPosition.X - startPoint.X;
            return 0;
        }

        public double CalcNextTopOffset()
        {

            return 0;

        }
        public double Calculate()
        {
            return 0;
        }
    }
}