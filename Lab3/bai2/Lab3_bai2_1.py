import binascii
import hashlib
hexMessage1 = "d131dd02c5e6eec4693d9a0698aff95c2fcab58712467eab4004583eb8fb7f8955ad340609f4b30283e488832571415a085125e8f7cdc99fd91dbdf280373c5bd8823e3156348f5bae6dacd436c919c6dd53e2b487da03fd02396306d248cda0e99f33420f577ee8ce54b67080a80d1ec69821bcb6a8839396f9652b6ff72a70"
hexMessage2 = "d131dd02c5e6eec4693d9a0698aff95c2fcab50712467eab4004583eb8fb7f8955ad340609f4b30283e4888325f1415a085125e8f7cdc99fd91dbd7280373c5bd8823e3156348f5bae6dacd436c919c6dd53e23487da03fd02396306d248cda0e99f33420f577ee8ce54b67080280d1ec69821bcb6a8839396f965ab6ff72a70"

dif_char_hex1 = []
dif_char_hex2 = []

for i, (char1, char2) in enumerate(zip(hexMessage1, hexMessage2)):
    if char1 != char2:
        dif_char_hex1.append(char1)
        dif_char_hex2.append(char2)

print("Những kí tự khác nhau ứng với từng thông điệp Hex:")
print(dif_char_hex1)
print(dif_char_hex2)


for i in range(len(dif_char_hex1)):
    dif_char_hex1[i] = "{0:04b}".format(int(dif_char_hex1[i], 16)) 
    dif_char_hex2[i] = "{0:04b}".format(int(dif_char_hex2[i], 16))

print("\nĐổi kí tự Hex thành binary:")
print(dif_char_hex1)
print(dif_char_hex2)


number_of_dif = 0
for i in range(len(dif_char_hex1)):
  for i, (char1, char2) in enumerate(zip(dif_char_hex1[i], dif_char_hex2[i])):
    if char1 != char2:
      number_of_dif += 1
      
print("Số lượng bit khác nhau:",number_of_dif,"\n Số lượng Byte khác nhau:",number_of_dif/8)

byte_data_1 = bytes.fromhex(hexMessage1)
byte_data_2 = bytes.fromhex(hexMessage2)

md5_hash_1 = hashlib.md5(byte_data_1).hexdigest()
md5_hash_2 = hashlib.md5(byte_data_2).hexdigest()

print("MD5 hash của message 1:", md5_hash_1)
print("MD5 hash của message 2:", md5_hash_2)
