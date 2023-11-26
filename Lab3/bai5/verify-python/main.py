import binascii
from cryptography import x509
import os
from cryptography.hazmat.primitives.hashes import SHA256
from cryptography.hazmat.primitives.asymmetric.padding import PKCS1v15
import subprocess
for root, dirs, files in os.walk(os.getcwd()):
    for file in files:
        if file.endswith(".cer") or file.endswith(".signature") or file.endswith(".body"):
            os.remove(os.path.join(root, file))

hostname = input("Hostname: ")
full_url = f"https://{hostname}"
command = [os.getcwd() + "\\getcert-C#.exe", full_url]

result = subprocess.run(command)

cer_chain = [element for element in os.listdir(os.getcwd()) if element.__contains__(".cer")]

for i in range(0, len(cer_chain) - 1):
    print(f"certification: {i}")
    with open(f"./{i}.cer", mode="rb") as reader:
        body = b""
        signature = b""
        reader.seek(4)
        body += reader.read(4)
        body_length = int.from_bytes(body[2:4], byteorder="big", signed=False)
        body += reader.read(body_length)
        reader.seek(4 + body_length + 24)
        signature += reader.read()
        print("Body content")
        print(binascii.hexlify(body))
        print("Signature")
        print(binascii.hexlify(signature))
    with open(f"./{i}.body", mode="wb") as writer:
        writer.write(body)
    with open(f"./{i}.signature", mode="wb") as writer:
        writer.write(signature)

    with open(f"./{i + 1}.cer", mode="rb") as reader:
        previous_x509 = x509.load_der_x509_certificate(reader.read())
        previous_x509.public_key().verify(signature, body, algorithm=SHA256(),
                                          padding=PKCS1v15())
        print(f"public key of {i + 1} verify Ok")
        print("\n\n")
        # print(f"public key of {i + 1} verify failed")
