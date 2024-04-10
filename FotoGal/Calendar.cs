using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util;
using static FotoGal.Calendar;
namespace FotoGal
{
    public partial class Calendar : Form
    {
        private UserCredential credential;
        private CalendarService service;
        private Dictionary<DateTime, Color> dateColorMap = new Dictionary<DateTime, Color>();
        public Calendar()
        {
            InitializeComponent();
            monthCalendar1.DateSelected += MonthCalendar1_DateSelected;
        }
        // Renk paleti tanımla
        private Dictionary<string, Color> colorPalette = new Dictionary<string, Color>
        {
            {"1", Color.FromArgb(255, 255, 255)}, // Beyaz
            {"2", Color.FromArgb(196, 0, 0)},     // Kırmızı
            {"3", Color.FromArgb(255, 187, 0)},   // Turuncu
            {"4", Color.FromArgb(51, 51, 255)},   // Mavi
            // Diğer renkler buraya eklenebilir...
        };
       
        private async void Calendar_Load(object sender, EventArgs e)
        {
            await ConnectToGoogleAccountAsync();
            this.BackColor = ColorTranslator.FromHtml("#4B98AD");
        }
        private async Task ConnectToGoogleAccountAsync()
        {
            try
            {
                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(
                    new ClientSecrets
                    {
                        ClientId = "1040409940651-dq631pn0k3fdmsds9kib2j8qvg721pi3.apps.googleusercontent.com",
                        ClientSecret = "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"
                    },
                    new[] { "https://www.googleapis.com/auth/calendar.readonly" },
                    "user",
                    System.Threading.CancellationToken.None
                );

                service = new CalendarService(new BaseClientService.Initializer()
                {
                    HttpClientInitializer = credential,
                    ApplicationName = "Google Calendar Integration"
                });

                // Takvim verilerini al ve formdaki takvimde işaretle
                await MarkCalendarEventsAsync();
            }
            catch (Google.GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Yetkilendirme hatası: Lütfen doğru Google hesabına erişim izni verdiğinizden emin olun.");
                }
                else
                {
                    MessageBox.Show($"Google hesabına bağlanırken bir hata oluştu: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Google hesabına bağlanırken bir hata oluştu: {ex.Message}");
            }
        }

        private async Task LoadCalendarEventsAsync()
        {
            // Takvim verilerini al
            Events events = await service.Events.List("primary").ExecuteAsync();

            // Takvim verilerini formda göster
            foreach (var eventItem in events.Items)
            {
                listBox1.Items.Add($"{eventItem.Summary} - {eventItem.Start.DateTime}");
            }
        }
        /*
        private void InitializeConnectButton()
        {
            connectButton = new Button();
            connectButton.Text = "Connect to Google Account";
            connectButton.Dock = DockStyle.Top;
            connectButton.Click += ConnectButton_Click;

            // Eğer Google hesabına bağlandıysa butonu gizle
            if (credential != null && credential.Token.IsExpired(SystemClock.Default))
            {
                connectButton.Visible = false;
            }

            Controls.Add(connectButton);
        }*/

        private void AddBoldedDateWithColor(DateTime date, Color color)
        {
            monthCalendar1.AddBoldedDate(date);
            monthCalendar1.UpdateBoldedDates();

            dateColorMap[date] = color;
        }
        private async Task MarkCalendarEventsAsync()
        {
            try
            {
                // Takvim verilerini al
                Events events = await service.Events.List("primary").ExecuteAsync();

                // Takvimdeki etkinlikleri işaretle
                foreach (var eventItem in events.Items)
                {
                    if (eventItem.Start.DateTime != null)
                    {
                        DateTime eventDate = eventItem.Start.DateTime.GetValueOrDefault();
                        string colorId = eventItem.ColorId ?? "1"; // Varsayılan renk beyaz
                        Color eventColor = colorPalette.ContainsKey(colorId) ? colorPalette[colorId] : Color.White;

                        BoldedDateWithColor boldedDateWithColor = new BoldedDateWithColor(eventDate.Date, eventColor);
                        monthCalendar1.AddBoldedDateWithColor(boldedDateWithColor);
                    }
                }

                monthCalendar1.UpdateBoldedDates();
            }
            catch (Google.GoogleApiException ex)
            {
                if (ex.HttpStatusCode == System.Net.HttpStatusCode.Unauthorized)
                {
                    MessageBox.Show("Yetkilendirme hatası: Lütfen doğru Google hesabına erişim izni verdiğinizden emin olun.");
                }
                else
                {
                    MessageBox.Show($"Takvim verileri yüklenirken bir hata oluştu: {ex.Message}");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Takvim verileri yüklenirken bir hata oluştu: {ex.Message}");
            }
        }

        private async void MonthCalendar1_DateSelected(object sender, DateRangeEventArgs e)
        {
            try
            {
                listBox1.Items.Clear();
                DateTime selectedDate = e.Start;
                Events events = await service.Events.List("primary").ExecuteAsync();

                foreach (var eventItem in events.Items)
                {
                    if (eventItem.Start.DateTime != null)
                    {
                        DateTime eventDate = eventItem.Start.DateTime.GetValueOrDefault();
                        if (eventDate.Date == selectedDate.Date)
                        {
                            listBox1.Items.Add($"{eventItem.Summary} - {eventItem.Start.DateTime}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Etkinlikler yüklenirken bir hata oluştu: {ex.Message}");
            }

        }
        public class BoldedDateWithColor
    {
        public DateTime Date { get; set; }
        public Color Color { get; set; }

        public BoldedDateWithColor(DateTime date, Color color)
        {
            Date = date;
            Color = color;
        }
    }

   
        private async void ConnectButton_Click(object sender, EventArgs e)
        {
            await ConnectToGoogleAccountAsync();
        }

        private void monthCalendar1_DateChanged(object sender, DateRangeEventArgs e)
        {

        }

        private void btn_kopar_Click(object sender, EventArgs e)
        {
            credential?.RevokeTokenAsync(CancellationToken.None).Wait();
            credential = null;
            service = null;

            // Bağlantıyı kopardıktan sonra takvimi temizleme veya diğer işlemleri yapabilirsiniz.
            monthCalendar1.RemoveAllBoldedDates();
            listBox1.Items.Clear();

            MessageBox.Show("Google bağlantısı başarıyla kapatıldı.");

        }
    }
    public static class MonthCalendarExtensions
    {
        public static void AddBoldedDateWithColor(this MonthCalendar monthCalendar, BoldedDateWithColor boldedDateWithColor)
        {
            monthCalendar.AddBoldedDate(boldedDateWithColor.Date);
            monthCalendar.UpdateBoldedDates();
            monthCalendar.BackColor = boldedDateWithColor.Color; // Örnek olarak renk uygulanması
        }
    }
}
