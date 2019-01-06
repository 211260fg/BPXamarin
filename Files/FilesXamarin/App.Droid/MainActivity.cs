using System;
using System.Text;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using Environment = Android.OS.Environment;

namespace App.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity, View.IOnClickListener
    {
        private static string JavaFileName => "javafile.txt";
        private static string NetFileName => "netfile.txt";
        private static byte[] Data => Encoding.ASCII.GetBytes(DateTimeOffset.Now.ToUnixTimeMilliseconds().ToString());

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var btnCreateJava = FindViewById(Resource.Id.btn_create_java);
            var btnReadJava = FindViewById(Resource.Id.btn_read_java);
            var btnDeleteJava = FindViewById(Resource.Id.btn_delete_java);
            var btnCreateNet = FindViewById(Resource.Id.btn_create_net);
            var btnReadNet = FindViewById(Resource.Id.btn_read_net);
            var btnDeleteNet = FindViewById(Resource.Id.btn_delete_net);

            btnCreateJava.SetOnClickListener(this);
            btnReadJava.SetOnClickListener(this);
            btnDeleteJava.SetOnClickListener(this);
            btnCreateNet.SetOnClickListener(this);
            btnReadNet.SetOnClickListener(this);
            btnDeleteNet.SetOnClickListener(this);
        }

        private void CreateFileJava(string fileName, byte[] data)
        {
            var path = Environment.ExternalStorageDirectory + Java.IO.File.Separator + fileName;
            var file = new Java.IO.File(path);
            file.CreateNewFile();
            if (file.Exists())
            {
                Java.IO.OutputStream fo = new Java.IO.FileOutputStream(file);
                fo.Write(data);
                fo.Close();
                Toast.MakeText(this, $"File created at {path}", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Failed to create file", ToastLength.Short).Show();
            }
        }

        private void ReadFileJava(string fileName)
        {
            var path = Environment.ExternalStorageDirectory + Java.IO.File.Separator + fileName;
            var file = new Java.IO.File(path);
            if (file.Exists())
            {
                var text = "";
                var br = new Java.IO.BufferedReader(new Java.IO.FileReader(file));
                string line;
                while ((line = br.ReadLine()) != null)
                {
                    if(!text.Equals(""))
                        text += '\n';
                    text += line;
                }
                br.Close();
                Toast.MakeText(this, text, ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "File does not exist", ToastLength.Short).Show();
            }
        }

        private void DeleteFileJava(string fileName)
        {
            var path = Environment.ExternalStorageDirectory + Java.IO.File.Separator + fileName;
            var file = new Java.IO.File(path);
            if (file.Exists())
            {
                file.Delete();
                Toast.MakeText(this, $"File deleted at {path}", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "File does not exist", ToastLength.Short).Show();
            }
        }

        private void CreateFileNet(string fileName, byte[] data)
        {
            var path = System.IO.Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, fileName);
            using (System.IO.File.Create(path)) { }
            if (System.IO.File.Exists(path))
            {
                System.IO.File.WriteAllBytes(path, data);
                Toast.MakeText(this, $"File created at {path}", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "Failed to create file", ToastLength.Short).Show();
            }
        }

        private void ReadFileNet(string fileName)
        {
            var path = System.IO.Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, fileName);
            if (System.IO.File.Exists(path))
            {
                var text = System.IO.File.ReadAllText(path);
                Toast.MakeText(this, text, ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "File does not exist", ToastLength.Short).Show();
            }
        }

        private void DeleteFileNet(string fileName)
        {
            var path = System.IO.Path.Combine(Environment.ExternalStorageDirectory.AbsolutePath, fileName);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
                Toast.MakeText(this, $"File deleted at {path}", ToastLength.Short).Show();
            }
            else
            {
                Toast.MakeText(this, "File does not exist", ToastLength.Short).Show();
            }
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btn_create_java:
                    CreateFileJava(JavaFileName, Data);
                    break;
                case Resource.Id.btn_read_java:
                    ReadFileJava(JavaFileName);
                    break;
                case Resource.Id.btn_delete_java:
                    DeleteFileJava(JavaFileName);
                    break;
                case Resource.Id.btn_create_net:
                    CreateFileNet(NetFileName, Data);
                    break;
                case Resource.Id.btn_read_net:
                    ReadFileNet(NetFileName);
                    break;
                case Resource.Id.btn_delete_net:
                    DeleteFileNet(NetFileName);
                    break;
            }
        }
    }
}

