package com.mnolimp.m;

import com.sun.net.httpserver.HttpServer;

import java.net.InetSocketAddress;

public class Main {

    public static void main(String[] args) throws Exception {
        HttpServer httpServer = HttpServer.create(new InetSocketAddress(8000), 0); {
            httpServer.createContext("/test", new HackatonHandler());
            httpServer.setExecutor(null);
            httpServer.start();
        }
    }
}
