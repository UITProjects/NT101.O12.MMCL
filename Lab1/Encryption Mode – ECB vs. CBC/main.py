import tkinter
from tkinter import filedialog
from aes_module import Ecbhelper, Cbchelper
from PIL import Image

window = tkinter.Tk()
window.title("Encypt and decyprt image program")
window.minsize(height=300, width=500)
introduce_label = tkinter.Label(text="Chọn một hình .bmp bất kì để mã hoá", font=("Arial", 24, "bold"))
introduce_label.pack()

ecb_value_IntVar = tkinter.IntVar()
cbc_value_IntVar = tkinter.IntVar()


def check_valid_cbc_checkbox():
    if cbc_value_IntVar.get() == 1:
        cbc_value_IntVar.set(0)


def check_valid_ecb_checkbox():
    if ecb_value_IntVar.get() == 1:
        ecb_value_IntVar.set(0)


ecb_checkbox = tkinter.Checkbutton(text="ECB", variable=ecb_value_IntVar, command=check_valid_cbc_checkbox)
cbc_checkbox = tkinter.Checkbutton(text="CBC", variable=cbc_value_IntVar, command=check_valid_ecb_checkbox)
ecb_checkbox.pack()
cbc_checkbox.pack()

encrypt_value_IntVar = tkinter.IntVar()
decrypt_value_IntVar = tkinter.IntVar()


def check_valid_decrypt_checkbox():
    if decrypt_value_IntVar.get() == 1:
        decrypt_value_IntVar.set(0)


def check_valid_encrypt_checkbox():
    if encrypt_value_IntVar.get() == 1:
        encrypt_value_IntVar.set(0)


encrypt_checkbox = tkinter.Checkbutton(text="Encrypt", variable=encrypt_value_IntVar,
                                       command=check_valid_decrypt_checkbox)
decrypt_checkbox = tkinter.Checkbutton(text="Decrypt", variable=decrypt_value_IntVar,
                                       command=check_valid_encrypt_checkbox)
encrypt_checkbox.pack()
decrypt_checkbox.pack()


def image_choose_button_click():
    initial_dir_str = "./original_image"
    if encrypt_value_IntVar.get() == 1:
        image_file_path_str = filedialog.askopenfilename(initialdir=initial_dir_str)
        with open(image_file_path_str, mode='rb') as image_file_reader:
            header_file_bytes = image_file_reader.read(54)
            raw_data_bytes = image_file_reader.read()
        if ecb_value_IntVar.get() == 1:
            with open("encrypted_image/encrypted_image.bmp", mode='wb') as image_file_writer:
                image_file_writer.write(header_file_bytes)
                image_file_writer.write(Ecbhelper.encrypt(raw_data_bytes))
        elif cbc_value_IntVar.get() == 1:
            with open("encrypted_image/encrypted_image.bmp", mode='wb') as image_file_writer:
                image_file_writer.write(header_file_bytes)
                image_file_writer.write(Cbchelper.encrypt(raw_data_bytes))
        show_image = Image.open("./encrypted_image/encrypted_image.bmp")

    elif decrypt_value_IntVar.get()==1:
        initial_dir_str="./encrypted_image"
        image_file_path_str = filedialog.askopenfilename(initialdir=initial_dir_str)

        with open(image_file_path_str, mode='rb') as image_file_reader:
            header_file_bytes = image_file_reader.read(54)
            raw_data_bytes = image_file_reader.read()
        if ecb_value_IntVar.get() == 1:
            with open("decrypted_image/decrypted_image.bmp", mode='wb') as image_file_writer:
                image_file_writer.write(header_file_bytes)
                image_file_writer.write(Ecbhelper.decrypt(raw_data_bytes))
        elif cbc_value_IntVar.get() == 1:
            with open("decrypted_image/decrypted_image.bmp", mode='wb') as image_file_writer:
                image_file_writer.write(header_file_bytes)
                image_file_writer.write(Cbchelper.decrypt(raw_data_bytes))
        show_image = Image.open("./decrypted_image/decrypted_image.bmp")
    show_image.show()


image_choose_button = tkinter.Button(text="Chọn hình bất kì", command=image_choose_button_click)
image_choose_button.pack()
window.mainloop()
