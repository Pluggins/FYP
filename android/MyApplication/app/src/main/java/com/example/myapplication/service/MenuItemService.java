package com.example.myapplication.service;

import com.example.myapplication.model.MenuItem;

import java.lang.reflect.Array;
import java.util.ArrayList;
import java.util.List;

public class MenuItemService {
    private static String menuId;
    private static String menuName;
    private static List<MenuItem> menuItems = new ArrayList<MenuItem>();

    public static void clear() {
        menuItems = new ArrayList<MenuItem>();
    }

    public static void addMenuItem(MenuItem item) {
        menuItems.add(item);
    }

    public static List<MenuItem> retrireveList() {
        return menuItems;
    }

    public static String getMenuName() {
        return menuName;
    }

    public static void setMenuName(String menuName) {
        MenuItemService.menuName = menuName;
    }

    public static String getMenuId() {
        return menuId;
    }

    public static void setMenuId(String menuId) {
        MenuItemService.menuId = menuId;
    }
}
