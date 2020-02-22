package com.example.myapplication;

import androidx.appcompat.app.AppCompatActivity;

import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.ListView;

import com.example.myapplication.ui.home.menu.MenuViewModel;

public class Main2Activity extends AppCompatActivity {
    private ListView listView;
    private ArrayAdapter aAdapter;
    private String[] users = { "Suresh Dasari", "Rohini Alavala", "Trishika Dasari", "Praveen Alavala", "Madav Sai", "Hamsika Yemineni"};

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_main2);
        aAdapter = new ArrayAdapter<String>(this, R.layout.listrow_menu, users);
        listView = (ListView) findViewById(R.id.menuListView);
        listView.setAdapter(aAdapter);
    }
}
