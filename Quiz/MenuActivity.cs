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
using MySql.Data.MySqlClient;
using System.Data;

namespace Quiz
{
    [Activity(Label = "MenuActivity")]
    public class MenuActivity : Activity
    {
        private Button mBtnLogOut;
        private Button mBtnSelectCategory;
        private Button mBtnToAddCategory;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            RequestWindowFeature(WindowFeatures.NoTitle);
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.menu);

            mBtnToAddCategory = FindViewById<Button>(Resource.Id.btnToAddCategory);
            mBtnLogOut = FindViewById<Button>(Resource.Id.btnLogOut);
            mBtnSelectCategory = FindViewById<Button>(Resource.Id.btnSelectCategory);


            mBtnLogOut.Click += delegate
            {
                has.saveset("id_user", null);
                Finish();
                StartActivity(typeof(MainActivity));

            };

            mBtnSelectCategory.Click += delegate
            {

                StartActivity(typeof(selectCategoryActivity));
            };

            mBtnToAddCategory.Click += delegate
            {
                FragmentTransaction transcation = FragmentManager.BeginTransaction();
                dialogCategory dialogCat = new dialogCategory();
                dialogCat.Show(transcation, "dialog fragment");
                dialogCat.mOnAddCategoryCompleted += DialogCat_mOnAddCategoryCompleted;

            };


        }

            private void DialogCat_mOnAddCategoryCompleted(object sender, onAddCategoryEventArgs e)
        {
            DialogFragment dialogAddCategory = sender as DialogFragment; // do zamkniecia

            if (e.Name.Length > 3)
            {
                MySqlConnection con = new MySqlConnection("Server=mysql8.hekko.net.pl;database=majsterw_quiz;User=majsterw_quiz;Password=fnipSWs4;charset=utf8");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open();
                        MySqlCommand cmd = new MySqlCommand("SELECT name FROM categories where name = @name", con);
                        cmd.Parameters.AddWithValue("@name", e.Name);
                        MySqlDataReader Reader = cmd.ExecuteReader();

                        if (Reader.Read())
                        {
                            Toast.MakeText(this, "Podana kateogria ju¿ instnieje!", ToastLength.Long).Show();
                        }
                        else
                        {
                            Reader.Close();
                            cmd.CommandText = "INSERT INTO categories(name) VALUES(@name)";
                            cmd.ExecuteNonQuery();
                            Toast.MakeText(this, "Pomyślnie dodano nową kategorię!", ToastLength.Long).Show();
                            dialogAddCategory.Dismiss();
                        }
                        if (!Reader.IsClosed) Reader.Close();
                    }
                }
                catch (MySqlException ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
                finally
                {
                    con.Close();
                }
            }
            else
            {
                Toast.MakeText(this, "Minimalna długość nazwy to 3 znaki", ToastLength.Short).Show();
            }
        }



    }
    
}