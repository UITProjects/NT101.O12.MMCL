import json
import threading
from socket import socket
import cipher_module

clients = []


class Client:
    def __init__(self, client_socket: socket):
        self.client_socket = client_socket
        self.username = None
        self.public_key: str = ""
        self.encrypted = False

        self.type_message = {
            'register': self.register,
            "get_clients": self.broadcast_public_key,
            "forward": self.forward,
            "exchange_publickey": self.exchange_publickey,
            "session_key": self.session_key
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

    def broadcast_public_key(self, argument: dict):
        global clients
        message_dict = {
            "type": "recipients_publickey",
            "clients": {}
        }
        for client in clients:
            if client.username == None:
                continue
            message_dict["clients"][client.username] = client.public_key
        self.send(message_dict)

    def listen(self):
        global clients
        encrypt_bytes: bytes
        header_length_bytes: bytes
        header_content_bytes: bytes
        message_bytes: bytes
        message_length_bytes: bytes
        while True:
            try:
                encrypt_bytes = self.client_socket.recv(1)
                header_length_bytes = self.client_socket.recv(4)
                header_content_bytes = self.client_socket.recv(int.from_bytes(header_length_bytes, byteorder="little"))
                message_length_bytes = self.client_socket.recv(4)
                message_bytes = self.client_socket.recv(int.from_bytes(message_length_bytes, byteorder="little"))
            except ConnectionError:
                clients_temp = []
                for client in clients:
                    if client.username == self.username:
                        continue
                    clients_temp.append(client)
                clients.clear()
                clients = clients_temp
                print(self.username + " has close connection")
                return

            if bool.from_bytes(encrypt_bytes, byteorder="little"):
                message_bytes = cipher_module.decrypt(message_bytes)
                header_content_bytes = cipher_module.decrypt(header_content_bytes)
            print(header_content_bytes)
            message_str = header_content_bytes.decode()
            try:
                message_dict = json.loads(message_str)
            except json.decoder.JSONDecodeError:
                print(self.username + " has close connection")
                clients_temp = []
                for client in clients:
                    if client.username == self.username:
                        continue
                    clients_temp.append(client)
                clients.clear()
                clients = clients_temp
                return
            self.process(message_dict)

    def send(self, header_dict: dict, message_str: str = "null", encrypt: bool = False):

        message_bytes = message_str.encode()

        header_json_str = json.dumps(header_dict)
        header_content_bytes = header_json_str.encode()
        if self.encrypted:
            header_content_bytes = cipher_module.encrypt(header_content_bytes, self.public_key.encode())
            message_bytes = cipher_module.encrypt(message_bytes, self.public_key.encode())

        encrypt_bytes = bool.to_bytes(encrypt, byteorder="little")
        print(encrypt_bytes)
        header_length_bytes = len(header_content_bytes).to_bytes(4, byteorder="little")
        message_length_bytes = len(message_bytes).to_bytes(4, byteorder="little")
        self.client_socket.send(encrypt_bytes)
        self.client_socket.send(header_length_bytes)
        self.client_socket.send(header_content_bytes)
        self.client_socket.send(message_length_bytes)
        self.client_socket.send(message_bytes)
    def register(self, arguments: dict):
        self.username = arguments["username"]
        message_dict = {
            "type": "register",
            "status": "OK"
        }
        self.send(message_dict)

    def exchange_publickey(self, argument: dict):
        message_dict = {
            "type": "exchange_publickey_response",
            "server_publickey": cipher_module.public_key_der_str,
        }
        self.public_key = argument["client_publickey"]
        self.send(message_dict)
        self.encrypted = True

    def session_key(self, argument: dict):
        pass

    def process(self, message_dict: dict):
        if message_dict["type"] != "get_clients":
            print(message_dict)
        self.type_message[message_dict["type"]](message_dict)
