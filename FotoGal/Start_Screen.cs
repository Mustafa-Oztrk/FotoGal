using System;
using System.IO;
using System.Windows.Forms;

namespace FotoGal
{
    public partial class Start_Screen : Form
    {
        private static string firstRunFilePath = "first_run.txt";

        public Start_Screen()
        {
            InitializeComponent();
        }

        private void Start_Screen_Load(object sender, EventArgs e)
        {
            // Uygulamanın ilk açılışı kontrolü
            if (!IsFirstRun())
            {
                this.Hide();
                return;
            }

            // Formu göster
            this.Show();
        }

        private bool IsFirstRun()
        {
            // firstRunFilePath dosyasının varlığını kontrol et
            if (!File.Exists(firstRunFilePath))
            {
                // Dosya yoksa, uygulama ilk kez çalışıyor demektir.
                // Dosyayı oluştur ve true döndür.
                File.WriteAllText(firstRunFilePath, DateTime.Now.ToString());
                return true;
            }

            // Dosya varsa, uygulama daha önce açılmış demektir.
            return false;
        }

        private void Start_Screen_FormClosed(object sender, FormClosedEventArgs e)
        {
            // Form kapatıldığında uygulamayı tamamen kapat
            Application.Exit();
        }

        private void btn_foto_sec_Click(object sender, EventArgs e)
        {
            // Ana klasör seçme işlemi için bir iletişim kutusu aç
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox1.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btn_vsk_sec_Click(object sender, EventArgs e)
        {
            // Vesikalık klasör seçme işlemi için bir iletişim kutusu aç
            using (FolderBrowserDialog folderBrowserDialog = new FolderBrowserDialog())
            {
                if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
                {
                    textBox2.Text = folderBrowserDialog.SelectedPath;
                }
            }
        }

        private void btn_veri_sec_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Filter = "Access Veritabanı|*.accdb|Tüm Dosyalar|*.*";
                openFileDialog.FilterIndex = 1;
                openFileDialog.RestoreDirectory = true;

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string selectedFilePath = openFileDialog.FileName;
                    textBox3.Text = selectedFilePath;

                    // Uygulamanın çalıştığı dizini al
                    string appDirectory = AppDomain.CurrentDomain.BaseDirectory;

                    try
                    {
                        // Dosyanın adını ve yolunu al
                        string fileName = Path.GetFileName(selectedFilePath);

                        // Uygulamanın çalıştığı dizinde bir kısayol oluştur
                        string shortcutPath = Path.Combine(appDirectory, fileName);

                        // Dosyayı kopyala (kısayol oluştur)
                        File.Copy(selectedFilePath, shortcutPath, true);

                        // Kullanıcıya bilgi mesajı göster
                        MessageBox.Show($"Veritabanı dosyası uygulamanın içine kopyalandı: {shortcutPath}", "Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Dosya kopyalama hatası: {ex.Message}", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void btn_Iptal_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(textBox1.Text) || string.IsNullOrEmpty(textBox2.Text) || string.IsNullOrEmpty(textBox3.Text))
            {
                MessageBox.Show("Lütfen tüm klasör ve dosya yollarını seçin.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string[] selectedPaths = new string[]
            {
                textBox1.Text, // Ana klasör yolu
                textBox2.Text, // Vesikalık klasör yolu
                textBox3.Text  // Veritabanı dosyası yolu
            };

            // Kurulum yollarını kaydet
            string filePath = "kurulum_yollari.txt";
            using (StreamWriter writer = new StreamWriter(filePath))
            {
                foreach (string path in selectedPaths)
                {
                    writer.WriteLine(path);
                }
            }

            MessageBox.Show("Kurulum yolları başarıyla kaydedildi.");

            // Kurulum tamamlandıktan sonra ana forma geç
            OpenForm2();

            // Start_Screen formundan kurtul
            this.Hide(); // Formu gizle
                         // this.Close(); // Formu kapat
        }

        // Start_Screen formundan Form2'yi başlatırken selectedPaths dizisine değerlerin atanması
        private void OpenForm2()
        {
            // Form2'yi başlatırken selectedPaths dizisini oluşturun ve değerlerini atayın
            Form2 form2 = new Form2();
            form2.SelectedPaths = new string[]
            {
                textBox1.Text, // Ana klasör yolu
                textBox2.Text, // Vesikalık klasör yolu
                textBox3.Text  // Veritabanı dosyası yolu
            };
            form2.Show();
        }
    }
}
