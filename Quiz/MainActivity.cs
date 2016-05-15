﻿using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using System.Threading;
using System.Net;
using System.Configuration;

using System.Data;
using MySql.Data.MySqlClient;

namespace Quiz
{
    [Activity(Label = "Quiz", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity
    {
        private Button mBtnRegister;
        private Button mBtnLogin;
        private ProgressBar mProgressBar;


        private OnAddUserEventArgs userArgs;
        private onLoginEventsArgs loginArgs;
        

        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource


            if (has.retrieveset("id_user") != null)
            {
                StartActivity(typeof(MenuActivity));
            }
            else
            {
                SetContentView(Resource.Layout.Main);
                mBtnRegister = FindViewById<Button>(Resource.Id.btnRegister);
                mProgressBar = FindViewById<ProgressBar>(Resource.Id.progressBar1);

                mBtnRegister.Click += (object sender, EventArgs args) =>
                {
                    FragmentTransaction transcation = FragmentManager.BeginTransaction();
                    dialog_Register registerDialog = new dialog_Register();
                    registerDialog.Show(transcation, "dialog fragment");
                    registerDialog.mOnAddUserComplete += RegisterDialog_mOnAddUserComplete;
                };



                mBtnLogin = FindViewById<Button>(Resource.Id.btnLogin);
                mBtnLogin.Click += delegate
                {
                    FragmentTransaction transcation = FragmentManager.BeginTransaction();
                    dialog_Login loginDialog = new dialog_Login();
                    loginDialog.Show(transcation, "dialog fragment");

                    loginDialog.mOnLoginComplete += LoginDialog_mOnLoginComplete;

                };

                
            }
           

        }

      
        private void LoginDialog_mOnLoginComplete(object sender, onLoginEventsArgs e)
        {
            
            if (e.Login.Length < 3) Toast.MakeText(this, "Login musi być dłuższy niż 3 znaki", ToastLength.Short).Show();
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                (sender as DialogFragment).Dismiss();
                loginArgs = e;
                Thread thread = new Thread(checkLogin);
                thread.Start();
            }


        }

        private void RegisterDialog_mOnAddUserComplete(object sender, OnAddUserEventArgs e)
        {
           

            if (e.Login.Length<3) Toast.MakeText(this, "Login musi być dłuższy niż 3 znaki", ToastLength.Short).Show();
            else if(!has.IsValidEmail(e.Email)) Toast.MakeText(this, "Podaj poprawnie adres E-mail", ToastLength.Short).Show();
            else if(e.Password.Length<5) Toast.MakeText(this, "Hasło nie może być krótsze niż 5 znaków", ToastLength.Short).Show();
            else if(e.Password!=e.Password2) Toast.MakeText(this, "Hasła różnią się od siebie", ToastLength.Short).Show();
            else
            {
                mProgressBar.Visibility = ViewStates.Visible;
                (sender as DialogFragment).Dismiss();
                userArgs = e;
                Thread thread = new Thread(CreateAccount);
                thread.Start();
            }
     
        }

        private void CreateAccount()
        {
            Thread.Sleep(1000);
            
            RunOnUiThread(() => {
                MySqlConnection con = new MySqlConnection("Server=mysql8.hekko.net.pl;database=majsterw_quiz;User=majsterw_quiz;Password=fnipSWs4;charset=utf8");
                try
                {
                    if(con.State == ConnectionState.Closed)
                    {
                        con.Open(); //
                        MySqlCommand cmd = new MySqlCommand("SELECT INTO user(login, mail, pass) VALUES (@login, @mail, @pass)", con);
                        cmd.Parameters.AddWithValue("@login", userArgs.Login);
                        cmd.Parameters.AddWithValue("@mail", userArgs.Email);
                        cmd.Parameters.AddWithValue("@pass", has.Hash(userArgs.Password));
                        cmd.ExecuteNonQuery();
                        Toast.MakeText(this, "Witaj "+userArgs.Login, ToastLength.Short).Show();
                    }
                }
                catch(MySqlException ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
                finally
                {
                    con.Close();
                    mProgressBar.Visibility = ViewStates.Invisible;

                }
            });


        }
        private void checkLogin()
        {
            Thread.Sleep(1000);

            RunOnUiThread(() => {
                MySqlConnection con = new MySqlConnection("Server=mysql8.hekko.net.pl;database=majsterw_quiz;User=majsterw_quiz;Password=fnipSWs4;charset=utf8");
                try
                {
                    if (con.State == ConnectionState.Closed)
                    {
                        con.Open(); 
                        MySqlCommand cmd = new MySqlCommand("SELECT id_user, pass FROM user where login = @login", con);  
                        cmd.Parameters.AddWithValue("@login", loginArgs.Login);
                        String idUser = "";
                        String passDb = "";
                        MySqlDataReader Reader = cmd.ExecuteReader();
                        
                        if (Reader.Read())
                        {
                            idUser = Reader[0].ToString();
                            passDb = Reader[1].ToString();
                   
                        }
                        Reader.Close();
                      
                        if (passDb == has.Hash(loginArgs.Password))
                        {   has.saveset("id_user", idUser);
                           
                            Toast.MakeText(this, "Zalogowano", ToastLength.Short).Show();
                            StartActivity(typeof(MenuActivity));
                        }
                        else Toast.MakeText(this, "Zły login lub hasło", ToastLength.Short).Show();

                    }
                }
                catch (MySqlException ex)
                {
                    Toast.MakeText(this, ex.ToString(), ToastLength.Short).Show();
                }
                finally
                {
                    con.Close();
                    mProgressBar.Visibility = ViewStates.Invisible;
                }
            });


        }    
    }
}

