
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FireSharp.Config;
using FireSharp.Interfaces;
using GMap.NET;
using GMap.NET.MapProviders;
using GMap.NET.WindowsForms;
using GMap.NET.WindowsForms.Markers;
using FireSharp.Response;
using FireSharp;
using Newtonsoft.Json;

namespace Kargo_Takip
{
    public partial class MapForm : Form
    {

        

         List<PointLatLng> PointList;
         List<double> MesafeList;

        IFirebaseConfig firebase_config = new FirebaseConfig()
        {

            AuthSecret = "ZCeGdxew2thaffRuiGClky1JSq1rrnRW6y2jW84P",
            BasePath = "https://kargo-takip-5ae47-default-rtdb.firebaseio.com/"
        };
        IFirebaseClient client;



        string a= "a";
        string b= "b";
        int c = 1;


        public MapForm()
        {
            
            InitializeComponent();
            PointList = new List<PointLatLng>();
            MesafeList = new List<double>();
            map.ShowCenter = false;

            try
            {

                client = new FireSharp.FirebaseClient(firebase_config);

            }
            catch
            {

                MessageBox.Show("Error");
            }




            Harita();               

        }




        private void Harita()
        {

            //HARİTA OLUŞTURMA
            
            GMapProviders.GoogleMap.ApiKey = @"AIzaSyDyXuOpWKHqH3Ybjdy9jvrT4zRHPP6reug";
            map.DragButton = MouseButtons.Left;
            map.MapProvider = GMapProviders.GoogleMap;
          
            map.SetPositionByKeywords("Fatih,Istanbul,Turkey");
            map.MinZoom = 5;
            map.MaxZoom = 100;
            map.Zoom = 10;

            //ADRES GETİRME

          
            LiveCall();
            
        }


        private void AddMarker(PointLatLng pointAdd, GMarkerGoogleType marketType = GMarkerGoogleType.blue)
        {

            var markers = new GMapOverlay("markerss");
            var marker = new GMarkerGoogle(pointAdd, marketType);
            markers.Markers.Add(marker);
            map.Overlays.Add(markers);

        }
    
        async void LiveCall()
        {
            while (true)
            {
                
                
                await Task.Delay(1000);
                FirebaseResponse res = await client.GetAsync(@"Musteri_Bilgileri");
                Dictionary<string, MusteriBilgileri> data = JsonConvert.DeserializeObject<Dictionary<string, MusteriBilgileri> > (res.Body.ToString());
                UpdateRTB(data);
                

            }

        }
        int x = 0;
        void UpdateRTB(Dictionary<string,MusteriBilgileri> record)
        {
            
            richTextBox1.Clear();

               

                if (x<record.Count)
                {
                   
                    richTextBox1.Text += record.ElementAt(x).Value.Kullanici_Mahalle + " " + record.ElementAt(x).Value.Kullanici_Ilce + " " + record.ElementAt(x).Value.Kullanici_Sehir + " " + record.ElementAt(x).Value.Kullanici_Ulke + "\n";


                GeoCoderStatusCode statusCode;
                var pointLatLng = GoogleMapProvider.Instance.GetPoint(richTextBox1.Text.Trim(), out statusCode);
                if (statusCode == GeoCoderStatusCode.OK)
                {
                    a = pointLatLng?.Lat.ToString();
                    b = pointLatLng?.Lng.ToString();
                    richTextBox1.Text += a + " " + b;
                    var point2 = new PointLatLng(Convert.ToDouble(a), Convert.ToDouble(b));
                    PointList.Add(point2);
                   


                    if (x < record.Count)
                    {
                        x++;
                        AddMarker(point2);
                    }

                    if (x == record.Count)
                    {
                        MesafeHesap();
                    }

                }

              

            }
            

            
        

        }

        private void MesafeHesap()
        {

            for (int i = 1; i < PointList.Count; i++)
            {
                var route = GoogleMapProvider.Instance.GetRoute(PointList[i-1], PointList[i], false, false, 14);
               
                var r = new GMapRoute(route.Points, "MY Route")
                {
                    Stroke = new Pen(Color.Red, 3)
                };
                var routes = new GMapOverlay("routes");
                routes.Routes.Add(r);
                map.Overlays.Add(routes);


                MesafeList.Add(route.Distance);
                
                
                
            }



        }

    }
}
