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
        string task = "";
        string peopleId = "";
        string do_status = "collect";
        Queue<string> urls = new Queue<string>();
        Dictionary<string, bool> downloaded = new Dictionary<string, bool>();

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            web.Navigate("https://www.douban.com/accounts/login?redir=http%3A//www.douban.com/mine/");
        }

        private void 开始抓取ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "diary"; start = 0; step = 1;
            loadList();
            web.Navigate(string.Format("http://www.douban.com/people/{0}/notes?start={1}", peopleId, start));
        }

        private void error(string message)
        {
            MessageBox.Show(message);
            step = -1;
            web.GoBack();
        }

        private bool matchTask(string url)
        {
            url = url.ToLower();
            switch(task) {
                case "diary":
                    return url.StartsWith("http://www.douban.com/note/") && url.EndsWith("/");
                case "review":
                    return url.Contains(".douban.com/review/") && url.EndsWith("/");
                case "status":
                    return url.StartsWith("http://www.douban.com/people/" + peopleId + "/status/") && url.EndsWith("/");
                case "listbook":
                case "listmovie":
                case "listmusic":
                    return url.StartsWith("http://" + task.Substring(4) + ".douban.com/subject/") && url.EndsWith("/");
                default:
                    return false;
            }
        }

        private void web_Navigated(object sender, WebBrowserNavigatedEventArgs e)
        {
        }

        private void web_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            switch (step)
            {
                case -1:
                    if (string.IsNullOrEmpty(peopleId))
                        step = 0;
                    break;
                case 0: // 登录
                    if (string.IsNullOrEmpty(peopleId))
                    {
                        for (int i = 0; i < e.Url.Segments.Length; ++i)
                            if (e.Url.Segments[i].ToLower() == "people/" && i < e.Url.Segments.Length - 1)
                            {
                                peopleId = e.Url.Segments[i + 1]; peopleId = peopleId.Substring(0, peopleId.Length - 1);
                            }
                    }
                    if (string.IsNullOrEmpty(peopleId))
                    {
                        error("导航失败，请登录后重试。");
                    }
                    else
                    {
                        this.Text = "已登录 - " + peopleId;
                        step = 1; start = 0;
                        urls.Clear();
                        return;
                    }
                    break;

                case 1: // 抓取
                    int c = 0;
                    var divs = web.Document.GetElementsByTagName("div");
                    HtmlElement article = null;
                    foreach (HtmlElement div in divs)
                    {
                        Console.WriteLine(div.Style);
                        if (div.GetAttribute("className") == "article") { article = div; break; }
                    }
                    if (article == null)
                    {
                        error("找不到列表。");
                        return;
                    }
                    foreach (HtmlElement link in article.GetElementsByTagName("a"))
                    {
                        string ls = link.GetAttribute("href");
                        if (matchTask(ls))
                        {
                            if (!downloaded.ContainsKey(ls))
                            {
                                ++c;
                                downloaded.Add(ls, true);
                                if (task == "diary" || task == "review" || (task == "status" && Properties.Settings.Default.获取广播详情))
                                    urls.Enqueue(ls); 
                            }
                        }
                    }
                    if (c > 0)
                        tmrDownload.Enabled = true;
                    else
                    {
                        error("下载队列完毕！");
                        using (StreamWriter sw = new StreamWriter(peopleId + "\\" +  task + ".list"))
                        {
                            foreach (string key in downloaded.Keys)
                            {
                                sw.WriteLine(key);
                            }
                        }
                    }
                    break;
            }
        }

        private void notes_DocumentCompleted(object sender, WebBrowserDocumentCompletedEventArgs e)
        {
            //if (!e.Url.AbsolutePath.Contains(".douban.com")) return;
            string id = e.Url.Segments[e.Url.Segments.Length - 1];
            id = task + id;
            if (id.EndsWith("/")) id = id.Substring(0, id.Length - 1);

            if (!Directory.Exists(peopleId)) Directory.CreateDirectory(peopleId);
            if (notes.Document.GetElementsByTagName("h1").Count == 1)
            {
                foreach (HtmlElement div in notes.Document.GetElementsByTagName("div"))
                {

                    switch (task)
                    {
                        case "diary":
                            if (div.GetAttribute("className") == "article")
                            {
                                File.WriteAllText(peopleId + "\\" + id + ".html", div.InnerHtml);
                                break;
                            }
                            break;
                        case "review":
                        case "status":
                            if (div.GetAttribute("id") == "content")
                            {
                                File.WriteAllText(peopleId + "\\" + id + ".html", div.InnerHtml);
                                break;
                            }
                            break;
                    }
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
                switch (task)
                {
                    case "diary":
                        start += 10;
                        web.Navigate(string.Format("http://www.douban.com/people/{0}/notes?start={1}", peopleId, start));
                        break;
                    case "review":
                        start += 10;
                        web.Navigate(string.Format("http://www.douban.com/people/{0}/reviews?start={1}", peopleId, start));
                        break;
                    case "status":
                        start += 1;
                        web.Navigate(string.Format("http://www.douban.com/people/{0}/statuses?p={1}", peopleId, start));
                        break;
                    case "listbook":
                    case "listmovie":
                    case "listmusic":
                        start += 30;
                        web.Navigate(
                            string.Format("http://{3}.douban.com/people/{0}/{1}?sort=time&start={2}&filter=all&mode=list&tags_sort=count", peopleId, do_status, start, task.Substring(4))
                            );
                        break;
                }
            }
            tmrDownload.Enabled = false;
        }

        private void loadList()
        {
            downloaded.Clear();
            if (File.Exists(peopleId + "\\" + task + ".list"))
            {
                foreach (string url in File.ReadAllLines(peopleId + "\\" + task + ".list"))
                {
                    downloaded.Add(url, true);
                }
            }
        }

        private void 抓取评论ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "review"; start = 0; step = 1;
            web.Navigate(string.Format("http://www.douban.com/people/{0}/reviews?start={1}", peopleId, start));
        }

        private void 重试ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            switch(task)
            {
                case "diary":
                case "review":
                case "status":
                    notes.Navigate(notes.Url);
                    break;
                default:
                    web.Navigate(web.Url);
                    break;
            }
        }

        private void 抓取广播ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "status";
            start = 1; step = 1;
            web.Navigate(string.Format("http://www.douban.com/people/{0}/statuses?p={1}", peopleId, start));
        }

        private void 抓取书籍列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "listbook";
            start = -1; step = 1;
            tmrDownload.Enabled = true;
        }

        private void 抓取电影列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "listmovie";
            start = -1; step = 1;
            tmrDownload.Enabled = true;
        }

        private void 抓取音乐列表ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            task = "listmusic";
            start = -1; step = 1;
            tmrDownload.Enabled = true;
        }
    }
}
