using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseProject
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {   //Kayıt ol butonu 


            // Tüm alanların dolu olup olmadığını kontrol et
            if (string.IsNullOrWhiteSpace(textBox1.Text) ||  // İsim
                string.IsNullOrWhiteSpace(textBox2.Text) ||  // Soyisim
                string.IsNullOrWhiteSpace(textBox3.Text) ||  // Kullanıcı adı
                string.IsNullOrWhiteSpace(textBox4.Text) ||  // Şifre
                string.IsNullOrWhiteSpace(textBox5.Text) ||  // Şifre tekrar
                string.IsNullOrWhiteSpace(textBox6.Text) ||  // E-mail
                string.IsNullOrWhiteSpace(textBox7.Text) ||  // Adres
                (!radioButton1.Checked && !radioButton2.Checked) || // Cinsiyet
                comboBox1.SelectedIndex == -1 ||            // ComboBox (Rol)
                dateTimePicker1.Value == dateTimePicker1.MinDate) // Doğum tarihi boş kontrolü
            {
                MessageBox.Show("Lütfen tüm alanları doldurunuz!", "Eksik Bilgi", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            
             // Kullanıcı adı uzunluğu kontrolü
            if (textBox3.Text.Length < 6 || textBox3.Text.Length > 15)
            {
                MessageBox.Show("Kullanıcı ad 6 ile 15 karakter arasında olmalıdır!", "Geçersiz İsim", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }


            // Şifre uzunluğu ve içeriği kontrolü
            string sifre = textBox4.Text;
            string sifreTekrar = textBox5.Text;
            if (sifre.Length < 6 || sifre.Length > 10)
            {
                MessageBox.Show("Şifre 6 ile 10 karakter arasında olmalıdır!", "Geçersiz Şifre", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (!System.Text.RegularExpressions.Regex.IsMatch(sifre, @"^[a-zA-Z0-9]+$"))
            {
                MessageBox.Show("Şifre sadece harf ve rakamlardan oluşmalıdır!", "Geçersiz Karakter", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            // Şifre eşleşmesi
            if (sifre != sifreTekrar)
            {
                MessageBox.Show("Şifreler uyuşmuyor!", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }


            // VERİTABANI BAĞLANTISI
            string userName = textBox1.Text.Trim();         // İsim
            string userSurname = textBox2.Text.Trim();      // Soyisim
            string kullaniciAd = textBox3.Text.Trim();      // Kullanıcı adı
            string password = textBox4.Text;  
            password = Sifreleme.HashleSHA256(textBox4.Text); // Şifreyi hashle                                                                    
            string email = textBox6.Text.Trim();            // Email
            string gender = radioButton1.Checked ? "Kadın" : "Erkek"; // Cinsiyet
            String rol = comboBox1.SelectedItem.ToString(); // Rol
            DateTime dogumTarihi = dateTimePicker1.Value; // Doğum tarihi
            DateTime value = dogumTarihi;
            String adres = textBox7.Text.Trim(); // Adres


            string connectionString = "Data Source=MELISA\\MSSQLSERVER01;Initial Catalog=Database_project;Integrated Security=True;TrustServerCertificate=True;";


            try
            {
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // tblkimlik'e kayıt
                    string queryKimlik = "INSERT INTO tblkimlik (userName, userSurname, gender,dogumTarihi) VALUES (@userName, @userSurname, @gender,@dogumTarihi)";
                    using (SqlCommand cmd = new SqlCommand(queryKimlik, connection))
                    {
                        cmd.Parameters.AddWithValue("@userName", userName);
                        cmd.Parameters.AddWithValue("@userSurname", userSurname);
                        cmd.Parameters.AddWithValue("@gender", gender);
                        cmd.Parameters.AddWithValue("@dogumTarihi", dogumTarihi);
                        cmd.ExecuteNonQuery();
                    }

                    // tblgiris'e kayıt
                    string queryGiris = "INSERT INTO tblgiris (kullaniciAd, Password,rol) VALUES (@kullaniciAd, @Password,@rol)";
                    using (SqlCommand cmd = new SqlCommand(queryGiris, connection))
                    {
                        cmd.Parameters.AddWithValue("@kullaniciAd", kullaniciAd);
                        cmd.Parameters.AddWithValue("@Password", password);
                        cmd.Parameters.AddWithValue("@rol", rol);                       
                        cmd.ExecuteNonQuery();
                    }

                    // tbliletisim'e kayıt
                    string queryIletisim = "INSERT INTO tbliletisim (email,adres) VALUES (@Email,@adres)";
                    using (SqlCommand cmd = new SqlCommand(queryIletisim, connection))
                    {
                        cmd.Parameters.AddWithValue("@Email", email);
                        cmd.Parameters.AddWithValue("@adres", adres);
                        cmd.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Kayıt başarıyla tamamlandı!", "Başarılı", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Veritabanı hatası: " + ex.Message, "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }






        }





    }
}
