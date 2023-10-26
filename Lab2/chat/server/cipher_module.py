import rsa
import base64

public_key, private_key = rsa.newkeys(512)
public_key_der_str: str = base64.b64encode(public_key.save_pkcs1(format="DER")).decode()
print(public_key_der_str)


def encrypt(plaintext_bytes: bytes, client_publickey: bytes = None):
    if client_publickey is None:
        return rsa.encrypt(plaintext_bytes, public_key)
    else:
        return rsa.encrypt(plaintext_bytes, rsa.PublicKey.load_pkcs1(client_publickey))


def decrypt(ciphertext: bytes):
    return rsa.decrypt(ciphertext, private_key)
