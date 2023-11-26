FROM ubuntu 
RUN apt-get update && apt-get -y install dos2unix git python3 python3-pip
WORKDIR /app
RUN git clone --branch Lab2-bai3 https://github.com/UITProjects/NT101.O12.MMCL.git
RUN dos2unix /app/NT101.O12.MMCL/Lab2/chat/server/*
WORKDIR /app/NT101.O12.MMCL/Lab2/chat/server
RUN pip install -r requirement.txt
EXPOSE 3004
CMD ["python3","-u","main.py"]

