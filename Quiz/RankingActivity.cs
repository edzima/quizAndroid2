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
using Newtonsoft.Json;

namespace Quiz
{
    [Activity(Label = "RankingActivity")]
    public class RankingActivity : Activity
    {
        private List<String> rankName;
        private ListView listrank;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ranking);
            listrank = FindViewById<ListView>(Resource.Id.listrank);

            listrank.ItemClick += (object sender, Android.Widget.AdapterView.ItemClickEventArgs e) =>
            {
                //pobranie nazwy kategorii i przejscie do quizu
                string selectedFromList = listrank.GetItemAtPosition(e.Position).ToString();

            };

            var webclient = new WebClient();
            webclient.DownloadStringCompleted += webClient_DownloadStringCompleted;
            webclient.DownloadStringAsync(new Uri(string.Format("http://www.robocza.h2g.pl/quiz/ranking.php?id_user=1")));

        }
        void webClient_DownloadStringCompleted(object sender, DownloadStringCompletedEventArgs e)
        {
            // Parse to Json String
            if (!string.IsNullOrEmpty(e.Result))
            {

                rankName = new List<String>();
                String[] ranking = e.Result.Trim().Split('\n');


                foreach (string login in ranking)
                {
                    rankName.Add(login);
                }
                ArrayAdapter<String> adapter = new ArrayAdapter<String>(this, Android.Resource.Layout.SimpleListItem1, rankName);
                listrank.Adapter = adapter;

            }
        }
    }
}