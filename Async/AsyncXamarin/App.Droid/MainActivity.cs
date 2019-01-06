using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace App.Droid
{
    [Activity(MainLauncher = true)]
    public class MainActivity : Activity, View.IOnClickListener
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var btnAsyncTask = FindViewById(Resource.Id.btn_async_task);
            var btnAsyncMethod = FindViewById(Resource.Id.btn_async_method);

            btnAsyncTask.SetOnClickListener(this);
            btnAsyncMethod.SetOnClickListener(this);
        }

        private async void ExecuteAsyncMethod()
        {
            Toast.MakeText(this, "Started async method", ToastLength.Short).Show();
            await Task.Run(() =>
            {
                System.Threading.Thread.Sleep(3000);
            });
            Toast.MakeText(this, "Finished async method", ToastLength.Short).Show();
        }

        private void ExecuteAsyncTask()
        {
            Toast.MakeText(this, "Started async task", ToastLength.Short).Show();
            new WaitTask().Execute(this);
        }

        private class WaitTask : AsyncTask {
            protected override Java.Lang.Object DoInBackground(params Java.Lang.Object[] @params)
            {
                var context = @params[0];
                Java.Lang.Thread.Sleep(3000);
                return context;
            }

            protected override void OnPostExecute(Java.Lang.Object result)
            {
                Toast.MakeText((Context)result, "Finished async task", ToastLength.Short).Show();
            }
        }

        public void OnClick(View v)
        {
            switch (v.Id)
            {
                case Resource.Id.btn_async_task:
                    ExecuteAsyncTask();
                    break;
                case Resource.Id.btn_async_method:
                    ExecuteAsyncMethod();
                    break;
            }
        }
    }
}

