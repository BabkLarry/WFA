using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA
{
    public class MyButton : Button
    {

        /// <summary>
        /// 形状
        /// </summary>
        public enum Shape
        {
            /// <summary>
            /// 方形
            /// </summary>
            方形 = 0,
            /// <summary>
            /// 圆形
            /// </summary>
            圆形 = 1
        }
        private Shape shape;
        [Description("按钮形状"), Category("外观")]
        public Shape 按钮形状
        {
            get { return shape; }
            set
            {
                shape = value;
                //重新绘制控件
                Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
                var path = GDI(rect);
                this.Region = new Region(path);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            base.OnPaintBackground(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBilinear;
            Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
            var path = GDI(rect);
            this.Region = new Region(path);
            //文字样式
            SolidBrush b = new SolidBrush(this.BackColor);
            e.Graphics.FillPath(b, path);
            Brush brush = new SolidBrush(this.ForeColor);
            StringFormat gs = new StringFormat();
            switch (TextAlign)
            {
                case ContentAlignment.TopLeft:
                    gs.Alignment = StringAlignment.Near;
                    gs.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    gs.Alignment = StringAlignment.Center;
                    gs.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    gs.Alignment = StringAlignment.Far;
                    gs.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleLeft:
                    gs.Alignment = StringAlignment.Near;
                    gs.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleCenter:
                    gs.Alignment = StringAlignment.Center;
                    gs.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    gs.Alignment = StringAlignment.Far;
                    gs.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomLeft:
                    gs.Alignment = StringAlignment.Near;
                    gs.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomCenter:
                    gs.Alignment = StringAlignment.Center;
                    gs.LineAlignment = StringAlignment.Far;
                    break;
                case ContentAlignment.BottomRight:
                    gs.Alignment = StringAlignment.Far;
                    gs.LineAlignment = StringAlignment.Far;
                    break;
            }
            e.Graphics.DrawString(this.Text, this.Font, brush, rect, gs);

        }
        /// <summary>
        /// 绘制控件形状
        /// </summary>
        /// <param name="rect"></param>
        /// <param name="radius"></param>
        /// <returns></returns>
        private GraphicsPath GDI(Rectangle rect)
        {
            Rectangle arcRect = new Rectangle(rect.Location, new Size(Width, Height));
            GraphicsPath path = new GraphicsPath();
            switch (shape)
            {
                case Shape.方形:
                    path.AddRectangle(new RectangleF() { X = 0, Y = 0, Width = Width, Height = Height });
                    path.CloseAllFigures();
                    break;
                case Shape.圆形:
                    path.AddEllipse(arcRect);
                    path.CloseAllFigures();
                    break;
            }
            return path;
        }
    }
}
