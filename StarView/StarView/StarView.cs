using System;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using Akari.StarView.Annotations;
using CoreGraphics;
using Foundation;
using UIKit;

namespace Akari.StarView
{
    public class AkrStarView : UIView, INotifyPropertyChanged
    {
        private double _value;

        public double Value
        {
            get { return _value; }
            set
            {
                _value = value;
                OnPropertyChanged();
                SetNeedsDisplay();
            }
        }

        public bool CanEdited
        {
            get { return UserInteractionEnabled; }
            set { UserInteractionEnabled = value; }
        }

        public UIColor NormalColor
        {
            get { return _normalColor; }
            set
            {
                _normalColor = value;
                SetNeedsDisplay();
            }
        }

        public UIColor HighlightColor
        {
            get { return _highlightColor; }
            set
            {
                _highlightColor = value;
                SetNeedsDisplay();
            }
        }

        public UIColor _normalColor = UIColor.LightGray.ColorWithAlpha(.8f);
        public UIColor _highlightColor = UIColor.Orange;

        public readonly int StarConut = 5;
        private static UIImage _normalStarImage;
        private static UIImage _highlightStarImage;

        public AkrStarView(int starConut)
        {
            StarConut = starConut;
            BackgroundColor = UIColor.Clear;
        }

        public AkrStarView(CGRect frame, int starConut)
            : base(frame)
        {
            StarConut = starConut;
            BackgroundColor = UIColor.Clear;

        }

        public override void TouchesMoved(NSSet touches, UIEvent evt)
        {
            if (!CanEdited && touches.Count <= 0) return;
            var e = touches.First() as UITouch;
            var p = e.LocationInView(this);

            Value = Math.Max(0, Math.Min(1, p.X / Frame.Width));
        }

        public override void Draw(CGRect rect)
        {
            base.Draw(rect);

            _normalStarImage?.Dispose();
            _normalStarImage = null;
            _highlightStarImage?.Dispose();
            _highlightStarImage = null;

            var w = rect.Width / StarConut;

            _normalStarImage = CreateStarImage(w, NormalColor);
            _highlightStarImage = CreateStarImage(w, HighlightColor);

            _normalStarImage.Draw(CGPoint.Empty);
            _highlightStarImage.DrawAsPatternInRect(new CGRect(0, 0, w * StarConut * (Value > 1 ? 1 : Value), w));
        }

        private UIImage CreateStarImage(nfloat w, UIColor normalColor)
        {
            UIGraphics.BeginImageContext(new CGSize(Frame.Size.Width, w));

            for (int i = 0; i < StarConut; i++)
            {
                DrawStar(i * w, 0, w, 0, 1, normalColor);
            }

            var im = UIGraphics.GetImageFromCurrentImageContext();
            UIGraphics.EndImageContext();

            return im;
        }

        private void DrawStar(nfloat x, nfloat y, nfloat size, float start, float end, UIColor color)
        {
            var g = UIGraphics.GetCurrentContext();

            g.SetLineWidth(100);

            var xCenter = x + size / 2;
            var yCenter = y + size / 2;
            var r = size / 2;
            var flip = -1.0;

            g.SetFillColor(color.CGColor);
            g.SetStrokeColor(color.CGColor);

            var theta = 2 * Math.PI * (2.0 / 5);

            g.MoveTo(xCenter, (nfloat)(r * flip + yCenter));
            for (var i = 0; i < 5; i++)
            {
                var dx = r * Math.Sin(i * theta);
                var dy = r * Math.Cos(i * theta);
                g.AddLineToPoint((nfloat)dx + xCenter, (nfloat)(dy * flip + yCenter));
            }

            g.ClosePath();
            g.FillPath();

            g.ClearRect(new CGRect(0, 0, size * start, size));
            g.ClearRect(new CGRect(x + size * end, 0, size * (1 - end), size));

        }

        #region INotifyPropertyChanged

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion

    }
}
