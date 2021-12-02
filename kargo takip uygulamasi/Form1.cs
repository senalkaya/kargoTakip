using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Response;
using FireSharp.Interfaces;




namespace Kargo_Takip
{
    public partial class Form1 : Form
    {

        public Form1()
        {
            InitializeComponent();
            
        }

        IFirebaseConfig firebase_config = new FirebaseConfig()
        {

            AuthSecret = "ZCeGdxew2thaffRuiGClky1JSq1rrnRW6y2jW84P",
            BasePath = "https://kargo-takip-5ae47-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;

        private void Form1_Load(object sender, EventArgs e)
        {
            try
            {

                client = new FireSharp.FirebaseClient(firebase_config);

            }
            catch
            {

                MessageBox.Show("Error");
            }
        }

        

        private void insert_button_Click(object sender, EventArgs e)
        {

             Guid myuuid = Guid.NewGuid();
            string myuuidAsString = myuuid.ToString();


            MusteriBilgileri Musteri_Bilgileri = new MusteriBilgileri()
            {
                Kullanici_Adi = name_Tbox.Text,
                Kullanici_Sifre = sifre_Tbox.Text,
                Kullanici_Ulke = ulke_Tbox.Text,
                Kullanici_Sehir = sehir_Tbox.Text,
                Kullanici_Ilce = ilce_Tbox.Text,
                Kullanici_Mahalle = mah_Tbox.Text,
                

        };

            var setter = client.Set("Musteri_Bilgileri/" + name_Tbox.Text, Musteri_Bilgileri);
            MessageBox.Show("Data Insert Successfuly");
        }

        private void label4_Click(object sender, EventArgs e)
        {

        }

        private void giris_button_Click(object sender, EventArgs e)
        {

            var result = client.Get("Musteri_Bilgileri/" + ad_giris.Text);
            MusteriBilgileri musteri_bilgileri = result.ResultAs<MusteriBilgileri>();
            if (musteri_bilgileri == null)
            {

                MessageBox.Show("Giris Başarısız");
            }
            
           else if (sifre_giris.Text == musteri_bilgileri.Kullanici_Sifre)
            {

                MessageBox.Show("Giris Yapıldı");
                MapForm mapform = new MapForm();
                mapform.Show();            }

            else
            {
                MessageBox.Show("Giris Basarisiz");
            }

        }

        private void update_button_Click(object sender, EventArgs e)
        {

            var result = client.Get("Musteri_Bilgileri/" + ad_giris.Text);
            MusteriBilgileri musteri_bilgileri = result.ResultAs<MusteriBilgileri>();

            if (musteri_bilgileri == null)
            {

                MessageBox.Show("Güncelleme Başarısız");
            }

            else if (ad_giris.Text == musteri_bilgileri.Kullanici_Adi && sifre_giris.Text!="") { 
                MusteriBilgileri Musteri_Bilgileri = new MusteriBilgileri()
                {

                    Kullanici_Adi = musteri_bilgileri.Kullanici_Adi,
                    Kullanici_Sifre = sifre_giris.Text,
                    Kullanici_Ulke = musteri_bilgileri.Kullanici_Ulke,
                    Kullanici_Sehir = musteri_bilgileri.Kullanici_Sehir,
                    Kullanici_Ilce = musteri_bilgileri.Kullanici_Ilce,
                    Kullanici_Mahalle = musteri_bilgileri.Kullanici_Mahalle

                };

            var setter = client.Update("Musteri_Bilgileri/" + ad_giris.Text, Musteri_Bilgileri);
            MessageBox.Show("Şifre Güncellendi");
        }

            else if (ad_giris.Text == "" || sifre_giris.Text=="")
            {
                MessageBox.Show("Boşluk Alanları Doldurun");
            }

        }
    }
}
