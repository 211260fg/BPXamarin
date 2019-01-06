using System;
using System.IO;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;
using SQLite;

namespace App.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity, View.IOnClickListener
    {
        private TextView _dataView;

        private SQLiteConnection _db;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            _dataView = FindViewById<TextView>(Resource.Id.tv_data);

            var btnInsert = FindViewById(Resource.Id.btn_insert);
            var btnDelete = FindViewById(Resource.Id.btn_delete);
            var btnRead = FindViewById(Resource.Id.btn_read);

            btnInsert.SetOnClickListener(this);
            btnDelete.SetOnClickListener(this);
            btnRead.SetOnClickListener(this);

            _db = InitDatabase();
            ReadUsers();
        }

        private static SQLiteConnection InitDatabase()
        {
            var path = Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), "dataxamarin.db3");
            var db = new SQLiteConnection(path);
            db.CreateTable<User>();
            return db;
        }

        private void InsertUser()
        {
            var value = new Random().Next(0, 999999);
            _db.Insert(new User
            {
                FirstName = $"FirstName{value}",
                LastName = $"LastName{value}",
                Email = $"Email@Email{value}"
            });
        }

        private void DeleteAll()
        {
            _db.DeleteAll<User>();
        }

        private void ReadUsers()
        {
            var users = _db.Table<User>().ToList();
            _dataView.Text = "";
            if (users == null)
                return;
            foreach (var user in users)
            {
                _dataView.Text += $"Id: {user.Id}, FirstName: {user.FirstName}, LastName: {user.LastName}\n";
            }
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btn_insert:
                    InsertUser();
                    break;
                case Resource.Id.btn_delete:
                    DeleteAll();
                    break;
                case Resource.Id.btn_read:
                    ReadUsers();
                    break;
            }
        }
    }
}

