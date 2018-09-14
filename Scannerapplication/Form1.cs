using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WIATest;

namespace Scannerapplication
{
    public partial class Form1 : Form
    {
        bool cansaveImage = false;
        public Form1(string[] args)
        {
            InitializeComponent();
            SetAllControlsFont(Controls);
            cansaveImage = false;
            if (args.Length>=1) textBox1.Text = args[0];
            if (args.Length>1) textBox2.Text = args[1];
            try
            {
                //get list of devices available
                List<string> devices = WIAScanner.GetDevices();

                foreach (string device in devices)
                {
                    lbDevices.Items.Add(device);
                }
                //check if device is not available
                if (lbDevices.Items.Count == 0)
                {
                    MessageBox.Show("Você não possui nenhum scanner instalado","Error",MessageBoxButtons.OK,MessageBoxIcon.Error);
                }
                else
                {
                    lbDevices.SelectedIndex = 0;
                }
            }
            catch
            {

            }
           
        }

      

        //button click event
        private void btn_scan_Click(object sender, EventArgs e)
        {
            try
            {
                //get images from scanner
                // List<Image> images = WIAScanner.Scan(((string)lbDevices.SelectedItem).Split('|')[1]);
                List<Image> images = WIAScanner.Scan();
                foreach (Image image in images)
                {
                    pic_scan.Image = image;
                    pic_scan.BackColor = Color.Transparent;
                    pic_scan.Show();
                    pic_scan.SizeMode = PictureBoxSizeMode.StretchImage;
                    cansaveImage = true;
                    //save scanned image into specific folder
                  //  image.Save(@"D:\" + DateTime.Now.ToString("yyyy-MM-dd HHmmss") + ".jpeg", ImageFormat.Jpeg);
                }
            }
            catch (Exception exc)
            {
                MessageBox.Show(exc.Message);
            }
        }


        private void Home_SizeChanged(object sender, EventArgs e)
        {
            int pheight = this.Size.Height - 153;
            pic_scan.Size = new Size(pheight - 150, pheight);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
            }
        }

        private void textBox2_Leave(object sender, EventArgs e)
        {
            if(!textBox2.Text.Contains(".jpeg"))
            {
                textBox2.Text += ".jpeg";
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (pic_scan.Image != null && !String.IsNullOrEmpty(textBox1.Text) && !string.IsNullOrEmpty(textBox2.Text))
            {
                try
                {
                    if (!Directory.Exists(textBox1.Text)) Directory.CreateDirectory(textBox1.Text);
                    if (File.Exists(Path.Combine(textBox1.Text, textBox2.Text))) File.Delete(Path.Combine(textBox1.Text, textBox2.Text));
                }
                catch
                {
                    MessageBox.Show("O caminho para salver o arquivo não é válido");
                    return;

                }
                try
                {
                    pic_scan.Image.Save(Path.Combine(textBox1.Text, textBox2.Text), ImageFormat.Jpeg);
                    MessageBox.Show("Imagem salva com sucesso, pressione OK para sair");
                    Environment.Exit(0);
                }
                catch
                {
                    MessageBox.Show("Falha ao salvar a imagem, cheque se o nome e o caminho são válidos");
                    return;
                }
            }
            else if(String.IsNullOrEmpty(textBox1.Text) ||string.IsNullOrEmpty(textBox2.Text))
            {
                MessageBox.Show("Você não preencheu o caminho do arquivo");

            }
            else
            {
                MessageBox.Show("Nenhuma imagem ainda foi escaneada");

            }
        }
        public void SetAllControlsFont(Control.ControlCollection ctrls)
        {
            foreach (Control ctrl in ctrls)
            {
                if (ctrl.Controls != null)
                    SetAllControlsFont( ctrl.Controls);

                try { 
                
                    if (! (ctrl is LinkLabel))
                    ctrl.Font = new Font("tahoma", 10);

                }
                catch
                {

                }
                }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://nathanvmag.github.io");
        }

        private void pnl_capture_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}
