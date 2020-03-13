using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace FileCompare
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        List<FileRead> ListKaynak = new List<FileRead>();
        List<FileRead> ListHedef = new List<FileRead>();

        private void btnCompare_Click(object sender, EventArgs e)
        {
            ListKaynak = new List<FileRead>();
            ListHedef = new List<FileRead>();

            List<string> files = new List<string>();
            files.AddRange(Directory.GetFiles(txtKaynak.Text, "*.*", SearchOption.AllDirectories));

            foreach (var item in files)
            {
                FileRead fileRead = new FileRead();
                fileRead.Path = item.Replace(txtKaynak.Text, "");
                fileRead.FullPath = Path.GetFullPath(item);
                fileRead.FileName = Path.GetFileName(item);
                fileRead.CreationTime = File.GetCreationTime(item);
                fileRead.LastWriteTime = File.GetLastWriteTime(item);
                fileRead.LastWriteTimeString = File.GetLastWriteTime(item).ToString("dd.MM.yyyy HH:mm");
                fileRead.Size = new System.IO.FileInfo(item).Length;
                ListKaynak.Add(fileRead);
            }


            files = new List<string>();
            files.AddRange(Directory.GetFiles(txtHedef.Text, "*.*", SearchOption.AllDirectories));

            foreach (var item in files)
            {
                FileRead fileRead = new FileRead();
                fileRead.Path = item.Replace(txtHedef.Text, "");
                fileRead.FullPath = Path.GetFullPath(item);
                fileRead.FileName = Path.GetFileName(item);
                fileRead.CreationTime = File.GetCreationTime(item);
                fileRead.LastWriteTime = File.GetLastWriteTime(item);
                fileRead.LastWriteTimeString = File.GetLastWriteTime(item).ToString("dd.MM.yyyy HH:mm");
                fileRead.Size = new System.IO.FileInfo(item).Length;
                ListHedef.Add(fileRead);
            }

            CompareRun();
        }

        private void CompareRun()
        {
            listView1.Items.Clear();
            foreach (FileRead item in ListKaynak)
            {
                FileRead fileReadHedef = ListHedef.Find(f => f.Path == item.Path);
                if (fileReadHedef != null)//file varsa
                {
                    if (fileReadHedef.LastWriteTimeString != item.LastWriteTimeString || fileReadHedef.Size != item.Size)
                    {
                        ListViewItem li = new ListViewItem();
                        li.Text = "Değişen";
                        li.SubItems.Add(item.Path);
                        li.SubItems.Add(item.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                        li.SubItems.Add(fileReadHedef.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                        li.ForeColor = System.Drawing.Color.Blue;
                        listView1.Items.Add(li);
                    }
                }
                else
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = "Dosya Yok";
                    li.SubItems.Add(item.Path);
                    li.SubItems.Add(item.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                    li.ForeColor = System.Drawing.Color.Green;
                    listView1.Items.Add(li);
                }
            }

            foreach (FileRead item in ListHedef)
            {
                FileRead fileReadKaynak = ListKaynak.Find(f => f.Path == item.Path);
                if (fileReadKaynak != null)//file varsa
                {
                    //if (fileReadKaynak.LastWriteTime != item.LastWriteTime)
                    //{
                    //    listBox1.Items.Add("Değişen - " + item.Path);
                    //}
                }
                else
                {
                    ListViewItem li = new ListViewItem();
                    li.Text = "Dosya Fazla";
                    li.SubItems.Add(item.Path);
                    li.SubItems.Add(item.LastWriteTime.ToString("dd.MM.yyyy HH:mm"));
                    li.ForeColor = System.Drawing.Color.Red;
                    listView1.Items.Add(li);
                }
            }
        }



        public class FileRead
        {
            public string FileName { get; set; }
            public string Path { get; set; }
            public string FullPath { get; set; }
            public long Size { get; set; }
            public DateTime CreationTime { get; set; }
            public DateTime LastWriteTime { get; set; }
            public string LastWriteTimeString { get; set; }
        }
    }
}
