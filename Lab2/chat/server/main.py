import json
import socket
import threading

from server_core import Client,clients

server = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
server.bind(("0.0.0.0", 3004))
server.listen()
print("Server is listening")
lock = threading.Lock()
while True:
    new_client_socket, address = server.accept()
    print("connect from ")
    print(address)
    clients.append(Client(new_client_socket,lock))

