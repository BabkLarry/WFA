using System.Windows.Forms;

namespace WFA
{
    partial class MyForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.size_left = new System.Windows.Forms.Panel();
            this.size_bottom = new System.Windows.Forms.Panel();
            this.size_bottom_right = new System.Windows.Forms.Panel();
            this.size_bottom_left = new System.Windows.Forms.Panel();
            this.size_right = new System.Windows.Forms.Panel();
            this.size_top = new System.Windows.Forms.Panel();
            this.size_top_right = new System.Windows.Forms.Panel();
            this.size_top_left = new System.Windows.Forms.Panel();
            this.menu = new System.Windows.Forms.Panel();
            this.Name_ = new System.Windows.Forms.Label();
            this.log = new System.Windows.Forms.PictureBox();
            this.home_big = new WFA.MyButton();
            this.home_close = new WFA.MyButton();
            this.home_small = new WFA.MyButton();
            this.size_bottom.SuspendLayout();
            this.size_top.SuspendLayout();
            this.menu.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.log)).BeginInit();
            this.SuspendLayout();
            // 
            // size_left
            // 
            this.size_left.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.size_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.size_left.Location = new System.Drawing.Point(0, 3);
            this.size_left.Name = "size_left";
            this.size_left.Size = new System.Drawing.Size(3, 432);
            this.size_left.TabIndex = 0;
            this.size_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_left_MouseDown);
            // 
            // size_bottom
            // 
            this.size_bottom.Controls.Add(this.size_bottom_right);
            this.size_bottom.Controls.Add(this.size_bottom_left);
            this.size_bottom.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.size_bottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.size_bottom.Location = new System.Drawing.Point(0, 435);
            this.size_bottom.Name = "size_bottom";
            this.size_bottom.Size = new System.Drawing.Size(720, 3);
            this.size_bottom.TabIndex = 2;
            this.size_bottom.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_bottom_MouseDown);
            // 
            // size_bottom_right
            // 
            this.size_bottom_right.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.size_bottom_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.size_bottom_right.Location = new System.Drawing.Point(717, 0);
            this.size_bottom_right.Name = "size_bottom_right";
            this.size_bottom_right.Size = new System.Drawing.Size(3, 3);
            this.size_bottom_right.TabIndex = 4;
            this.size_bottom_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_bottom_right_MouseDown);
            // 
            // size_bottom_left
            // 
            this.size_bottom_left.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.size_bottom_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.size_bottom_left.Location = new System.Drawing.Point(0, 0);
            this.size_bottom_left.Name = "size_bottom_left";
            this.size_bottom_left.Size = new System.Drawing.Size(3, 3);
            this.size_bottom_left.TabIndex = 1;
            this.size_bottom_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_bottom_left_MouseDown);
            // 
            // size_right
            // 
            this.size_right.Cursor = System.Windows.Forms.Cursors.SizeWE;
            this.size_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.size_right.Location = new System.Drawing.Point(717, 3);
            this.size_right.Name = "size_right";
            this.size_right.Size = new System.Drawing.Size(3, 432);
            this.size_right.TabIndex = 3;
            this.size_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_right_MouseDown);
            // 
            // size_top
            // 
            this.size_top.Controls.Add(this.size_top_right);
            this.size_top.Controls.Add(this.size_top_left);
            this.size_top.Cursor = System.Windows.Forms.Cursors.SizeNS;
            this.size_top.Dock = System.Windows.Forms.DockStyle.Top;
            this.size_top.Location = new System.Drawing.Point(0, 0);
            this.size_top.Name = "size_top";
            this.size_top.Size = new System.Drawing.Size(720, 3);
            this.size_top.TabIndex = 5;
            this.size_top.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_top_MouseDown);
            // 
            // size_top_right
            // 
            this.size_top_right.Cursor = System.Windows.Forms.Cursors.SizeNESW;
            this.size_top_right.Dock = System.Windows.Forms.DockStyle.Right;
            this.size_top_right.Location = new System.Drawing.Point(717, 0);
            this.size_top_right.Name = "size_top_right";
            this.size_top_right.Size = new System.Drawing.Size(3, 3);
            this.size_top_right.TabIndex = 4;
            this.size_top_right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_top_right_MouseDown);
            // 
            // size_top_left
            // 
            this.size_top_left.Cursor = System.Windows.Forms.Cursors.SizeNWSE;
            this.size_top_left.Dock = System.Windows.Forms.DockStyle.Left;
            this.size_top_left.Location = new System.Drawing.Point(0, 0);
            this.size_top_left.Name = "size_top_left";
            this.size_top_left.Size = new System.Drawing.Size(3, 3);
            this.size_top_left.TabIndex = 1;
            this.size_top_left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.size_top_left_MouseDown);
            // 
            // menu
            // 
            this.menu.BackColor = System.Drawing.SystemColors.Highlight;
            this.menu.Controls.Add(this.Name_);
            this.menu.Controls.Add(this.log);
            this.menu.Controls.Add(this.home_big);
            this.menu.Controls.Add(this.home_close);
            this.menu.Controls.Add(this.home_small);
            this.menu.Dock = System.Windows.Forms.DockStyle.Top;
            this.menu.Location = new System.Drawing.Point(3, 3);
            this.menu.Name = "menu";
            this.menu.Size = new System.Drawing.Size(714, 40);
            this.menu.TabIndex = 6;
            this.menu.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menu_MouseDown);
            this.menu.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menu_MouseMove);
            // 
            // Name_
            // 
            this.Name_.AutoSize = true;
            this.Name_.Font = new System.Drawing.Font("宋体", 18F);
            this.Name_.Location = new System.Drawing.Point(46, 7);
            this.Name_.Name = "Name_";
            this.Name_.Size = new System.Drawing.Size(82, 24);
            this.Name_.TabIndex = 125;
            this.Name_.Text = "MyForm";
            this.Name_.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menu_MouseDown);
            this.Name_.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menu_MouseMove);
            // 
            // log
            // 
            this.log.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.log.Location = new System.Drawing.Point(0, 0);
            this.log.Name = "log";
            this.log.Size = new System.Drawing.Size(40, 40);
            this.log.TabIndex = 124;
            this.log.TabStop = false;
            this.log.MouseDown += new System.Windows.Forms.MouseEventHandler(this.menu_MouseDown);
            this.log.MouseMove += new System.Windows.Forms.MouseEventHandler(this.menu_MouseMove);
            // 
            // home_big
            // 
            this.home_big.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.home_big.BackColor = System.Drawing.Color.Transparent;
            this.home_big.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.home_big.Location = new System.Drawing.Point(651, 7);
            this.home_big.Name = "home_big";
            this.home_big.Size = new System.Drawing.Size(25, 25);
            this.home_big.TabIndex = 123;
            this.home_big.UseVisualStyleBackColor = false;
            this.home_big.按钮形状 = WFA.MyButton.Shape.方形;
            this.home_big.Click += new System.EventHandler(this.home_big_Click);
            // 
            // home_close
            // 
            this.home_close.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.home_close.BackColor = System.Drawing.Color.Transparent;
            this.home_close.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.home_close.Location = new System.Drawing.Point(682, 7);
            this.home_close.Name = "home_close";
            this.home_close.Size = new System.Drawing.Size(25, 25);
            this.home_close.TabIndex = 121;
            this.home_close.UseVisualStyleBackColor = false;
            this.home_close.按钮形状 = WFA.MyButton.Shape.方形;
            this.home_close.Click += new System.EventHandler(this.home_close_Click);
            // 
            // home_small
            // 
            this.home_small.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.home_small.BackColor = System.Drawing.Color.Transparent;
            this.home_small.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.home_small.Location = new System.Drawing.Point(620, 7);
            this.home_small.Name = "home_small";
            this.home_small.Size = new System.Drawing.Size(25, 25);
            this.home_small.TabIndex = 122;
            this.home_small.UseVisualStyleBackColor = false;
            this.home_small.按钮形状 = WFA.MyButton.Shape.方形;
            this.home_small.Click += new System.EventHandler(this.home_small_Click);
            // 
            // MyForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(194)))), ((int)(((byte)(236)))));
            this.ClientSize = new System.Drawing.Size(720, 438);
            this.ControlBox = false;
            this.Controls.Add(this.menu);
            this.Controls.Add(this.size_right);
            this.Controls.Add(this.size_left);
            this.Controls.Add(this.size_bottom);
            this.Controls.Add(this.size_top);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(250, 150);
            this.Name = "MyForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MyForm";
            this.size_bottom.ResumeLayout(false);
            this.size_top.ResumeLayout(false);
            this.menu.ResumeLayout(false);
            this.menu.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.log)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel size_left;
        private System.Windows.Forms.Panel size_bottom;
        private System.Windows.Forms.Panel size_right;
        private System.Windows.Forms.Panel size_bottom_right;
        private System.Windows.Forms.Panel size_bottom_left;
        private System.Windows.Forms.Panel size_top;
        private System.Windows.Forms.Panel size_top_right;
        private System.Windows.Forms.Panel size_top_left;
        private System.Windows.Forms.Panel menu;
        private System.Windows.Forms.PictureBox log;
        private MyButton home_big;
        private MyButton home_close;
        private MyButton home_small;
        private System.Windows.Forms.Label Name_;
    }
}