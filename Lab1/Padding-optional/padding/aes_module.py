from Crypto.Cipher import AES
import base64
from Crypto.Util.Padding import pad
from Crypto.Util.Padding import unpad
import os


class Cbchelper:
    key_bytes = base64.b64decode(os.getenv("symmetric_key"))

    e_cipher = AES.new(key_bytes, AES.MODE_CBC)
    d_cipher = AES.new(key_bytes, AES.MODE_CBC, e_cipher.iv)

    @staticmethod
    def encrypt(plaintext_bytes: bytes) -> bytes:
            padding_plaintext_bytes = pad(plaintext_bytes, 16)
            encrypted_plaintext_bytes = Cbchelper.e_cipher.encrypt(padding_plaintext_bytes)
            return encrypted_plaintext_bytes

    @staticmethod
    def decrypt(ciphertext_bytes: bytes):
            plaintext_bytes = Cbchelper.d_cipher.decrypt(ciphertext_bytes)
            print("decrypted data before padding")
            print(plaintext_bytes)
            print("Decrypted data after remove padding")
            plaintext_unpadding_bytes = unpad(plaintext_bytes, 16)
            print(plaintext_unpadding_bytes)
