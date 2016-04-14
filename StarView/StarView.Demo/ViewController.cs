using System;
using Akari.StarView;
using CoreGraphics;
using UIKit;

namespace StarView.Demo
{
    public partial class ViewController : UIViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private AkrStarView v;
        public override void ViewDidLoad()
        {
            base.ViewDidLoad();
            // Perform any additional setup after loading the view, typically from a nib.

            v = new AkrStarView(new CGRect(100, 100, 150, 100), 5);

            View.AddSubview(v);

            var btn = new UIButton(new CGRect(100, 200, 100, 44));
            btn.SetTitle("Click", UIControlState.Normal);
            btn.TouchUpInside += Btn_TouchUpInside;
            btn.SetTitleColor(UIColor.Red, UIControlState.Normal);
            View.AddSubview(btn);

            var btn2 = new UIButton(new CGRect(100, 250, 100, 44));
            btn2.SetTitle("Clean", UIControlState.Normal);
            btn2.TouchUpInside += Btn2_TouchUpInside; ;
            btn2.SetTitleColor(UIColor.Red, UIControlState.Normal);
            View.AddSubview(btn2);

            var lb = new UILabel(new CGRect(100,300,100,44)); 
            lb.TextColor = UIColor.Orange;
            View.AddSubview(lb);

            v.PropertyChanged += delegate
            {
                lb.Text = v.Value.ToString();
            };
        }

        private void Btn2_TouchUpInside(object sender, EventArgs e)
        {
            v.Value = 0;
        }

        private void Btn_TouchUpInside(object sender, EventArgs e)
        {
            v.Value += 0.05;
        }
    }
}