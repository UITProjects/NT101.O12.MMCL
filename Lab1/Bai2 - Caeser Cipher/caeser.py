def encrypt(plaintext, shift):
  ciphertext=""
  for i in range(len(plaintext)):
    char = plaintext[i]
    if ( 32<=ord(char)<=64) or (91<=ord(char)<=96) or (123<=ord(char)<=126):
      ciphertext += char
    elif (char.isupper()):
      ciphertext += chr((ord(char) + shift - 65) % 26 + 65)
    else:
      ciphertext += chr((ord(char) + shift - 97) % 26 + 97)
  return ciphertext

def decrypt(ciphertext, shift):
  plaintext=""
  for i in range(len(ciphertext)):
    char = ciphertext[i]
    if ( 32<=ord(char)<=64) or (91<=ord(char)<=96) or (123<=ord(char)<=126):
      plaintext += char
    elif (char.isupper()):
      plaintext += chr((ord(char) - shift - 65) % 26 + 65)
    else:
      plaintext += chr((ord(char) - shift - 97) % 26 + 97)
  return plaintext
      
def decryptBrute_force(plaintext):
  for shift in range(26):
    ciphertext=""
    for i in range(len(plaintext)):
      char = plaintext[i]
      if ( 32 <= ord(char) <= 64) or ( 91 <= ord(char) <= 96) or ( 123 <= ord(char) <=126):
        ciphertext += char
      elif (char.isupper()):
        ciphertext += chr((ord(char) - shift - 65) % 26 + 65)
      else:
        ciphertext += chr((ord(char) - shift - 97) % 26 + 97)
    print("Shift "+ str(shift) + ": " + ciphertext + "\n")

options = input("Enter (1) to encrypt or (2) to decrypt: ")

if options == '1':
  plaintext = input("Enter plaintext: ")
  shift = int(input("Enter shift: "))
  print("Ciphertext: " + encrypt(plaintext,shift))
else :
  suboption = input("Choose (1) to enter shift or (2) to brute-force all option: ")
  if suboption == '1':
    ciphertext = input("Enter ciphertext: ")
    shift = int(input("Enter shift: "))
    print("Plaintext: " + decrypt(ciphertext,shift))
  else :
    ciphertext = input("Enter ciphertext: ")
    print("Plaintext: ")
    decryptBrute_force(ciphertext)