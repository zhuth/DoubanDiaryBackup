namespace DoubanDiaryBackup
{
    partial class Form1
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
            this.toolStripContainer1 = new System.Windows.Forms.ToolStripContainer();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.web = new System.Windows.Forms.WebBrowser();
            this.notes = new System.Windows.Forms.WebBrowser();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.设置ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.开始抓取ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抓取评论ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抓取书籍列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抓取电影列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抓取音乐列表ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.tmrDownload = new System.Windows.Forms.Timer(this.components);
            this.重试ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.抓取广播ToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripContainer1.ContentPanel.SuspendLayout();
            this.toolStripContainer1.TopToolStripPanel.SuspendLayout();
            this.toolStripContainer1.SuspendLayout();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStripContainer1
            // 
            // 
            // toolStripContainer1.ContentPanel
            // 
            this.toolStripContainer1.ContentPanel.Controls.Add(this.splitContainer1);
            this.toolStripContainer1.ContentPanel.Size = new System.Drawing.Size(645, 405);
            this.toolStripContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.toolStripContainer1.Location = new System.Drawing.Point(0, 0);
            this.toolStripContainer1.Name = "toolStripContainer1";
            this.toolStripContainer1.Size = new System.Drawing.Size(645, 430);
            this.toolStripContainer1.TabIndex = 0;
            this.toolStripContainer1.Text = "toolStripContainer1";
            // 
            // toolStripContainer1.TopToolStripPanel
            // 
            this.toolStripContainer1.TopToolStripPanel.Controls.Add(this.menuStrip1);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 0);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.web);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.notes);
            this.splitContainer1.Size = new System.Drawing.Size(645, 405);
            this.splitContainer1.SplitterDistance = 413;
            this.splitContainer1.TabIndex = 0;
            // 
            // web
            // 
            this.web.Dock = System.Windows.Forms.DockStyle.Fill;
            this.web.Location = new System.Drawing.Point(0, 0);
            this.web.MinimumSize = new System.Drawing.Size(20, 20);
            this.web.Name = "web";
            this.web.ScriptErrorsSuppressed = true;
            this.web.Size = new System.Drawing.Size(413, 405);
            this.web.TabIndex = 1;
            this.web.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.web_DocumentCompleted);
            // 
            // notes
            // 
            this.notes.Dock = System.Windows.Forms.DockStyle.Fill;
            this.notes.Location = new System.Drawing.Point(0, 0);
            this.notes.MinimumSize = new System.Drawing.Size(20, 20);
            this.notes.Name = "notes";
            this.notes.ScriptErrorsSuppressed = true;
            this.notes.Size = new System.Drawing.Size(228, 405);
            this.notes.TabIndex = 1;
            this.notes.DocumentCompleted += new System.Windows.Forms.WebBrowserDocumentCompletedEventHandler(this.notes_DocumentCompleted);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Dock = System.Windows.Forms.DockStyle.None;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.设置ToolStripMenuItem,
            this.开始抓取ToolStripMenuItem,
            this.抓取评论ToolStripMenuItem,
            this.抓取广播ToolStripMenuItem,
            this.抓取书籍列表ToolStripMenuItem,
            this.抓取电影列表ToolStripMenuItem,
            this.抓取音乐列表ToolStripMenuItem,
            this.重试ToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(645, 25);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // 设置ToolStripMenuItem
            // 
            this.设置ToolStripMenuItem.Name = "设置ToolStripMenuItem";
            this.设置ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.设置ToolStripMenuItem.Text = "设置";
            this.设置ToolStripMenuItem.Click += new System.EventHandler(this.设置ToolStripMenuItem_Click);
            // 
            // 开始抓取ToolStripMenuItem
            // 
            this.开始抓取ToolStripMenuItem.Name = "开始抓取ToolStripMenuItem";
            this.开始抓取ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.开始抓取ToolStripMenuItem.Text = "抓取日记";
            this.开始抓取ToolStripMenuItem.Click += new System.EventHandler(this.开始抓取ToolStripMenuItem_Click);
            // 
            // 抓取评论ToolStripMenuItem
            // 
            this.抓取评论ToolStripMenuItem.Name = "抓取评论ToolStripMenuItem";
            this.抓取评论ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.抓取评论ToolStripMenuItem.Text = "抓取评论";
            this.抓取评论ToolStripMenuItem.Click += new System.EventHandler(this.抓取评论ToolStripMenuItem_Click);
            // 
            // 抓取书籍列表ToolStripMenuItem
            // 
            this.抓取书籍列表ToolStripMenuItem.Name = "抓取书籍列表ToolStripMenuItem";
            this.抓取书籍列表ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.抓取书籍列表ToolStripMenuItem.Text = "抓取书籍列表";
            this.抓取书籍列表ToolStripMenuItem.Click += new System.EventHandler(this.抓取书籍列表ToolStripMenuItem_Click);
            // 
            // 抓取电影列表ToolStripMenuItem
            // 
            this.抓取电影列表ToolStripMenuItem.Name = "抓取电影列表ToolStripMenuItem";
            this.抓取电影列表ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.抓取电影列表ToolStripMenuItem.Text = "抓取电影列表";
            this.抓取电影列表ToolStripMenuItem.Click += new System.EventHandler(this.抓取电影列表ToolStripMenuItem_Click);
            // 
            // 抓取音乐列表ToolStripMenuItem
            // 
            this.抓取音乐列表ToolStripMenuItem.Name = "抓取音乐列表ToolStripMenuItem";
            this.抓取音乐列表ToolStripMenuItem.Size = new System.Drawing.Size(92, 21);
            this.抓取音乐列表ToolStripMenuItem.Text = "抓取音乐列表";
            this.抓取音乐列表ToolStripMenuItem.Click += new System.EventHandler(this.抓取音乐列表ToolStripMenuItem_Click);
            // 
            // tmrDownload
            // 
            this.tmrDownload.Interval = 10;
            this.tmrDownload.Tick += new System.EventHandler(this.tmrDownload_Tick);
            // 
            // 重试ToolStripMenuItem
            // 
            this.重试ToolStripMenuItem.Name = "重试ToolStripMenuItem";
            this.重试ToolStripMenuItem.Size = new System.Drawing.Size(44, 21);
            this.重试ToolStripMenuItem.Text = "重试";
            this.重试ToolStripMenuItem.Click += new System.EventHandler(this.重试ToolStripMenuItem_Click);
            // 
            // 抓取广播ToolStripMenuItem
            // 
            this.抓取广播ToolStripMenuItem.Name = "抓取广播ToolStripMenuItem";
            this.抓取广播ToolStripMenuItem.Size = new System.Drawing.Size(68, 21);
            this.抓取广播ToolStripMenuItem.Text = "抓取广播";
            this.抓取广播ToolStripMenuItem.Click += new System.EventHandler(this.抓取广播ToolStripMenuItem_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(645, 430);
            this.Controls.Add(this.toolStripContainer1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "Form1";
            this.Text = "导出豆瓣内容";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.toolStripContainer1.ContentPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.ResumeLayout(false);
            this.toolStripContainer1.TopToolStripPanel.PerformLayout();
            this.toolStripContainer1.ResumeLayout(false);
            this.toolStripContainer1.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.ResumeLayout(false);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ToolStripContainer toolStripContainer1;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem 开始抓取ToolStripMenuItem;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.WebBrowser web;
        private System.Windows.Forms.WebBrowser notes;
        private System.Windows.Forms.Timer tmrDownload;
        private System.Windows.Forms.ToolStripMenuItem 设置ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抓取书籍列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抓取电影列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抓取音乐列表ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抓取评论ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 重试ToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem 抓取广播ToolStripMenuItem;

    }
}

