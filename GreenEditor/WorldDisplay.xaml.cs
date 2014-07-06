using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace GreenEditor
{
    /// <summary>
    /// Interaction logic for WorldDisplay.xaml
    /// </summary>
    public partial class WorldDisplay : Canvas, INotifyPropertyChanged
    {
        double scale;
        public double Scale
        {
            get { return scale; }
            set { scale = value; OnPropertyChanged("Scale"); OnPropertyChanged("PenSize"); }
        }

        double translateX;
        public double TranslateX
        {
            get { return translateX; }
            set { translateX = value; OnPropertyChanged("TranslateX"); }
        }

        double translateY;
        public double TranslateY
        {
            get { return translateY; }
            set { translateY = value; OnPropertyChanged("TranslateY"); }
        }

        public double GridSize { get; set; }
        public bool IsBusy { get; private set; }
        public List<WorldObject> Objects { get; private set; }
        public Point WorldCenter { get; set; }
        Path CurrentShape;

        public WorldDisplay()
        {
            this.GridSize = 10;
            this.Scale = 15;
            this.Objects = new List<WorldObject>();

            this.CurrentShape = new Path();
            this.Children.Add(CurrentShape);
            CurrentShape.Fill = Brushes.Gray.WithAlphaSetTo(128);
            CurrentShape.Stroke = Brushes.DarkGray;
            CurrentShape.DataContext = this;
            CurrentShape.SetBinding(Path.StrokeThicknessProperty, new Binding("PenSize"));
            Canvas.SetZIndex(CurrentShape, 99);

            InitializeComponent();

            this.RenderTransform = new ScaleTransform(Scale, Scale);
        }

        protected override void OnRender(DrawingContext dc)
        {
            dc.DrawRectangle(Brushes.Snow, null, new Rect(0, 0, this.ActualWidth, this.ActualHeight));

            Pen pen = new Pen(Brushes.Black, 1/Scale);
            for (double x = 0; x < this.ActualWidth - translateX; x += GridSize)
            {
                dc.DrawLine(pen, new Point(x, 0), new Point(x, ActualHeight));
            }

            for (double y = 0; y < this.ActualHeight- translateY; y += GridSize)
            {
                dc.DrawLine(pen, new Point(0, y), new Point(ActualWidth, y));
            }

            base.OnRender(dc);
        }

        protected override void OnMouseWheel(MouseWheelEventArgs e)
        {
            const double Factor = 1.02;

            if (e.Delta > 0)
            { 
                Scale *= Factor;
            }
            else
            { Scale /= Factor; }

            var transform = new TransformGroup();
            transform.Children.Add(new ScaleTransform(Scale, Scale));
            this.RenderTransform = transform;

            this.InvalidateVisual();
        }

        #region UI selects (__very__ bad code)
        public void SelectRectangle(Action<Rect> callback)
        {
            if (IsBusy) { throw new System.InvalidOperationException(); }
            IsBusy = true;
            bool startSelected = false;
            Point firstPoint = new Point(0, 0);

            MouseButtonEventHandler downHandler = null;
            downHandler = (sender, e) =>
            {
                startSelected = true;

                firstPoint = e.GetPosition(this);
                Rect r = new Rect(firstPoint, firstPoint);
                CurrentShape.Data = new RectangleGeometry(r);
                Mouse.RemoveMouseDownHandler(this, downHandler);
            };
            Mouse.AddMouseDownHandler(this, downHandler);

            MouseEventHandler moveHandler = null;
            moveHandler = (sender, e) =>
            {
                if (!startSelected) { return; }
                Rect r = new Rect(firstPoint, e.GetPosition(this));
                CurrentShape.Data = new RectangleGeometry(r);
                this.InvalidateVisual();
            };
            Mouse.AddMouseMoveHandler(this, moveHandler);

            MouseButtonEventHandler upHandler = null;
            upHandler = (sender, e) =>
            {
                if (!startSelected) { return; }
                this.IsBusy = false;
                Mouse.RemoveMouseMoveHandler(this, moveHandler);
                Mouse.RemoveMouseUpHandler(this, upHandler);
                CurrentShape.Data = null;
                this.InvalidateVisual();
                callback(new Rect(firstPoint, e.GetPosition(this)));
            };
            Mouse.AddMouseUpHandler(this, upHandler);
        }

        public void SelectPoint(Action<Point> callback)
        {
            if (IsBusy) { throw new System.InvalidOperationException(); }
            IsBusy = true;

            MouseButtonEventHandler handler = null;
            handler = (sender, e) =>
            {
                IsBusy = false;
                Mouse.RemoveMouseDownHandler(this, handler);
                this.InvalidateVisual();
                callback(e.GetPosition(this));
            };

            Mouse.AddMouseDownHandler(this, handler);
        }

        public void SelectPolygon(Action<List<Point>> callback)
        {
            if (IsBusy) { throw new System.InvalidOperationException(); }
            IsBusy = true;

            List<Point> points = new List<Point>();
            MouseButtonEventHandler handler = null;
            handler = (sender, e) =>
            {
                if (e.ChangedButton == MouseButton.Left)
                {
                    points.Add(e.GetPosition(this));
                    CurrentShape.Data = CreatePolygon(points);
                    this.InvalidateVisual();
                }
                else if (e.ChangedButton == MouseButton.Right)
                {
                    this.IsBusy = false;
                    Mouse.RemoveMouseDownHandler(this, handler);
                    CurrentShape.Data = null;
                    this.InvalidateVisual();
                    points = points.Select((p) => p).ToList();
                    callback(points);
                }
            };
            Mouse.AddMouseDownHandler(this, handler);
        }
        #endregion

        public void AddObject(WorldObject obj)
        {
            this.Objects.Add(obj);
            var path = new Path
            {
                Data = obj.Shape,
                Fill = obj.DisplayBrush,
                Stroke = Brushes.Black,
            };
            path.DataContext = this;
            path.SetBinding(Path.StrokeThicknessProperty, new Binding("PenSize"));
            this.Children.Add(path);
        }

        public static Geometry CreatePolygon(List<Point> points)
        {
            StreamGeometry streamGeometry = new StreamGeometry();
            using (StreamGeometryContext geometryContext = streamGeometry.Open())
            {
                geometryContext.BeginFigure(points[0], true, true);
                PointCollection pointColl = new PointCollection(points);
                geometryContext.PolyLineTo(pointColl, true, true);
            }

            return streamGeometry;
        }

        public double PenSize
        { get { return 4 / Scale; } }

        /*void SetUpMouse()
        {
            bool dragging = false;
            Point lastPoint = default(Point);
            Mouse.AddPreviewMouseDownHandler(this, (sender, e) =>
            {
                if (IsBusy) { return; }
                dragging = true;
                lastPoint = e.GetPosition(this);
            });

            Mouse.AddPreviewMouseUpHandler(this, (sender, e) =>
            {
                dragging = false;
            });

            Mouse.AddPreviewMouseMoveHandler(this, (sender, e) =>
            {
                if (!dragging) { return; }
                var currPoint = e.GetPosition(this);
                TranslateX += currPoint.X - lastPoint.X;
                TranslateY += currPoint.Y - lastPoint.Y;
                lastPoint = currPoint;
            });
        }*/

        void OnPropertyChanged(string prop)
        {
            if (PropertyChanged != null)
            { PropertyChanged(this, new PropertyChangedEventArgs(prop)); }
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
