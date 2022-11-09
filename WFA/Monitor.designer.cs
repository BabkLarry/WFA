namespace Weighing_Management
{
    partial class Monitor
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
            this.components = new System.ComponentModel.Container();
            this.webcam = new System.Windows.Forms.PictureBox();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.顶置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.刷新画面ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.关闭ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.webcam)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // webcam
            // 
            this.webcam.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.webcam.ContextMenuStrip = this.contextMenuStrip1;
            this.webcam.Dock = System.Windows.Forms.DockStyle.Fill;
            this.webcam.Location = new System.Drawing.Point(0, 0);
            this.webcam.Name = "webcam";
            this.webcam.Size = new System.Drawing.Size(300, 200);
            this.webcam.TabIndex = 1;
            this.webcam.TabStop = false;
            this.webcam.MouseDown += new System.Windows.Forms.MouseEventHandler(this.monitor_MouseDown);
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.顶置ToolStripMenuItem,
            this.刷新画面ToolStripMenuItem,
            this.toolStripSeparator1,
            this.关闭ToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(125, 76);
            // 
            // 顶置ToolStripMenuItem
            // 
            this.顶置ToolStripMenuItem.Name = "顶置ToolStripMenuItem";
            this.顶置ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.顶置ToolStripMenuItem.Text = "置顶";
            this.顶置ToolStripMenuItem.Click += new System.EventHandler(this.顶置ToolStripMenuItem_Click);
            // 
            // 刷新画面ToolStripMenuItem
            // 
            this.刷新画面ToolStripMenuItem.Name = "刷新画面ToolStripMenuItem";
            this.刷新画面ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.刷新画面ToolStripMenuItem.Text = "刷新画面";
            this.刷新画面ToolStripMenuItem.Click += new System.EventHandler(this.刷新画面ToolStripMenuItem_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(121, 6);
            // 
            // 关闭ToolStripMenuItem
            // 
            this.关闭ToolStripMenuItem.Name = "关闭ToolStripMenuItem";
            this.关闭ToolStripMenuItem.Size = new System.Drawing.Size(124, 22);
            this.关闭ToolStripMenuItem.Text = "关闭";
            this.关闭ToolStripMenuItem.Click += new System.EventHandler(this.关闭ToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.pictureBox1.Location = new System.Drawing.Point(280, 180);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(20, 20);
            this.pictureBox1.MinimumSize = new System.Drawing.Size(20, 20);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 20);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            // 
            // Monitor
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(300, 200);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.webcam);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "Monitor";
            this.ShowInTaskbar = false;
            this.Text = "monitor";
            this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.monitor_MouseDown);
            this.Resize += new System.EventHandler(this.monitor_Resize);
            ((System.ComponentModel.ISupportInitialize)(this.webcam)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox webcam;
        private System.Windows.Forms.ToolStripMenuItem 顶置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 关闭ToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        public System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.ToolStripMenuItem 刷新画面ToolStripMenuItem;
    }
}