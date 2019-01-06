package com.bpxamarin.filesandroid;

import android.app.Activity;
import android.os.Bundle;
import android.os.Environment;
import android.view.View;
import android.widget.Button;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileOutputStream;
import java.io.FileReader;
import java.io.IOException;
import java.io.OutputStream;

public class MainActivity extends Activity {

    private static final String fileName = "androidfile.txt";
    private static final byte[] data = String.valueOf(System.currentTimeMillis()).getBytes();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main);

        Button btnCreate = findViewById(R.id.btn_create);
        Button btnRead = findViewById(R.id.btn_read);
        Button btnDelete = findViewById(R.id.btn_delete);

        btnCreate.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                create(fileName, data);
            }
        });

        btnRead.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                read(fileName);
            }
        });

        btnDelete.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                delete(fileName);
            }
        });
    }

    private void create(String fileName, byte[] data) {
        String path = Environment.getExternalStorageDirectory() + File.separator + fileName;
        File file = new File(path);
        try {
            file.createNewFile();
            if (file.exists()) {
                OutputStream fo = new FileOutputStream(file);
                fo.write(data);
                fo.close();
                Toast.makeText(this, "File created at " + path, Toast.LENGTH_SHORT).show();
            } else {
                Toast.makeText(this, "Failed to create file", Toast.LENGTH_SHORT).show();
            }
        } catch (IOException e) {
            e.printStackTrace();
        }
    }

    private void read(String fileName) {
        String path = Environment.getExternalStorageDirectory() + File.separator + fileName;
        File file = new File(path);
        if (file.exists()) {
            StringBuilder text = new StringBuilder();
            BufferedReader br;
            try {
                br = new BufferedReader(new FileReader(file));
                String line;
                while ((line = br.readLine()) != null) {
                    if (!text.toString().equals(""))
                        text.append('\n');
                    text.append(line);
                }
                br.close();
            } catch (IOException e) {
                e.printStackTrace();
            }
            Toast.makeText(this, text.toString(), Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(this, "File does not exist", Toast.LENGTH_SHORT).show();
        }
    }

    private void delete(String fileName) {
        String path = Environment.getExternalStorageDirectory() + File.separator + fileName;
        File file = new File(path);
        if (file.exists()) {
            file.delete();
            Toast.makeText(this, "File deleted at "+path, Toast.LENGTH_SHORT).show();
        } else {
            Toast.makeText(this, "File does not exist", Toast.LENGTH_SHORT).show();
        }
    }
}
