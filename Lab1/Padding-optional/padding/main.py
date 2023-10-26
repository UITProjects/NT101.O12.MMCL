from aes_module import Cbchelper
import os
def lab_padding(file_size: int):
    if file_size==0:
        return 0
    file_name_str = str(file_size) + "bytes.bin"
    with open("./original/"+file_name_str, mode='wb') as file_writer:
        file_writer.write(os.urandom(file_size))
    with open("./original/"+file_name_str, mode='rb') as file_reader:
        raw_data = file_reader.read()
    print("raw data: ")
    print(raw_data)
    encrypted_data = Cbchelper.encrypt(raw_data)
    encrypted_file_name = "encrypted_{file_size}bytes.bin"
    encrypted_file_name = encrypted_file_name.format(file_size=file_size)
    with open("./encrypted_file/"+encrypted_file_name, mode='wb') as file_writer:
        file_writer.write(encrypted_data)
        print("encrypted file size: " + str(len(encrypted_data)) + " bytes")
        print(encrypted_data)
    Cbchelper.decrypt(encrypted_data)
    return file_size

while lab_padding(int(input("Nhap size bytes: "))):
    print()
    pass


