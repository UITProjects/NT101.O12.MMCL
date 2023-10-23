import json
import threading
from socket import socket

clients = []


class Client:
    def __init__(self, client_socket: socket):
        self.client_socket = client_socket
        self.username = None
        self.public_key = None
        self.type_message = {
            'register': self.register,
            "get_clients": self.broadcast_public_key,
            "forward": self.forward
        }
        clients.append(self)
        self.thread = threading.Thread(target=self.listen)
        self.thread.start()

    def forward(self, argument: dict):
        recipient_Client: Client
        for client in clients:
            if client.username == argument["to_recipient"]:
                recipient_Client = client
                break
        argument["type"] = "forwarded"
        recipient_Client.send(argument)

    def get_clients(self, argument: dict):
        message_dict = {
            "type": "get_clients",
            "status": "OK",
            "clients": ["akira"]
        }
        for client in clients:
            if client.username != self.username:
                message_dict["clients"].append(client.username)
        print(message_dict)
        self.send(message_dict)

    def broadcast_public_key(self,argument:dict):
        message_dict = {
            "type": "recipients_publickey",
            "clients": {}
        }
        for client in clients:
            message_dict["clients"][client.username] = client.public_key
        self.send(message_dict)

    def listen(self):
        while True:
            header_bytes = self.client_socket.recv(4)
            header_int = int.from_bytes(header_bytes, byteorder='little', signed=True)
            message_bytes = self.client_socket.recv(header_int)
            message_str = message_bytes.decode()
            message_dict = json.loads(message_str)
            self.process(message_dict)

    def send(self, message_dict: dict):
        message_json_str = json.dumps(message_dict)
        message_bytes = message_json_str.encode()
        header_bytes = len(message_bytes).to_bytes(4, byteorder="little")
        self.client_socket.send(header_bytes)
        self.client_socket.send(message_bytes)

    def register(self, arguments: dict):
        self.username = arguments["username"]
        self.public_key = arguments["public_key"]
        message_dict = {
            "type": "register",
            "status": "OK"
        }
        self.send(message_dict)

    def process(self, message_dict: dict):
        print(message_dict)
        self.type_message[message_dict["type"]](message_dict)


clients: [Client] = []
