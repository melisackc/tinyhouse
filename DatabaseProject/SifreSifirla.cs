using System;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Windows.Forms;

namespace DatabaseProject
{
    public partial class SifreSifirla : Form
    {
        public SifreSifirla()
        {
            InitializeComponent();
        }

        private void SifreSifirla_Load(object sender, EventArgs e)
        {
            // İsterseniz burada form açılırken yapılacak işlemleri yazabilirsiniz.
        }

       
            private void button1_Click(object sender, EventArgs e)
        {
            string email = textBox1.Text.Trim();
            string kullaniciAdi = textBox2.Text.Trim();

            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(kullaniciAdi))
            {
                MessageBox.Show("Lütfen hem e-posta adresinizi hem de kullanıcı adınızı giriniz.", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string connectionString = "Data Source=MELISA\\MSSQLSERVER01;Initial Catalog=Database_project;Integrated Security=True;TrustServerCertificate=True;";
            int kullaniciID = -1;

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                // Email ve kullanıcı adı ile kullaniciID al (tblKimlik ve tblGiris join ile)
                string sql = @"
            SELECT k.kullaniciID 
            FROM tblKimlik ki
            INNER JOIN tblGiris k ON ki.kullaniciID = k.kullaniciID
            WHERE ki.email = @Email AND k.kullaniciAd = @KullaniciAdi";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Email", email);
                    cmd.Parameters.AddWithValue("@KullaniciAdi", kullaniciAdi);

                    object result = cmd.ExecuteScalar();
                    if (result != null)
                    {
                        kullaniciID = Convert.ToInt32(result);
                    }
                    else
                    {
                        MessageBox.Show("Email ve kullanıcı adı eşleşen kayıt bulunamadı.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }
                }

                // Yeni şifre oluştur
                string yeniSifre = SifreUret();
                string hashliSifre = Sifreleme.HashleSHA256(yeniSifre);

                // Şifreyi güncelle
                string updateSql = "UPDATE tblGiris SET Password = @yeniSifre WHERE kullaniciID = @kullaniciID";
                using (SqlCommand updateCmd = new SqlCommand(updateSql, conn))
                {
                    updateCmd.Parameters.AddWithValue("@yeniSifre", hashliSifre);
                    updateCmd.Parameters.AddWithValue("@kullaniciID", kullaniciID);
                    updateCmd.ExecuteNonQuery();
                }

                // Mail gönder
                if (mailGonderme(yeniSifre, email))
                {
                    MessageBox.Show("Yeni şifreniz e-posta adresinize gönderildi.", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
        }
        

        private bool mailGonderme(string yeniSifre, string email)
        {
            try
            {
                string smtpServer = "smtp.gmail.com";
                int smtpPort = 587;
                string smtpUser = "bmbhouses@gmail.com";  // kendi mail adresin
                string smtpPass = "bzfo jssz lnvw vfzi"; // uygulama şifren

                MailMessage mail = new MailMessage();
                SmtpClient smtpClient = new SmtpClient();

                mail.From = new MailAddress(smtpUser);
                mail.To.Add(email);
                mail.Subject = "Yeni Şifreniz";
                mail.Body = $"Kullanabileceğiniz yeni geçici şifreniz: {yeniSifre}\nLütfen şifrenizi kimseyle paylaşmayınız.";

                smtpClient.Host = smtpServer;
                smtpClient.Port = smtpPort;
                smtpClient.Credentials = new NetworkCredential(smtpUser, smtpPass);
                smtpClient.EnableSsl = true;

                smtpClient.Send(mail);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show("E-posta gönderilirken hata oluştu: " + ex.Message);
                return false;
            }
        }

        private string SifreUret()
        {
            string karakterler = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random rnd = new Random();
            return new string(Enumerable.Repeat(karakterler, 8)
                .Select(s => s[rnd.Next(s.Length)]).ToArray());
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            // Email alan textbox değişimi
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //kullanıcı adı alan textbox
        }
    }
}
