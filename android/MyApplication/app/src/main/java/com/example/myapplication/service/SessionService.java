package com.example.myapplication.service;


import android.os.AsyncTask;
import android.os.NetworkOnMainThreadException;

import org.json.JSONArray;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStreamReader;
import java.net.HttpURLConnection;
import java.net.MalformedURLException;
import java.net.URL;

public class SessionService {
    private static String sessionId;
    private static String sessionKey;

    public static void setTemporarySession(String newSessionId, String newSessionKey) {
        sessionId = newSessionId;
        sessionKey = newSessionKey;
    }

    public static String getSessionId() {
        return sessionId;
    }

    public static String getSessionKey() {
        return sessionKey;
    }
}
