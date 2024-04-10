using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FotoGal
{
    public partial class Main : Form
    {
        private string selectedFolderPath; // Seçilen klasör yolunu saklamak için bir değişken
        private PictureBox pictureBox = new PictureBox(); // Form üzerinde kullanılacak PictureBox nesnesi
        Start_Screen Start_Screen = new Start_Screen();
        public Main()
        {
            InitializeComponent();

           // this.Text = "Naz Fotoğrafçılık";

            textBoxArama.TextChanged += textBoxArama_TextChanged;
            pictureBox.BorderStyle = BorderStyle.FixedSingle;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
        }

        private void Main_Load(object sender, EventArgs e)
        {
            if (!IsFirstRun())
            {
                LoadFolders();
                return;
            }

            Start_Screen.ShowDialog();
            // üst panel rengi
            panel2.BackColor = ColorTranslator.FromHtml("#4B98AD");

            // yan panel rengi
            panelKlasorler.BackColor = ColorTranslator.FromHtml("#4B98AD");



            textBoxArama.Multiline = false;
            textBoxArama.Height = 50;

            // herşey için ekran çözünürlüğü 
            // X Genişlik 
            // y Yükseliklik

            int y, x;
            x = Screen.PrimaryScreen.Bounds.Width;
            y = Screen.PrimaryScreen.Bounds.Height;



            // yan panel yükseklik
             panelKlasorler.Height = y;
            // yan panel ototmatik kaydırma özeliğini aktif eder
            panelKlasorler.AutoScroll = true;

            int pk = panelKlasorler.Width;  
            // resimlerin olduğu panelin genişliğini ayarlar
            panelResimler.Width = x - pk;

            // üst panel genişlik
            panel2.Width = x;

            btn_vsklik.BackColor = ColorTranslator.FromHtml("#FFDE59");
            btn_tkvm.BackColor = ColorTranslator.FromHtml("#FFDE59");
            btn_cp.BackColor = ColorTranslator.FromHtml("#FFDE59");
            //button4.BackColor = ColorTranslator.FromHtml("#FFDE59");


            //  button3.ForeColor = System.Drawing.Color.White;

            // uygulama logosu değiştirildi 
            this.Icon = new System.Drawing.Icon(@"C:\Users\oztur\source\repos\FotoGal\FotoGal\bin\Debug\ıcon.ico");
        }

        private bool IsFirstRun()
        {
            // Daha anlamlı bir dosya adı
            string filePath = "kurulum_tamamlandi.txt";

            // Dosya varlığını kontrol et
            if (File.Exists(filePath))
            {
                return false;
            }

            // Dosya yoksa oluştur ve true döndür
            File.WriteAllText(filePath, DateTime.Now.ToString());
            return true;
        }

        private void LoadFolders()
        {
            // üst panel rengi
            panel2.BackColor = ColorTranslator.FromHtml("#4B98AD");

            // yan panel rengi
            panelKlasorler.BackColor = ColorTranslator.FromHtml("#4B98AD");

            textBoxArama.Multiline = false;
            textBoxArama.Height = 50;

            // herşey için ekran çözünürlüğü 
            // X Genişlik 
            // y Yükseliklik

            int y, x;
            x = Screen.PrimaryScreen.Bounds.Width;
            y = Screen.PrimaryScreen.Bounds.Height;

            // yan panel yükseklik
            panelKlasorler.Height = y;
            // yan panel ototmatik kaydırma özeliğini aktif eder
            panelKlasorler.AutoScroll = true;

            int pk = panelKlasorler.Width;
            // resimlerin olduğu panelin genişliğini ayarlar
            panelResimler.Width = x - pk;

            // üst panel genişlik
            panel2.Width = x;

            btn_vsklik.BackColor = ColorTranslator.FromHtml("#FFDE59");
            btn_tkvm.BackColor = ColorTranslator.FromHtml("#FFDE59");
            btn_cp.BackColor = ColorTranslator.FromHtml("#FFDE59");
            //button4.BackColor = ColorTranslator.FromHtml("#FFDE59");

            //  button3.ForeColor = System.Drawing.Color.White;

            // uygulama logosu değiştirildi 
            this.Icon = new System.Drawing.Icon(@"C:\Users\oztur\source\repos\FotoGal\FotoGal\bin\Debug\ıcon.ico");

            string kurulumYolu = GetKurulumYolu();
            if (!string.IsNullOrEmpty(kurulumYolu))
            {
                ListSubfolders(kurulumYolu);
            }
            else
            {
                MessageBox.Show("Kurulum yolu bulunamadı. Lütfen tekrar kurulum yapın.");
            }
        }
        private string GetKurulumYolu()
        {
            string kurulumYolu = "";
            string kurulumYollariDosyaYolu = "kurulum_yollari.txt";
            if (File.Exists(kurulumYollariDosyaYolu))
            {
                try
                {
                    using (StreamReader sr = new StreamReader(kurulumYollariDosyaYolu))
                    {
                        kurulumYolu = sr.ReadLine();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Kurulum yolu okunurken bir hata oluştu: " + ex.Message);
                }
            }
            return kurulumYolu;
        }

    
    

    private void dosya_Click(object sender, EventArgs e)
        {
            using (FolderBrowserDialog folderSelectDialog = new FolderBrowserDialog())
            {
                folderSelectDialog.RootFolder = Environment.SpecialFolder.Desktop;
                if (folderSelectDialog.ShowDialog() == DialogResult.OK)
                {
                    selectedFolderPath = folderSelectDialog.SelectedPath;
                    ListSubfolders(selectedFolderPath);
                }
            }
        }

        private void textBoxArama_TextChanged(object sender, EventArgs e)
        {
            try
            {
                string arananKelime = textBoxArama.Text.ToLower();
                panelResimler.Controls.Clear();

                int resimWidth = pictureBox.Width;
                int resimHeight = pictureBox.Height;

                int x = 10;
                int y = 10;

                if (!string.IsNullOrEmpty(arananKelime))
                {
                    string[] filtrelenmisResimler = Directory
                      .GetFiles(selectedFolderPath, "*", SearchOption.AllDirectories)
                      .Where(resim =>
                      {
                          string resimAdi = Path.GetFileName(resim).ToLower();
                          return arananKelime.ToLower().Split(' ').All(aranan => resimAdi.Contains(aranan));
                      })
                      .Select(resim => resim.ToLower())
                      .Distinct()
                      .ToArray();

                    foreach (string resim in filtrelenmisResimler)
                    {
                        try
                        {
                            using (Image originalImage = Image.FromFile(resim))
                            {
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Image = new Bitmap(originalImage, resimWidth, resimHeight);
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Location = new Point(x, y);
                                pictureBox.Tag = resim;
                                pictureBox.DoubleClick += PictureBoxDoubleClick;
                                panelResimler.Controls.Add(pictureBox);

                                x += resimWidth + 10;

                                if (x + resimWidth > panelResimler.Width)
                                {
                                    x = 10;
                                    y += resimHeight + 10;
                                }

                                float aspectRatio = (float)pictureBox.Image.Width / pictureBox.Image.Height;

                                if (aspectRatio > 1)
                                {
                                    pictureBox.Width = resimWidth;
                                    pictureBox.Height = (int)(resimWidth / aspectRatio);
                                }
                                else
                                {
                                    pictureBox.Height = resimHeight; pictureBox.Width = (int)(resimHeight * aspectRatio);
                                }
                            }
                        }
                        catch (Exception ex) {
                            MessageBox.Show("Hata Oluştu   : " + ex);
                        };
                    }
                }
                else
                {
                    DirectoryInfo directory = new DirectoryInfo(selectedFolderPath);
                    foreach (FileInfo file in directory.GetFiles("*", SearchOption.AllDirectories))
                    {
                        try
                        {
                            using (Image originalImage = Image.FromFile(file.FullName))
                            {
                                PictureBox pictureBox = new PictureBox();
                                pictureBox.Image = new Bitmap(originalImage, resimWidth, resimHeight);
                                pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
                                pictureBox.Location = new Point(x, y);
                                pictureBox.Tag = file.FullName;
                                pictureBox.Click += PictureBoxDoubleClick;
                                panelResimler.Controls.Add(pictureBox);

                                x += resimWidth + 10;

                                if (x + resimWidth > panelResimler.Width)
                                {
                                    x = 10;
                                    y += resimHeight + 10;
                                }

                                float aspectRatio = (float)pictureBox.Image.Width / pictureBox.Image.Height;

                                if (aspectRatio > 1)
                                {
                                    pictureBox.Width = resimWidth;
                                    pictureBox.Height = (int)(resimWidth / aspectRatio);
                                }
                                else
                                {
                                    pictureBox.Height = resimHeight;
                                    pictureBox.Width = (int)(resimHeight * aspectRatio);
                                }
                            }
                        }
                        catch (Exception ex) { }
                    }
                }
            }
            catch (Exception ex) { }
        }


        private void ListSubfolders(string folderPath)
        {
            panelKlasorler.Controls.Clear();
            panelResimler.Controls.Clear();
            int butonsol = 50;
            string[] subfolders = Directory.GetDirectories(folderPath, "*", SearchOption.AllDirectories);
            foreach (string subfolder in subfolders)
            {
                string subfolderName = Path.GetFileName(subfolder);
                Button btnSubfolder = new Button
                {

                    Top = butonsol,
                    Left = panelKlasorler.Width / 2 - 50,
                    Width = 100,
                    Height = 30,
                    ForeColor = Color.Black,
                    Text = subfolderName,
                    Tag = subfolder
                };
                btnSubfolder.ForeColor = Color.White;
                btnSubfolder.Click += BtnSubfolder_Click;
                panelKlasorler.Controls.Add(btnSubfolder);
                butonsol += 40;
            }
        }


        private void BtnSubfolder_Click(object sender, EventArgs e)
        {
            panelResimler.Controls.Clear();

            Button btn = (Button)sender;
            string selectedSubfolderPath = btn.Tag.ToString();
            string[] imageFiles = Directory.GetFiles(selectedSubfolderPath).Where(IsImageFile).ToArray();
            int x = 10;
            int y = 10;
            int width = 100;
            int height = 100;

            foreach (string imageFilePath in imageFiles)
            {
                try
                {
                    PictureBox pictureBox = new PictureBox
                    {
                        SizeMode = PictureBoxSizeMode.StretchImage,

                        Width = width,
                        Height = height
                    };

                    pictureBox.Image = Image.FromFile(imageFilePath);
                    float aspectRatio = (float)pictureBox.Image.Width / pictureBox.Image.Height;

                    if (aspectRatio > 1)
                    {
                        pictureBox.Width = width;
                        pictureBox.Height = (int)(width / aspectRatio);
                    }
                    else
                    {
                        pictureBox.Height = height;
                        pictureBox.Width = (int)(height * aspectRatio);
                    }

                    pictureBox.Location = new Point(x, y);
                    panelResimler.Controls.Add(pictureBox);
                    x += pictureBox.Width + 10;
                    if (x + pictureBox.Width > panelResimler.Width)
                    {
                        x = 10;
                        y += pictureBox.Height + 10;
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Hata: " + ex);
                }
            }
        }

        private bool IsImageFile(string filePath)
        {
            string[] imageExtensions = { ".jpg", ".jpeg", ".png" };
            return imageExtensions.Contains(Path.GetExtension(filePath).ToLower());
        }

        private void PictureBoxDoubleClick(object sender, EventArgs e)
        {
            PictureBox pictureBox = sender as PictureBox;
            string imagePath = pictureBox.Tag.ToString();
            OpenFileWithAdobePhotoshop(imagePath);
        }

        private void OpenFileWithAdobePhotoshop(string filePath)
        {
            try
            {
                string photoshopPath = @"C:\Program Files\Adobe\Adobe Photoshop 2023\Photoshop.exe";
                Process.Start(photoshopPath, filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Dosya açma hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void RearrangePictures(int spaceBetweenImages)
        {
            int x = 0;
            foreach (Control control in panelResimler.Controls)
            {
                if (control is PictureBox picBox)
                {
                    picBox.Left = x;
                    x += picBox.Width + spaceBetweenImages;
                }
            }
        }

        private void btn_vsklik_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();  
            form2.ShowDialog();
        }

        private void btn_cp_Click(object sender, EventArgs e)
        {
            Start_Screen str
                = new Start_Screen();
            str.ShowDialog();
        }

        private void btn_tkvm_Click(object sender, EventArgs e)
        {
            Calendar calendar = new Calendar();
            calendar.ShowDialog();
        }
    }






}
