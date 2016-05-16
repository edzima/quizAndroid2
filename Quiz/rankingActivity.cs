using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using System.Net;
using Newtonsoft.Json.Linq;

namespace Quiz
{
    [Activity(Label = "rankingActivity")]
    public class rankingActivity : Activity
    {
        private List<String> rankList;
        private ListView listvieww;
        
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ranking);
            listvieww = FindViewById<ListView>(Resource.Id.listView1);

            var webclient = new WebClient();
            webclient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            webclient.DownloadStringAsync(new Uri(string.Format("http://www.robocza.h2g.pl/quiz/ranking.php?id_user=1")));
        }

        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            String response = e.Result;
            // Parse to Json String
            if (!string.IsNullOrEmpty(response))
            {
                JArray rankingJson = JArray.Parse(response);


                rankList = new List<String>();


               for(int i =0; i< rankingJson.Count; i++)
                {

                    rankList.Add(i+1+". "+rankingJson[i]["login"]+ " Œredni czas: "+ rankingJson[i]["Sredni_Czas"] + " Iloœæ pkt: "+ rankingJson[i]["Poprawne_Odpowiedzi"] );
                }
                ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, rankList);
                listvieww.Adapter = adapter;
                

            }
        }
        public class Ranking
        {
    
            public string login { get; set; }
   
            public string Sredni_Czas { get; set; }
            public string Poprawne_Odpowiedzi { get; set; }
        }
    }
}