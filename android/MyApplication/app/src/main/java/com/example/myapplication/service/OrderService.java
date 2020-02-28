package com.example.myapplication.service;

import com.example.myapplication.model.MenuItem;

import java.util.ArrayList;
import java.util.List;

public class OrderService {
    private static String orderId = null;
    private static boolean initiatedOrder = false;

    public static boolean isInitiatedOrder() {
        return initiatedOrder;
    }

    public static void setInitiatedOrder(boolean initiatedOrder) {
        OrderService.initiatedOrder = initiatedOrder;
    }

    public static String getOrderId() {
        return orderId;
    }

    public static void setOrderId(String orderId) {
        OrderService.orderId = orderId;
    }
}
