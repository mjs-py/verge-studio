// simple websocket server
// broadcasts messages to all clients

const express = require('express');
const WebSocket = require('ws');
const SocketServer = require('ws').Server;

const PORT = process.env.PORT || 3001;
const server = express().listen(PORT, () => console.log(`Listening on ${ PORT }`));
const wss = new SocketServer({ server });

wss.on('connection', (ws) => {

    // log when client connects
    console.log('Client connected');

    // log when client disconnects
    ws.on('close', () => console.log('Client disconnected'));

    ws.on('message', function incoming(message) {
        console.log('[Server] Received message: %s', message);
        // broadcast to every other client
        wss.clients.forEach(function each(client) {
            if (client !== ws && client.readyState === WebSocket.OPEN) {
                client.send(message);
            }
        })
    })
});