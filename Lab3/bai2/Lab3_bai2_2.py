import hashlib

def calculate_sha1(file_path):
    sha1_hash = hashlib.sha1()

    with open(file_path, 'rb') as file:

        while chunk := file.read(8192):
            sha1_hash.update(chunk)
            
    return sha1_hash.hexdigest()


pdf_file_path_shattered_1 = "shattered-1.pdf"
pdf_file_path_shattered_2 = "shattered-2.pdf"

sha1_value_shattered_1 = calculate_sha1(pdf_file_path_shattered_1)
sha1_value_shattered_2 = calculate_sha1(pdf_file_path_shattered_2)

print(f"SHA-1 của tệp PDF shattered_1: {sha1_value_shattered_1}")
print(f"SHA-1 của tệp PDF shattered_2: {sha1_value_shattered_2}")
