using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Support.Design.Widget;
using Android.Support.V4.Widget;
using Android.Support.V7.App;
using Android.Support.V7.Widget;
using Android.Views;
using Android.Views.Animations;
using Android.Widget;
using Toolbar = Android.Support.V7.Widget.Toolbar;

namespace App.Droid
{
    [Activity(Theme = "@style/AppTheme.NoActionBar", MainLauncher = true)]
    public class MainActivity : AppCompatActivity, View.IOnClickListener, SwipeRefreshLayout.IOnRefreshListener
    {
        private UsersAdapter _usersAdapter;
        private SwipeRefreshLayout _refreshLayout;

        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.activity_main);

            var toolbar = FindViewById<Toolbar>(Resource.Id.toolbar);
            SetSupportActionBar(toolbar);

            var fab = FindViewById<FloatingActionButton>(Resource.Id.fab);
            fab.SetOnClickListener(this);

            var rv = FindViewById<RecyclerView>(Resource.Id.rv_users);
            rv.SetLayoutManager(new LinearLayoutManager(this));
            rv.SetAdapter(_usersAdapter = new UsersAdapter { Users = GetUsers() });

            _refreshLayout = FindViewById<SwipeRefreshLayout>(Resource.Id.refresh_users);
            _refreshLayout.SetOnRefreshListener(this);
        }

        public override bool OnCreateOptionsMenu(IMenu menu)
        {
            MenuInflater.Inflate(Resource.Menu.menu_main, menu);
            return true;
        }

        public override bool OnOptionsItemSelected(IMenuItem item)
        {
            var id = item.ItemId;
            if (id == Resource.Id.action_settings)
            {
                return true;
            }

            return base.OnOptionsItemSelected(item);
        }

        public void OnClick(View v)
        {
            if (v.Id == Resource.Id.fab)
            {
                Snackbar.Make(v, "Replace with your own action", Snackbar.LengthLong).SetAction("Action", listener: null).Show();
            }
        }

        private static List<User> GetUsers()
        {
            var users = new List<User>();

            for (var i = 0; i < 200; i++)
            {
                var value = new Random().Next(0, 999999);
                users.Add(new User
                {
                    FirstName = $"FirstName{value}",
                    LastName = $"LastName{value}",
                    Email = $"Email@Email{value}"
                });
            }

            return users;
        }

        public void OnRefresh()
        {
            _usersAdapter.Users = GetUsers();
            _usersAdapter.NotifyDataSetChanged();
            _refreshLayout.Refreshing = false;
        }
    }

    internal class UsersAdapter : RecyclerView.Adapter
    {
        public List<User> Users { get; set; }

        public override void OnBindViewHolder(RecyclerView.ViewHolder holder, int position)
        {
            var vh = (UserViewHolder) holder;
            var user = Users[position];

            holder.ItemView.StartAnimation(AnimationUtils.LoadAnimation(holder.ItemView.Context, Resource.Animation.anim_fade_fall));

            vh.User = user;
        }

        public override RecyclerView.ViewHolder OnCreateViewHolder(ViewGroup parent, int viewType)
        {
            return new UserViewHolder(parent);
        }

        public override int ItemCount => Users?.Count ?? 0;
    }

    internal class UserViewHolder : RecyclerView.ViewHolder
    {
        private readonly ImageView _iconView;
        private readonly TextView _labelView;

        private User _user;

        public User User
        {
            get => _user;
            set
            {
                _user = value;
                LoadView();
            }
        }

        public UserViewHolder(ViewGroup parent) : base(LayoutInflater.From(parent.Context).Inflate(Resource.Layout.view_user, parent, false))
        {
            _iconView = ItemView.FindViewById<ImageView>(Resource.Id.iv_icon);
            _labelView = ItemView.FindViewById<TextView>(Resource.Id.tv_label);
        }

        private void LoadView()
        {
            _labelView.Text = $"{_user.FirstName} {_user.LastName}\n{_user.Email}";
        }
    }
}

