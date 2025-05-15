using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.Mail;


namespace DatabaseProject
{
    public partial class Giriş2 : Form
    {
        public Giriş2()
        {
            InitializeComponent();
        }

        private void Form3_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            //kulanıcı adını alan textbox
        }

        private void textBox2_TextChanged(object sender, EventArgs e)
        {
            //şifreyi alan textbox
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Giriş yap butonu
            string sifre = textBox2.Text;
            string kullaniciAdi = textBox1.Text;

            // Boş alan kontrolü
            if (string.IsNullOrWhiteSpace(kullaniciAdi) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi doldurun.");
                return;
            }

            if (kullaniciAdi.Length < 6 || kullaniciAdi.Length > 15)
            {
                MessageBox.Show("Kullanıcı adı 6 ile 15 karakter arasında olmalıdır.");
                return;
            }

            if (sifre.Length < 6 || sifre.Length > 10)
            {
                MessageBox.Show("Şifre 6 ile 10 karakter arasında olmalıdır.");
                return;
            }

            if (string.IsNullOrWhiteSpace(textBox2.Text) || string.IsNullOrWhiteSpace(sifre))
            {
                MessageBox.Show("Lütfen kullanıcı adı ve şifreyi doldurun.");
                return;
            }
          

            string connectionString = "Data Source=MELISA\\MSSQLSERVER01;Initial Catalog=Database_project;Integrated Security=True;TrustServerCertificate=True;";

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                conn.Open();

                string hashliSifre = Sifreleme.HashleSHA256(sifre); // Şifreleme varsa

                string sql = "SELECT rol FROM tblGiris WHERE kullaniciAd = @kullaniciAdi AND Password = @sifre";

                using (SqlCommand cmd = new SqlCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@kullaniciAdi", kullaniciAdi);
                    cmd.Parameters.AddWithValue("@sifre", hashliSifre);

                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        string rol = result.ToString();

                        // Rol bazlı yönlendirme
                        switch (rol.ToLower())
                        {
                            case "kiracı":
                                Form5 kiraciForm = new Form5();
                                kiraciForm.Show();
                                break;
                            case "ev sahibi":
                                Form6 evSahibiForm = new Form6();
                                evSahibiForm.Show();
                                break;
                            case "admin":
                                Form7 adminForm = new Form7();
                                adminForm.Show();
                                break;
                            default:
                                MessageBox.Show("Rol tanımlanamıyor. Lütfen yöneticinizle iletişime geçin.");
                                return;
                        }

                        this.Hide(); // Giriş formunu gizle
                    }
                    else
                    {
                        MessageBox.Show("Kullanıcı adı veya şifre yanlış.", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
    

        

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            //şifremi unuttum link
            SifreSifirla sifreFormu = new SifreSifirla();
            sifreFormu.Show();
        }
    }
}
