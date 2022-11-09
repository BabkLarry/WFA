using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WFA
{
    public partial class MyForm : Form
    {
        public float MyForm_X;
        public float MyForm_Y;
        public int number = 0;
        public MyForm()
        {
            InitializeComponent();

            //保存原始大小
            MyForm_X = this.Width;
            MyForm_Y = this.Height;
            setTag(this);

            //setControls(X,  Y, this);
            this.Resize += new System.EventHandler(this.Detail_Resize);

        }
        #region 窗体自适应大小代码
        private void Detail_Resize(object sender, EventArgs e)
        {
            setControls((float)this.Width / MyForm_X, (float)this.Height / MyForm_Y, this);
        }
        private void setTag(System.Windows.Forms.Control cons)
        {
            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls(float newx, float newy, System.Windows.Forms.Control cons)
        {
            if (number > 1)
            {
                return;
            }

            foreach (System.Windows.Forms.Control con in cons.Controls)
            {
                try
                {
                    string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//执行到此处报错，把Tag换成Name时候，不报错。但是执行到下一条语句编译无法通过
                    float a = Convert.ToSingle(mytag[0]) * newx;

  
                    if (con.Name.Equals("Name_"))
                    {
                        continue;
                    }
                    if (con.Name.Equals("log"))
                    {
                        continue;
                    }
                    if(con.Name.Equals("home_small")&& con.Name.Equals("home_big") && con.Name.Equals("home_close"))
                    {
                        continue;
                    }
                    con.Width = (int)a;
                    a = Convert.ToSingle(mytag[1]);
                    con.Height = (int)(a);
                    a = Convert.ToSingle(mytag[2]) * newx;
                    con.Left = (int)(a);
                    a = Convert.ToSingle(mytag[3]);
                    con.Top = (int)(a);
                    Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                    //改变字体大小
                    con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                    if (con.Controls.Count > 0)
                    {
                        setControls(newx, newy, con);
                    }
                }
                catch
                {

                }
            }

            this.number++;


        }
        #endregion

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("标题栏名字")]
        [Category("自定义")]
        public string MenuText
        {
            get { return Text; }
            set
            {
                Text = value;
                Name_.Text = value;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("标题栏名字颜色色")]
        [Category("自定义")]
        public Color MenuColor
        {
            get { return Name_.ForeColor; }
            set
            {
                Name_.ForeColor = value;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("标题栏背景色")]
        [Category("自定义")]
        public Color MenuBackColor
        {
            get { return menu.BackColor; }
            set
            {
                menu.BackColor = value;
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("标题栏是否显示log")]
        [Category("自定义")]
        public bool MenuLogVisible
        {
            get
            { return log.Visible; }
            set
            {
                log.Visible = value;
                if (value)
                {
                    Name_.Location = new Point(46, 9);
                }
                else
                {
                    Name_.Location = new Point(6, 9);
                }
            }
        }

        [Browsable(true)]
        [EditorBrowsable(EditorBrowsableState.Always)]
        [Description("标题栏log图")]
        [Category("自定义")]
        public Image MenuLog
        {
            get { return log.BackgroundImage; }
            set
            {
                log.BackgroundImage = value;
            }
        }

        #region 拖拽窗体代码
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);
        #region 拖动菜单栏
        private Point mPoint;
        private void menu_MouseDown(object sender, MouseEventArgs e)
        {
            mPoint = new Point(e.X, e.Y);
        }
        private void menu_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                this.Location = new Point(this.Location.X + e.X - mPoint.X, this.Location.Y + e.Y - mPoint.Y);
            }
        }
        #endregion
        #region 拖动更改窗体大小
        private void size_top_left_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61444, 0);
        }
        private void size_top_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61443, 0);
        }
        private void size_top_right_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61445, 0);
        }
        private void size_right_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61442, 0);
        }
        private void size_bottom_right_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61448, 0);
        }
        private void size_bottom_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61446, 0);
        }
        private void size_bottom_left_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61447, 0);
        }
        private void size_left_MouseDown(object sender, MouseEventArgs e)
        {
            ReleaseCapture();
            SendMessage(this.Handle, 274, 61441, 0);
        }
        #endregion
        #region 右上角按钮
        private void home_close_Click(object sender, EventArgs e)
        {
            Close();
        }
        private void home_big_Click(object sender, EventArgs e)
        {
            if (this.WindowState.Equals(FormWindowState.Maximized))
            {
                this.WindowState = FormWindowState.Normal;
            }
            else
            {
                this.WindowState = FormWindowState.Maximized;
            }
        }
        private void home_small_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }
        #endregion
        #endregion


    }
}
