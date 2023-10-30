import json
import threading
from socket import socket
import cipher_module
from socket import SHUT_RDWR
clients = []


class Client:
    def __init__(self, client_socket: socket,lock:threading.Lock):
        self.machine_name = None
        self.client_socket = client_socket
        self.username = None
        self.public_key_pem_str: str = ""
        self.public_key_der_str: str = ""
        self.encrypted = False

        self.type_message = {
            'register': self.register,
            "get_clients": self.broadcast_public_key,
            "forward": self.forward,
            "exchange_publickey": self.exchange_publickey,
        }
        with lock:
            clients.append(self)
        self.thread = threading.Thread(target=self.listen,args=(lock,))
        self.thread.start()

    def receive(self, length: int) -> bytes:
        actual_data = b""
        byte_read = 0
        while byte_read < length:
            actual_data += self.client_socket.recv(length-byte_read)
            if actual_data == b"":
                return b""
            byte_read = len(actual_data)
        return actual_data

    def forward(self, header_dict: dict, message_bytes: bytes):
        recipient_Client: Client
        for client in clients:
            if client.username == header_dict["to_recipient"]:
                recipient_Client = client
                break
        header_dict["type"] = "forwarded"
        recipient_Client.send(header_dict, message_bytes)

    def broadcast_public_key(self, argument: dict, message_bytes: bytes):
        global clients
        message_dict = {
            "type": "recipients_publickey",
            "clients": {}
        }
        for client in clients:
            if client.username == None:
                continue
            message_dict["clients"][client.username] = client.public_key_der_str
        self.send(message_dict, force_no_encrypt=True)

    def listen(self,lock:threading.Lock):
        global clients
        while True:
            encrypt_bytes = self.receive(1)
            if encrypt_bytes == b"":
                self.client_socket.shutdown(SHUT_RDWR)
                self.client_socket.close()
                with lock:
                    clients_temp = []
                    for client in clients:
                        if client.username == self.username or client.username is None:
                            continue
                        clients_temp.append(client)
                    clients.clear()
                    clients = clients_temp
                if self.username is None:
                    print(self.machine_name + " has close connection")
                else:
                    print(self.username +" has close connection")
                return
            header_length_bytes = self.receive(4)
            header_content_bytes = self.receive(int.from_bytes(header_length_bytes,byteorder="little",signed=True))
            message_length_bytes = self.receive(4)
            message_bytes = self.client_socket.recv(int.from_bytes(message_length_bytes, byteorder="little",signed=True))

            print("header raw data:")
            print(header_content_bytes)
            print()
            print("message raw data:")
            print(message_bytes)
            print()

            if bool.from_bytes(encrypt_bytes, byteorder="little"):
                header_content_bytes = cipher_module.decrypt(header_content_bytes)
            header_content_str = header_content_bytes.decode()
            header_dict = json.loads(header_content_str)

            with lock:
                self.process(header_dict, message_bytes)

    def send(self, header_dict: dict, message_bytes: bytes = "null".encode(), force_no_encrypt: bool = False):
        header_json_str = json.dumps(header_dict)
        header_content_bytes = header_json_str.encode()
        if force_no_encrypt:
            encrypt_bytes = False.to_bytes(1, byteorder="little")
        elif self.encrypted:
            header_content_bytes = cipher_module.encrypt(header_content_bytes, self.public_key_pem_str.encode())
            encrypt_bytes = True.to_bytes(1, byteorder="little")
        else:
            encrypt_bytes = False.to_bytes(1, byteorder="little")

        header_length_bytes = len(header_content_bytes).to_bytes(4, byteorder="little")
        message_length_bytes = len(message_bytes).to_bytes(4, byteorder="little")

        self.client_socket.send(encrypt_bytes)
        self.client_socket.send(header_length_bytes)
        self.client_socket.send(header_content_bytes)
        self.client_socket.send(message_length_bytes)
        self.client_socket.send(message_bytes)

    def register(self, arguments: dict, message_byte):
        self.username = arguments["username"]
        message_dict = {
            "type": "register",
            "status": "OK"
        }
        self.send(message_dict)

    def exchange_publickey(self, arguments: dict, message_bytes: bytes):
        header_dict = {
            "type": "exchange_publickey_response",
            "server_publickey": cipher_module.public_key_der_str,
        }
        self.public_key_pem_str = arguments["client_publickey_pem"]
        self.public_key_der_str = arguments["client_publickey_der"]
        self.machine_name = arguments["machine_name"]
        self.send(header_dict)
        self.encrypted = True

    def process(self, header_dict: dict, message_byte: bytes):
        print("decrypted header:")
        print(header_dict)
        print()
        self.type_message[header_dict["type"]](header_dict, message_byte)
