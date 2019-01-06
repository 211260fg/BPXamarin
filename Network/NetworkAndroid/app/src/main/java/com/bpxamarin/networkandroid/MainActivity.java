package com.bpxamarin.networkandroid;

import android.app.Activity;
import android.os.Bundle;
import android.os.StrictMode;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import com.google.gson.Gson;

import java.io.BufferedInputStream;
import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

public class MainActivity extends Activity {

    private static final String url = "https://raw.githubusercontent.com/211260fg/BPXamarin/master/README.md";
    private static final String jsonData =
        "{ " +
            "'firstname' : 'Florian', " +
            "'lastname' : 'Goeteyn', " +
            "'email' : 'florian_goeteyn@outlook.com', " +
            "'balance' : 12.5 " +
        "}";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        //Allow network calls on the main thread
        StrictMode.setThreadPolicy(new StrictMode.ThreadPolicy.Builder().permitAll().build());

        Button btnGet = findViewById(R.id.btn_get);
        Button btnConvertJson = findViewById(R.id.btn_convertjson);

        btnGet.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                getTextFromUrlJava(url);
            }
        });

        btnConvertJson.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                convertJson(jsonData);
            }
        });
    }

    private void getTextFromUrlJava(String url) {
        HttpURLConnection urlConnection = null;
        StringBuilder result = new StringBuilder();
        try {
            URL connectionUrl = new URL(url);
            urlConnection = (HttpURLConnection) connectionUrl.openConnection();

            int code = urlConnection.getResponseCode();

            if (code == 200) {
                InputStream stream = new BufferedInputStream(urlConnection.getInputStream());
                BufferedReader bufferedReader = new BufferedReader(new InputStreamReader(stream));
                String line;

                while ((line = bufferedReader.readLine()) != null)
                    result.append(line);
                stream.close();
            }
            Toast.makeText(this, result.toString(), Toast.LENGTH_SHORT).show();
        } catch (IOException e) {
            e.printStackTrace();
        } finally {
            if (urlConnection != null)
                urlConnection.disconnect();
        }
    }

    private void convertJson(String json){
        User user = new Gson().fromJson(json, User.class);

        String text =
                "First name: "+user.getFirstName()+"\n"+
                "Last name"+user.getLastName()+"\n"+
                "Email: "+user.getEmail()+"\n"+
                "Balance: "+user.getBalance();

        Toast.makeText(this, text, Toast.LENGTH_SHORT).show();
    }
}
