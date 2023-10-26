from Crypto.Cipher import AES
import os
import base64

key_str = os.getenv('symmetric_key')
key_bytes = base64.b64decode(key_str)
cipher_e = AES.new(key_bytes, AES.MODE_CFB)
cipher_d = AES.new(key_bytes, AES.MODE_CFB, iv=cipher_e.iv)


def encrypt(plaintext_bytes: bytes):
    return cipher_e.encrypt(plaintext_bytes)


def decrypt(ciphertext_bytes: bytes):
    return cipher_d.decrypt(ciphertext_bytes)


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

# plaintext_str = input('Nhap thong tin: ')
# plaintext_bytes = plaintext_str.encode()
# print('plaintext_bytes: ')
# print(plaintext_bytes)
#
# encrypted_message_bytes = encrypt(plaintext_bytes)
# print('encrypted_message_bytes')
# print(encrypted_message_bytes)
# print('decrypted_message_bytes')
# decrypt_message_bytes = decrypt(encrypted_message_bytes)
# print(decrypt_message_bytes)
# print('message after decode: ' + decrypt_message_bytes.decode())
