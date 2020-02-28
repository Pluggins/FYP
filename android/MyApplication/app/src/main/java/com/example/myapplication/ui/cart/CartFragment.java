package com.example.myapplication.ui.cart;

import android.content.DialogInterface;
import android.os.Bundle;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ArrayAdapter;
import android.widget.ListView;
import android.widget.TextView;

import androidx.annotation.Nullable;
import androidx.annotation.NonNull;
import androidx.appcompat.app.AlertDialog;
import androidx.fragment.app.Fragment;
import androidx.lifecycle.Observer;
import androidx.lifecycle.ViewModelProviders;

import com.example.myapplication.R;
import com.example.myapplication.adapter.CartAdapter;
import com.example.myapplication.adapter.MenuAdapter;
import com.example.myapplication.model.CartItem;
import com.example.myapplication.model.Menu;
import com.example.myapplication.service.CartService;
import com.example.myapplication.service.MenuService;
import com.google.android.material.floatingactionbutton.FloatingActionButton;
import com.google.android.material.snackbar.Snackbar;

import java.util.ArrayList;

public class CartFragment extends Fragment {

    private CartViewModel cartViewModel;
    private ListView listView;
    private ArrayAdapter aAdapter;
    private ArrayList<CartItem> cartItems = (ArrayList<CartItem>) CartService.retrieveCartItems();
    private FloatingActionButton fab;
    private DialogInterface.OnClickListener dialogClickListener = null;

    public View onCreateView(@NonNull LayoutInflater inflater,
                             ViewGroup container, Bundle savedInstanceState) {
        cartViewModel =
                ViewModelProviders.of(this).get(CartViewModel.class);
        View root = inflater.inflate(R.layout.fragment_cart, container, false);
        cartViewModel.getText().observe(this, new Observer<String>() {
            @Override
            public void onChanged(@Nullable String s) {
            }
        });

        dialogClickListener = new DialogInterface.OnClickListener() {
            @Override
            public void onClick(DialogInterface dialog, int which) {
                switch (which){
                    case DialogInterface.BUTTON_POSITIVE:
                        //Yes button clicked
                        break;

                    case DialogInterface.BUTTON_NEGATIVE:
                        //No button clicked
                        break;
                }
            }
        };

        fab = root.findViewById(R.id.submitOrderfab);
        fab.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View view) {
                AlertDialog.Builder builder = new AlertDialog.Builder(getContext());
                builder.setMessage("Submit order for processing now?").setPositiveButton("Yes", dialogClickListener)
                        .setNegativeButton("No", dialogClickListener).show();
            }
        });

        aAdapter = new CartAdapter(cartItems, getActivity());
        listView = (ListView) root.findViewById(R.id.cartListView);
        listView.setAdapter(aAdapter);
        return root;
    }
}