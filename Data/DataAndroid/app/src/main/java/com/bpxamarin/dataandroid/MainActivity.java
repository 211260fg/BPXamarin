package com.bpxamarin.dataandroid;

import android.app.Activity;
import android.content.ContentValues;
import android.content.Context;
import android.database.Cursor;
import android.database.sqlite.SQLiteDatabase;
import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.TextView;

import java.nio.file.Path;
import java.nio.file.Paths;
import java.util.ArrayList;
import java.util.List;
import java.util.Random;

public class MainActivity extends Activity {

    private TextView dataView;
    private SQLiteDatabase db;

    private static final String TABLE_USER = "User";
    private static final String KEY_USER_ID = "Id";
    private static final String KEY_USER_FIRSTNAME = "FirstName";
    private static final String KEY_USER_LASTNAME = "LastName";

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        dataView = findViewById(R.id.tv_data);

        Button btnInsert = findViewById(R.id.btn_insert);
        Button btnDelete = findViewById(R.id.btn_delete);
        Button btnRead = findViewById(R.id.btn_read);

        btnInsert.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                insertUser();
            }
        });

        btnDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                deleteAll();
            }
        });

        btnRead.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                readUsers();
            }
        });

        db = initDatabase(this);
        readUsers();
    }

    private static SQLiteDatabase initDatabase(Context context) {
        SQLiteDatabase db = context.openOrCreateDatabase("dataandroid.db3", MODE_PRIVATE, null);
        db.execSQL("CREATE TABLE IF NOT EXISTS " + TABLE_USER + " (" +
                KEY_USER_ID + " INTEGER PRIMARY KEY AUTOINCREMENT, " +
                KEY_USER_FIRSTNAME + " VARCHAR(50), " +
                KEY_USER_LASTNAME + " VARCHAR(50));");
        return db;
    }

    private void insertUser() {
        int value = new Random().nextInt(999999);
        User user = new User("FirstName" + value, "LastName" + value);
        ContentValues content = new ContentValues();
        content.put(KEY_USER_FIRSTNAME, user.getFirstName());
        content.put(KEY_USER_LASTNAME, user.getLastName());
        db.insert(TABLE_USER, null, content);
    }

    private void deleteAll() {
        db.execSQL("DELETE FROM " + TABLE_USER);
    }

    private void readUsers() {
        List<User> users = new ArrayList<>();
        Cursor cursor = db.rawQuery("SELECT * FROM " + TABLE_USER, null);
        if (cursor.moveToFirst()) {
            do {
                User user = new User();
                user.setId(Integer.parseInt(cursor.getString(0)));
                user.setFirstName(cursor.getString(1));
                user.setLastName(cursor.getString(2));
                users.add(user);
            } while (cursor.moveToNext());
        }
        cursor.close();

        StringBuilder text = new StringBuilder();

        for (User user : users) {
            text.append("Id: ").append(user.getId()).append(", FirstName: ").append(user.getFirstName()).append(", LastName: ").append(user.getLastName()).append("\n");
        }

        dataView.setText(text.toString());
    }
}
