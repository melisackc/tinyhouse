using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DatabaseProject
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //Kiracı için oluşturulan buton
            // Kiracı rolü seçildi
            KullaniciRolBilgisi.Rol = "Kiraci";
            Giriş2 girisFormu = new Giriş2();
            girisFormu.Show();
            this.Hide();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            //Ev sahibi için oluşturulan buton
            // Ev Sahibi rolü seçildi
            KullaniciRolBilgisi.Rol = "EvSahibi";
            Giriş2 girisFormu = new Giriş2();
            girisFormu.Show();
            this.Hide();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            //Admin için oluşturulan buton
            // Admin rolü seçildi
            KullaniciRolBilgisi.Rol = "Admin";
            Giriş2 girisFormu = new Giriş2();
            girisFormu.Show();
            this.Hide();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            // kayıt ol butonu
            Form2 kayitOlFormu = new Form2();
            kayitOlFormu.Show(); // veya ShowDialog() kullanabilirsin

            // İstersen Form1'i gizleyebilirsin:
            this.Hide();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }
    }
}
