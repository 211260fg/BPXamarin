using System.IO;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Java.IO;
using Java.Net;
using Newtonsoft.Json;

namespace App.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity, View.IOnClickListener
    {
        private static string Url => "https://raw.githubusercontent.com/211260fg/BPXamarin/master/README.md";
        private static string JsonData => 
            @"
                {
                    'firstname' : 'Florian',
                    'lastname': 'Goeteyn',
                    'email' : 'florian_goeteyn@outlook.com',
                    'balance' : 12.5
                }
            ";

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);
            
            //Allow network calls on the main thread
            StrictMode.SetThreadPolicy(new StrictMode.ThreadPolicy.Builder().PermitAll().Build());

            var btnGetJava = FindViewById(Resource.Id.btn_get_java);
            var btnGetNet = FindViewById(Resource.Id.btn_get_net);
            var btnConvertJson = FindViewById(Resource.Id.btn_convertjson);

            btnGetJava.SetOnClickListener(this);
            btnGetNet.SetOnClickListener(this);
            btnConvertJson.SetOnClickListener(this);
        }

        private void GetTextFromUrlNet(string url)
        {
            string result;
            using (var client = new System.Net.WebClient())
            {
                result = client.DownloadString(url);
            }
            Toast.MakeText(this, result, ToastLength.Short).Show();
        }

        private void GetTextFromUrlJava(string url)
        {
            HttpURLConnection urlConnection = null;
            var result = "";
            try
            {
                var connectionUrl = new URL(url);
                urlConnection = (HttpURLConnection)connectionUrl.OpenConnection();

                var code = urlConnection.ResponseCode;

                if (code == HttpStatus.Ok)
                {
                    var stream = new BufferedStream(urlConnection.InputStream);
                    var bufferedReader = new BufferedReader(new InputStreamReader(stream));
                    string line;

                    while ((line = bufferedReader.ReadLine()) != null)
                        result += line;
                    stream.Close();
                }

                Toast.MakeText(this, result, ToastLength.Short).Show();
            }
            finally
            {
                urlConnection?.Disconnect();
            }
        }

        private void ConvertJson(string json)
        {
            var user = JsonConvert.DeserializeObject<User>(json);

            var text = $@"
                          First name: {user.FirstName}
                          Last name: {user.LastName}
                          Email: {user.Email}
                          Balance: {user.Balance}
                        ";

            Toast.MakeText(this, text, ToastLength.Short).Show();
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btn_get_java:
                    GetTextFromUrlJava(Url);
                    break;
                case Resource.Id.btn_get_net:
                    GetTextFromUrlNet(Url);
                    break;
                case Resource.Id.btn_convertjson:
                    ConvertJson(JsonData);
                    break;
            }
        }
    }
}

