package com.example.myapplication;

import androidx.appcompat.app.AppCompatActivity;
import androidx.core.content.ContextCompat;

import android.graphics.PorterDuff;
import android.graphics.PorterDuffColorFilter;
import android.graphics.drawable.Drawable;
import android.os.Bundle;
import android.view.View;
import android.view.Window;
import android.widget.Button;
import android.widget.EditText;
import android.widget.ImageButton;
import android.widget.TextView;

import com.example.myapplication.service.MenuItemService;

public class AddItem extends AppCompatActivity {
    EditText itemQuantity;
    ImageButton addBtn;
    ImageButton minusBtn;
    TextView title;
    Button backBtn;
    Button submitBtn;

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.activity_add_item);
        title = (TextView) findViewById(R.id.itemTitle);
        itemQuantity = (EditText) findViewById(R.id.itemQuantity);
        addBtn = (ImageButton) findViewById(R.id.itemAddBtn);
        minusBtn = (ImageButton) findViewById(R.id.itemMinusBtn);
        backBtn = (Button) findViewById(R.id.addBackBtn);
        submitBtn = (Button) findViewById(R.id.addItemBtn);
        title.setText(MenuItemService.getSelectedMenuItemName());

        addBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                int amount = Integer.parseInt(itemQuantity.getText().toString());
                itemQuantity.setText(String.valueOf(++amount));
            }
        });

        minusBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                int amount = Integer.parseInt(itemQuantity.getText().toString());
                itemQuantity.setText(String.valueOf(--amount));
            }
        });

        backBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                finish();
            }
        });
    }
}
