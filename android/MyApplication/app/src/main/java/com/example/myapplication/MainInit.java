package com.example.myapplication;

import androidx.appcompat.app.AppCompatActivity;

import android.content.Intent;
import android.os.AsyncTask;
import android.os.Bundle;
import android.os.NetworkOnMainThreadException;
import android.se.omapi.Session;
import android.view.View;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.ListView;

import com.example.myapplication.model.Menu;
import com.example.myapplication.service.MenuService;
import com.example.myapplication.service.SessionService;

import org.json.JSONArray;
import org.json.JSONException;
import org.json.JSONObject;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.io.OutputStreamWriter;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;
import java.util.ArrayList;
import java.util.List;

public class MainInit extends AppCompatActivity {

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_init);

        setTitle("Welcome");
        final Button button = (Button) findViewById(R.id.button2);
        button.setOnClickListener(new View.OnClickListener() {
            public void onClick(View v) {
                LoadSession load = new LoadSession();
                load.execute();
            }
        });
    }

    private class LoadSession extends AsyncTask<String, Void, String> {
        @Override
        protected String doInBackground(String[] params) {
            // HTTPPOST to API
            String response = null;
            URL url = null;
            try {
                url = new URL("https://fyp.amazecraft.net/Api/User/CreateTempUserSession");
            } catch (MalformedURLException e) {
                e.printStackTrace();
            }

            try {
                HttpURLConnection con = (HttpURLConnection) url.openConnection();
                con.setRequestMethod("POST");
                con.setRequestProperty("Content-Type", "application/json");
                con.setRequestProperty("Accept", "application/json");
                try(BufferedReader br = new BufferedReader(new InputStreamReader(con.getInputStream(), "utf-8"))) {
                    StringBuilder sb = new StringBuilder();
                    String responseLine = null;
                    while ((responseLine = br.readLine()) != null) {
                        sb.append(responseLine.trim());
                    }
                    response = sb.toString();
                }
            } catch (IOException e) {
                e.printStackTrace();
            } catch (NetworkOnMainThreadException e) {
                e.printStackTrace();
            }
            return response;
        }

        @Override
        protected void onPostExecute(String message) {
            try {
                JSONObject obj = new JSONObject(message);
                SessionService.setTemporarySession(obj.getString("sessionId"), obj.getString("sessionKey"));
            } catch (JSONException e) {
                e.printStackTrace();
            }

            LoadMenu loadMenu = new LoadMenu();
            loadMenu.execute();
        }
    }

    private class LoadMenu extends AsyncTask<String, Void, String>  {
        @Override
        protected String doInBackground(String[] params) {
            // HTTPPOST to API
            String response = null;
            URL url = null;
            try {
                url = new URL("https://fyp.amazecraft.net/Api/Menu/RetrieveListByVendorId");
            } catch (MalformedURLException e) {
                e.printStackTrace();
            }

            try {
                HttpURLConnection con = (HttpURLConnection) url.openConnection();
                con.setRequestMethod("POST");
                con.setRequestProperty("Content-Type", "application/json");
                con.setRequestProperty("Accept", "application/json");
                con.setDoInput(true);

                JSONObject json = new JSONObject();
                json.put("VendorId", "e916642b-d464-476f-920d-43462d0110b3");

                OutputStreamWriter wr = new OutputStreamWriter(con.getOutputStream());
                wr.write(json.toString());
                wr.flush();

                try(BufferedReader br = new BufferedReader(new InputStreamReader(con.getInputStream(), "utf-8"))) {
                    StringBuilder sb = new StringBuilder();
                    String responseLine = null;
                    while ((responseLine = br.readLine()) != null) {
                        sb.append(responseLine.trim());
                    }
                    response = sb.toString();
                }
            } catch (IOException e) {
                e.printStackTrace();
            } catch (NetworkOnMainThreadException e) {
                e.printStackTrace();
            } catch (JSONException e) {
                e.printStackTrace();
            }
            return response;
        }

        @Override
        protected void onPostExecute(String message) {
            try {
                JSONObject obj = new JSONObject(message);
                JSONArray jArray = obj.getJSONArray("menuList");
                for (int i = 0; i < jArray.length() ; i++) {
                    JSONObject tmpObj = jArray.getJSONObject(i);
                    Menu newMenu = new Menu();
                    newMenu.setId(tmpObj.getString("id"));
                    newMenu.setName(tmpObj.getString("name"));
                    MenuService.addList(newMenu);
                }
                obj.get("menuList");
            } catch (JSONException e) {
                e.printStackTrace();
            }

            Intent intent = new Intent(MainInit.this, MainActivity.class);
            startActivity(intent);
            finish();
        }
    }
}
