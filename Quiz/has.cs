using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace Quiz
{
    class has
    {
        public static string Hash(string input)
        {
            var hash = (new SHA1Managed()).ComputeHash(Encoding.UTF8.GetBytes(input));
            return string.Join("", hash.Select(b => b.ToString("x2")).ToArray());
        }
        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public  static void saveset(String name,String value)
        {

            //store
            var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
            var prefEditor = prefs.Edit();
            prefEditor.PutString(name, value);
            prefEditor.Commit();

        }

        public static String retrieveset(String name)
        {
            //retreive 
            var prefs = Application.Context.GetSharedPreferences("MyApp", FileCreationMode.Private);
            var somePref = prefs.GetString(name, null);
            return somePref;
        }

    }
}