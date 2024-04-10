using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace FotoGal
{
    public partial class Form2 : Form
    {
        private OleDbConnection connection;
        private string secilenKlasor = "";
        Class1 mouse_btn = new Class1();
        private string[] selectedPaths;
        string filePath = "kurulum_yollari.txt";

        public string[] SelectedPaths
        {
            get { return selectedPaths; }
            set { selectedPaths = value; }
        }
        public Form2()
        {
            InitializeComponent();
            this.Text = "Müşteri Vesikalık";
        }
        private void Form2_Load(object sender, EventArgs e)
        {
           
            if (File.Exists(filePath))
            {
                string[] lines = File.ReadAllLines(filePath);
                if (lines.Length >= 3)
                {
                    string veritabaniDosyaYolu = lines[2]; // 3. satırdaki veritabanı dosyası yolu
                    label1.Text = "Veritabanı Dosyası Yolu: " + veritabaniDosyaYolu;
                    try
                    {
                        // Bağlantıyı oluşturun ve açın
                        connection = new OleDbConnection(@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + veritabaniDosyaYolu);
                        connection.Open();

                        // DataGrid'i doldur
                        LoadData();

                        // Arka plan rengini ayarla
                        this.BackColor = ColorTranslator.FromHtml("#4B98AD");
                        pictureBox1.BackColor = Color.White;
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Veritabanı bağlantısı kurulurken bir hata oluştu: " + ex.Message);
                    }
                }
                else
                {
                    MessageBox.Show("Dosya yolunda yeterli bilgi bulunamadı.");
                }
            }
            else
            {
                MessageBox.Show("kurulum_yollari.txt dosyası bulunamadı.");
            }

            // DataGridView özelliği ayarla
            dataGridView1.ReadOnly = true;

            dataGridView1.ReadOnly = true;
            this.BackColor = ColorTranslator.FromHtml("#4B98AD");
            pictureBox1.BackColor = Color.White;
        }


        private void LoadData()
        {
            try
            {
                // Tüm verileri getiren sorguyu oluştur
                string query = "SELECT VskNo, MusteriName FROM Tablo1";

                // OleDbCommand nesnesini oluştur
                OleDbCommand selectCommand = new OleDbCommand(query, connection);

                // Veritabanına bağlı bir OleDbDataAdapter oluştur ve SelectCommand'i ayarla
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                adapter.SelectCommand = selectCommand;

                // Verileri tutmak için bir DataTable oluştur
                DataTable dataTable = new DataTable();

                // OleDbDataAdapter ile DataTable'ı doldur
                adapter.Fill(dataTable);

                // DataGridView'e DataTable'ı ata
                dataGridView1.DataSource = dataTable;
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veri yüklenirken bir hata oluştu: " + ex.Message);
            }
        }


        private void textBox_Search_TextChanged(object sender, EventArgs e)
        {
            // TextBox'ta yazılan değeri alın
            string musteriAdi = textBox_Search.Text.Trim();

            // Veritabanında müşteri adına göre arama yapmak için sorguyu oluşturun
            string query = "SELECT VskNo, MusteriName FROM Tablo1 WHERE MusteriName LIKE @musteriAdi";

            try
            {
                // Veritabanı bağlantısını açın ve sorguyu çalıştırın
                using (OleDbCommand command = new OleDbCommand(query, connection))
                {
                    // Parametreyi ekleyin
                    command.Parameters.AddWithValue("@musteriAdi", "%" + musteriAdi + "%");

                    // Verileri almak için bir OleDbDataAdapter oluşturun
                    using (OleDbDataAdapter adapter = new OleDbDataAdapter(command))
                    {
                        // DataSet'e verileri doldurun
                        DataSet dataSet = new DataSet();
                        adapter.Fill(dataSet);

                        // DataGridView'e DataSet'teki verileri atayın
                        dataGridView1.DataSource = dataSet.Tables[0];
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Arama işlemi sırasında bir hata oluştu: " + ex.Message);
            }
        }
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            // Sadece satırın içine tıklanırsa işlem yap
            if (e.RowIndex >= 0)
            {
                DataGridViewRow selectedRow = dataGridView1.Rows[e.RowIndex];
                string[] lines = File.ReadAllLines(filePath);
                string vesikalıkYolu = lines[1]; // 3. satırdaki veritabanı dosyası yolu
                // Seçilen satırdan VskNo değerini al
                if (selectedRow.Cells["VskNo"].Value != null)
                {
                    int vskNo = Convert.ToInt32(selectedRow.Cells["VskNo"].Value);

                    // Veritabanından VskNo'ya göre resim dosyasını al
                    string resimDosyasi = Path.Combine(vesikalıkYolu, vskNo.ToString() + ".jpg");

                    // Resim dosyası var mı kontrol et
                    if (File.Exists(resimDosyasi))
                    {
                        // Resmi boyutlandır
                        //SetPictureBoxSize(resimDosyasi);

                        // Resmi PictureBox'a yükle
                        pictureBox1.Image = Image.FromFile(resimDosyasi);

                        // PictureBox'ın Tag özelliğine resim dosyasının yolunu ata
                        pictureBox1.Tag = resimDosyasi;

                        // Vesikalik numarasını label1'e yazdır
                        label1.Text = "Vesikalık Numarası: " + vskNo.ToString();
                    }
                    else
                    {
                        MessageBox.Show("Resim dosyası bulunamadı.");
                    }
                }
            }
        }/*
        private void SetPictureBoxImage(string imagePath)
        {
            // Resmi dosyadan yükleyin
            Image image = Image.FromFile(imagePath);

            // Resmin orijinal boyutlarını alın
            int originalWidth = image.Width;
            int originalHeight = image.Height;

            // PictureBox'ın boyutlarını alın
            int pictureBoxWidth = pictureBox1.Width;
            int pictureBoxHeight = pictureBox1.Height;

            // Resmin orijinal boyutlarına göre PictureBox içindeki boyutlarını belirleyin
            if (originalWidth > pictureBoxWidth || originalHeight > pictureBoxHeight)
            {
                double widthRatio = (double)pictureBoxWidth / (double)originalWidth;
                double heightRatio = (double)pictureBoxHeight / (double)originalHeight;
                double ratio = Math.Min(widthRatio, heightRatio);

                int newWidth = (int)(originalWidth * ratio);
                int newHeight = (int)(originalHeight * ratio);

                // Resmi ölçekleyerek PictureBox'a ayarlayın
                pictureBox1.Image = new Bitmap(image, newWidth, newHeight);
            }
            else
            {
                // Resmi PictureBox'a doğrudan ayarlayın
                pictureBox1.Image = image;
            }

            // PictureBox'ın içeriği merkezlemek için PictureBox özelliklerini güncelleyin
            pictureBox1.SizeMode = PictureBoxSizeMode.CenterImage;
        }

        */

        private void btn_folder_Click(object sender, EventArgs e)
        {
            // Klasör seçme diyaloğu aç
            FolderBrowserDialog dialog = new FolderBrowserDialog();
            dialog.Description = "Resimler Klasörünü Seçin";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                // Seçilen klasörü kaydet
                secilenKlasor = dialog.SelectedPath;

                // Klasörün varlığını kontrol et
                if (Directory.Exists(secilenKlasor))
                {
                    // Klasör seçildi mesajı göster
                    MessageBox.Show("Klasör seçildi: " + secilenKlasor);
                }
                else
                {
                    // Geçersiz klasör mesajı göster
                    MessageBox.Show("Geçersiz bir klasör seçildi.");
                }
            }
        }

        private void btn_customer_Click(object sender, EventArgs e)
        {// Dosya seçme işlemi başlat
            // Dosya seçme işlemi başlat
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Image Files (*.jpg;*.jpeg;*.png;*.bmp)|*.jpg;*.jpeg;*.png;*.bmp|All Files (*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    // Seçilen resmin adını ve yolunu al
                    string resimYolu = openFileDialog.FileName;
                    string resimAdi = Path.GetFileName(resimYolu);

                    // Resmin adından VskNo'yu al
                    string vskNoStr = Path.GetFileNameWithoutExtension(resimAdi);
                    int vskNo;
                    if (!int.TryParse(vskNoStr, out vskNo))
                    {
                        MessageBox.Show("Resim adında geçerli bir VskNo bulunamadı.");
                        return;
                    }

                    // TextBox'tan müşteri adını al
                    string musteriAdi = textBox_Search.Text;

                    // Veritabanına ekleme işlemi için sorguyu oluşturun
                    string insertQuery = "INSERT INTO Tablo1 (VskNo, MusteriName) VALUES (@vskNo, @musteriAdi)";

                    // Yeni bir OleDbCommand oluşturun ve sorguyu bağlantıyla birlikte kullanın
                    using (OleDbCommand insertCommand = new OleDbCommand(insertQuery, connection))
                    {
                        // Parametreleri ekleyin
                        insertCommand.Parameters.AddWithValue("@vskNo", vskNo);
                        insertCommand.Parameters.AddWithValue("@musteriAdi", musteriAdi);

                        try
                        {
                            // Veriyi ekleyin
                            int rowsAffected = insertCommand.ExecuteNonQuery();
                            if (rowsAffected > 0)
                            {
                                MessageBox.Show("Veri başarıyla eklendi. Yeni Vesikalık Numarası: " + vskNo);
                            }
                            else
                            {
                                MessageBox.Show("Veri eklenirken bir hata oluştu.");
                            }
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Hata: " + ex.Message);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Resim yüklenirken bir hata oluştu: " + ex.Message);
                }
            }

        }
       

        private void PictureBoxDoubleClick(object sender, EventArgs e)
        {
            try
            {
                PictureBox pictureBox = sender as PictureBox;
                if (pictureBox != null && pictureBox.Tag != null)
                {
                    string imagePath = pictureBox.Tag.ToString();
                    OpenFileWithAdobePhotoshop(imagePath);
                }
                else
                {
                    MessageBox.Show("PictureBox veya Tag özelliği null.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata: " + ex.Message);
            }
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

      


        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Form kapatıldığında veritabanı bağlantısını kapat
            if (connection != null && connection.State == ConnectionState.Open)
            {
                connection.Close();
            }
        }

        private void pictureBox1_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                
            }
        }
    }

}
