package com.bpxamarin.uiandroid;

import android.os.Bundle;
import android.support.annotation.NonNull;
import android.support.design.widget.FloatingActionButton;
import android.support.design.widget.Snackbar;
import android.support.v4.widget.SwipeRefreshLayout;
import android.support.v7.app.AppCompatActivity;
import android.support.v7.widget.LinearLayoutManager;
import android.support.v7.widget.RecyclerView;
import android.support.v7.widget.Toolbar;
import android.view.LayoutInflater;
import android.view.View;
import android.view.Menu;
import android.view.MenuItem;
import android.view.ViewGroup;
import android.view.animation.AnimationUtils;
import android.widget.ImageView;
import android.widget.TextView;

import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class MainActivity extends AppCompatActivity implements SwipeRefreshLayout.OnRefreshListener {

    private UsersAdapter usersAdapter;
    private SwipeRefreshLayout refreshLayout;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);
        Toolbar toolbar = findViewById(R.id.toolbar);
        setSupportActionBar(toolbar);

        FloatingActionButton fab = findViewById(R.id.fab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                Snackbar.make(view, "Replace with your own action", Snackbar.LENGTH_LONG)
                        .setAction("Action", null).show();
            }
        });

        RecyclerView rv = findViewById(R.id.rv_users);
        rv.setLayoutManager(new LinearLayoutManager(this));
        rv.setAdapter(usersAdapter = new UsersAdapter());
        usersAdapter.setUsers(getUsers());

        refreshLayout = findViewById(R.id.refresh_users);
        refreshLayout.setOnRefreshListener(this);
    }

    @Override
    public boolean onCreateOptionsMenu(Menu menu) {
        getMenuInflater().inflate(R.menu.menu_main, menu);
        return true;
    }

    @Override
    public boolean onOptionsItemSelected(MenuItem item) {
        int id = item.getItemId();

        if (id == R.id.action_settings) {
            return true;
        }

        return super.onOptionsItemSelected(item);
    }


    private static List<User> getUsers()
    {
        List<User> users = new ArrayList<>();

        for (int i = 0; i < 200; i++)
        {
            int value = new Random().nextInt(999999);
            User user = new User();
            user.setFirstName("FirstName"+value);
            user.setFirstName("LastName"+value);
            user.setFirstName("Email@Email"+value);
            users.add(user);
        }

        return users;
    }

    @Override
    public void onRefresh() {
        usersAdapter.setUsers(getUsers());
        usersAdapter.notifyDataSetChanged();
        refreshLayout.setRefreshing(false);
    }

    public class UsersAdapter extends RecyclerView.Adapter<UserViewHolder>{

        private List<User> users;

        public void setUsers(List<User> users){
            this.users = users;
        }

        @NonNull
        @Override
        public UserViewHolder onCreateViewHolder(@NonNull ViewGroup viewGroup, int i) {
            return new UserViewHolder(viewGroup);
        }

        @Override
        public void onBindViewHolder(@NonNull UserViewHolder viewHolder, int i) {
            viewHolder.itemView.startAnimation(AnimationUtils.loadAnimation(viewHolder.itemView.getContext(), R.anim.anim_fade_fall));
            User user = users.get(i);
            viewHolder.setUser(user);
        }

        @Override
        public int getItemCount() {
            return users != null ? users.size() : 0;
        }
    }

    private class UserViewHolder extends RecyclerView.ViewHolder{

        private ImageView iconView;
        private TextView labelView;

        private User user;

        UserViewHolder(@NonNull ViewGroup parent) {
            super(LayoutInflater.from(parent.getContext()).inflate(R.layout.view_user, parent, false));
            iconView = itemView.findViewById(R.id.iv_icon);
            labelView = itemView.findViewById(R.id.tv_label);
        }

        public void setUser(User user){
            this.user = user;
            loadView();
        }

        private void loadView(){
            labelView.setText(user.getFirstName() + " " + user.getLastName() + "\n" + user.getEmail());
        }
    }
}
