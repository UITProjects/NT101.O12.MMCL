
from Crypto.Cipher import AES
import base64
from os import getenv

key_str = getenv('symmetric_key')
key_bytes = base64.b64decode(key_str)
from Crypto.Util.Padding import  pad,unpad
cipher = AES.new(key_bytes, mode=AES.MODE_ECB)

def encrypt(plaintext_bytes: bytes):

    return cipher.encrypt(pad(plaintext_bytes,16))


def decrypt(ciphertext_bytes: bytes):
    return unpad(cipher.decrypt(ciphertext_bytes),16)


with open('./file.txt',mode='r',encoding='utf8') as reader:
    content_str = reader.read()
    raw_data_bytes = content_str.encode()
    print('Nội dung file trước khi encrypted: ' + content_str)
    print('Raw data')
    print (raw_data_bytes)
with open('./file.txt',mode='rb') as reader:
    raw_data_bytes = reader.read()
    encrypted_raw_data = encrypt(raw_data_bytes)
with open('./encrypted_file.txt',mode='wb') as writer:
    writer.write(encrypted_raw_data)
with open('./encrypted_file.txt',mode='rb') as reader:
    print('encrypted_raw_data')
    print(reader.read())

with open('./encrypted_file.txt',mode='rb') as reader:
    print('Nội dung sau khi decrypt: '+decrypt(reader.read()).decode())
input('bấm phím bất kì để thoát')
