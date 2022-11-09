using Package.VZ;
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

namespace Weighing_Management
{
    public partial class Monitor : Form
    {
        #region 拖动无边框窗体
        [DllImport("user32.dll")]
        public static extern bool ReleaseCapture();//改变窗体大小
        [DllImport("user32.dll")]
        public static extern bool SendMessage(IntPtr hwnd, int wMsg, int wParam, int lParam);//发送windows消息
        #endregion
        #region 保存页面大小值
        public float X;
        public float Y;
        #endregion
        /// <summary>
        /// monitor(相机id,相机类型)
        /// 0臻识/1海康
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="cls"></param>
        public unsafe Monitor(VZ vz)
        {
            InitializeComponent();
            vz.Play(webcam.Handle);
            this.TopMost = true;
            this.FormBorderStyle = FormBorderStyle.None;
            #region 窗体自适应初始化
            //保存原始大小
            X = this.Width;
            Y = this.Height;
            //保存控件的原始大小数据到控件的tag
            setTag(this);
            #endregion
        }

        private void monitor_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 274, 61449, 0);
            }
        }

        private void 关闭ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void 顶置ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.TopMost)
            {
                this.TopMost = false;
            }
            else
            {
                this.TopMost = true;
            }
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ReleaseCapture();
                SendMessage(this.Handle, 0x0112, 0xF008, 0);
            }
        }

        private void monitor_Resize(object sender, EventArgs e)
        {
            float newx = (this.Width) / X;
            float newy = this.Height / Y;
            setControls(newx, newy, this);
            pictureBox1.Location = new Point(this.Width - 20, this.Height - 20);
        }
        private void setTag(Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                con.Tag = con.Width + ":" + con.Height + ":" + con.Left + ":" + con.Top + ":" + con.Font.Size;
                if (con.Controls.Count > 0)
                    setTag(con);
            }
        }
        private void setControls(float newx, float newy, Control cons)
        {
            foreach (Control con in cons.Controls)
            {
                string[] mytag = con.Tag.ToString().Split(new char[] { ':' });//执行到此处报错，把Tag换成Name时候，不报错。但是执行到下一条语句编译无法通过
                float a = Convert.ToSingle(mytag[0]) * newx;
                con.Width = (int)a;
                a = Convert.ToSingle(mytag[1]) * newy;
                con.Height = (int)(a);
                a = Convert.ToSingle(mytag[2]) * newx;
                con.Left = (int)(a);
                a = Convert.ToSingle(mytag[3]) * newy;
                con.Top = (int)(a);
                Single currentSize = Convert.ToSingle(mytag[4]) * newy;
                //改变字体大小
                con.Font = new Font(con.Font.Name, currentSize, con.Font.Style, con.Font.Unit);
                if (con.Controls.Count > 0)
                {
                    setControls(newx, newy, con);
                }
            }
        }

        private void 刷新画面ToolStripMenuItem_Click(object sender, EventArgs e)
        {
        }
    }
}
