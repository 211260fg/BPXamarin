package com.bpxamarin.asyncandroid;

import android.app.Activity;
import android.content.Context;
import android.os.AsyncTask;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

public class MainActivity extends Activity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button btnAsyncTask = findViewById(R.id.btn_async_task);
        btnAsyncTask.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                executeAsyncTask();
            }
        });
    }

    private void executeAsyncTask(){
        Toast.makeText(this, "Started async task", Toast.LENGTH_SHORT).show();
        new WaitTask().execute(this);
    }

    private static class WaitTask extends AsyncTask<Context, Void, Context> {
        @Override
        protected Context doInBackground(Context... params) {
            Context context = params[0];
            try {
                Thread.sleep(3000);
            } catch (InterruptedException e) {
                e.printStackTrace();
            }
            return context;
        }

        @Override
        protected void onPostExecute(Context context) {
            Toast.makeText(context, "Finished async task", Toast.LENGTH_SHORT).show();
        }
    }
}
