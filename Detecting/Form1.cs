using Alturos.Yolo;
using Alturos.Yolo.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Detecting
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            var configDetector = new ConfigurationDetector();
            var config = configDetector.Detect();
            var yolo = new YoloWrapper(config);
            var memoryStream = new MemoryStream();
            picImage.Image.Save(memoryStream, ImageFormat.Png);
            var items = yolo.Detect(memoryStream.ToArray()).ToList();
            AddDetailsToPictureBox(picImage, items);
        }

        void AddDetailsToPictureBox(PictureBox picBoxRender, List<YoloItem> items)
        {
            var img = picBoxRender.Image;

            var font = new Font("Arial", 50, FontStyle.Regular);
            var brush = new SolidBrush(Color.LightGreen);

            var x1 = 1;
            var y1 = 1;
            var point1 = new Point(x1, y1);
            int antal = items.Count;

            var graphics = Graphics.FromImage(img);
            foreach(var item in items)
            {
                
                var x = item.X;
                var y = item.Y;
                var width = item.Width;
                var height = item.Height;

                var rect = new Rectangle(x, y, width, height);
                var pen = new Pen(Color.LightGreen, 6);

                var point = new Point(x, y);

                graphics.DrawRectangle(pen, rect);
                graphics.DrawString(item.Type, font, brush, point);
                graphics.DrawString("Antal Objekt: " + antal, font, brush, point1);

            }
            picBoxRender.Image = img;
        }

        private void btnBrowse_Click(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog();
            ofd.Filter = "Image Files|*.jpg;*.png";
            if(ofd.ShowDialog() == DialogResult.OK)
            {
                picImage.Image = Image.FromFile(ofd.FileName);
            }
        }
    }
}
