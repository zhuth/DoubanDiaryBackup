using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Data.SQLite;

namespace DoubanDiaryBackup
{
    public partial class Form1 : Form
    {
        int step = -1, start = 0;
        string task = "";
        string peopleId = "";
        string do_status = "collect";
        SQLiteConnection conn = null;
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

        private HtmlElement getElementByTagAndClass(HtmlElement container, string tagname, string className)
        {
            className = className.ToLower();
            foreach (HtmlElement c in container.GetElementsByTagName(tagname))
            {
                if (c.GetAttribute("className").ToLower() == className)
                    return c;
            }
            return null;
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
                        conn = new SQLiteConnection("data source='" + peopleId + ".db3'");
                        conn.Open();
                        SQLiteCommand cmd = new SQLiteCommand("CREATE TABLE IF NOT EXISTS list (id VARCHAR PRIMARY KEY, type VARCHAR, title VARCHAR, status VARCHAR, comment VARCHAR, date VARCHAR, rate VARCHAR, tags VARCHAR) ", conn);
                        cmd.ExecuteNonQuery();
                        this.Text = "已登录 - " + peopleId;
                        step = 1; start = 0;
                        urls.Clear();
                        return;
                    }
                    break;

                case 1: // 抓取
                    var trans = conn.BeginTransaction();
                    int c = 0;
                    HtmlElement article = getElementByTagAndClass(web.Document.GetElementsByTagName("html")[0], "div", "article");
                    if (article == null)
                    {
                        error("找不到列表。");
                        return;
                    }
                    if (task.StartsWith("list"))
                    {
                        var items = article.GetElementsByTagName("li");
                        foreach (HtmlElement item in items)
                        {
                            if (item.GetAttribute("className") != "item")
                                continue;
                            string id = getElementByTagAndClass(item, "div", "title").GetElementsByTagName("a")[0].GetAttribute("href"),
                                title = getElementByTagAndClass(item, "div", "title").GetElementsByTagName("a")[0].InnerText,
                                date = getElementByTagAndClass(item, "div", "date").InnerText,
                                tags = (getElementByTagAndClass(item, "span", "tags") == null ? "" : getElementByTagAndClass(item, "span", "tags").InnerText),
                                comment = (getElementByTagAndClass(item, "div", "comment") == null ? "" : getElementByTagAndClass(item, "div", "comment").InnerText);
                            var dater = getElementByTagAndClass(item, "div", "date");
                            int rate = 0;
                            for (rate = 1; rate <= 5; ++rate)
                                if (getElementByTagAndClass(dater, "span", "rating" + rate + "-t") != null)
                                    break;
                            if (rate > 5)
                                rate = 0;
                            string[] args = new string[] { id, task, title, do_status, comment, date, rate.ToString(), tags };
                            for (int i = 0; i < args.Length; ++i)
                            {
                                args[i] = args[i].Replace("'", "''");
                            }
                            new SQLiteCommand(string.Format(
                                "REPLACE INTO list(id, type, title, status, comment, date, rate, tags) VALUES('{0}', '{1}', '{2}', '{3}', '{4}', '{5}', '{6}', '{7}')",
                                args)
                                , conn).ExecuteNonQuery();
                            ++c;
                        }
                    }
                    else if (task == "status")
                    {
                        foreach (HtmlElement item in article.GetElementsByTagName("div"))
                        {
                            if (item.GetAttribute("className") != "status-item")
                                continue;
                            string id = getElementByTagAndClass(item, "span", "created_at").GetElementsByTagName("a")[0].GetAttribute("href"),
                                date = getElementByTagAndClass(item, "span", "created_at").GetAttribute("title"),
                                content = (item.GetElementsByTagName("blockquote").Count > 0 ? item.GetElementsByTagName("blockquote")[0].InnerHtml : "");

                            string[] args = new string[] { id, date, content };
                            for (int i = 0; i < args.Length; ++i)
                                args[i] = args[i].Replace("'", "''");
                            
                            int q = new SQLiteCommand(string.Format(
                                "REPLACE INTO list(id, title, date, type) VALUES('{0}', '{1}', '{2}', 'status')",
                                args)
                                , conn).ExecuteNonQuery();
                            if (q == 0)
                                Console.WriteLine(args);
                            ++c;
                        }
                    }
                    trans.Commit();
                    foreach (HtmlElement link in article.GetElementsByTagName("a"))
                    {
                        string href = link.GetAttribute("href");
                        if (matchTask(href))
                        {
                            if (!downloaded.ContainsKey(href))
                            {
                                ++c;
                                if (task == "diary" || task == "review" || (task == "status" && Properties.Settings.Default.获取广播详情))
                                    urls.Enqueue(href); 
                            }
                        }
                    }
                    if (c > 0)
                        tmrDownload.Enabled = true;
                    else
                    {
                        if (task.StartsWith("list"))
                        {
                            if (do_status == "collect")
                            {
                                do_status = "do"; start = 0;
                                tmrDownload.Enabled = true;
                                return;
                            }
                            else if (do_status == "do")
                            {
                                do_status = "wish"; start = 0;
                                tmrDownload.Enabled = true;
                                return;
                            } else if (do_status == "wish")
                            {
                                do_status = "collect"; start = 0;
                            }
                        }
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
            if (!downloaded.ContainsKey(e.Url.ToString()))
                downloaded.Add(e.Url.ToString(), true);
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

        private void 设置ToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }
    }
}
