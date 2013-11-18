using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace DoubanDiaryBackup
{
    public partial class Form1 : Form
    {
        int step = -1, start = 0;
        string peopleId = "";
        Queue<string> urls = new Queue<string>();
        Dictionary<string, bool> downloaded = new Dictionary<string, bool>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            web.Navigate("http://www.douban.com/");
        }

        private void 开始抓取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            web.Navigate("http://www.douban.com/mine/notes");
            step = 0;
        }

        private void error(string message)
        {
            MessageBox.Show(message);
            step = -1; peopleId = "";
            web.Navigate("http://www.douban.com/");
        }

        private void web_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
        }

        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            switch (step)
            {
                case 0: // 登录
                    for (int i = 0; i < e.Url.Segments.Length; ++i)
                        if (e.Url.Segments[i].ToLower() == "people/" && i < e.Url.Segments.Length - 1)
                        {
                            peopleId = e.Url.Segments[i + 1]; peopleId = peopleId.Substring(0, peopleId.Length - 1);
                        }
                    if (peopleId == "")
                    {
                        error("导航失败，请登录后重试。");
                    }
                    else
                    {
                        this.Text = "已登录 - " + peopleId;
                        step = 1; start = 0;
                        urls.Clear();
                        web.Navigate(string.Format("http://www.douban.com/people/{0}/notes?start={1}", peopleId, start));
                        return;
                    }
                    break;

                case 1: // 抓取
                    int c = 0;
                    foreach (HtmlElement link in web.Document.Links)
                    {
                        string ls = link.GetAttribute("href");
                        if (ls.ToLower().StartsWith("http://www.douban.com/note/") && ls.EndsWith("/"))
                        {
                            ++c;
                            if (!downloaded.ContainsKey(ls))
                            {
                                urls.Enqueue(ls); downloaded.Add(ls, true);
                            }
                        }
                    }
                    if (c > 0)
                        tmrDownload.Enabled = true;
                    else
                    {
                        error("下载队列完毕！");
                    }
                    break;
            }
        }

        private void notes_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            if (!e.Url.AbsolutePath.Contains("note/")) return;
            string id = e.Url.Segments[e.Url.Segments.Length - 1];
            if (id.EndsWith("/")) id = id.Substring(0, id.Length - 1);

            if (!Directory.Exists(peopleId)) Directory.CreateDirectory(peopleId);
            foreach (HtmlElement div in notes.Document.GetElementsByTagName("div"))
            {
                if (div.GetAttribute("className") == "article")
                {
                    File.WriteAllText(peopleId + "\\" + id + ".html", div.InnerHtml);
                    break;
                }
            }
            tmrDownload.Enabled = true;
        }

        private void tmrDownload_Tick(object sender, EventArgs e)
        {
            if (urls.Count > 0)
            {
                string s = urls.Dequeue();
                notes.Navigate(s);
            }
            else
            {
                start += 10;
                web.Navigate(string.Format("http://www.douban.com/people/{0}/notes?start={1}", peopleId, start));
            }
            tmrDownload.Enabled = false;
        }
    }
}
