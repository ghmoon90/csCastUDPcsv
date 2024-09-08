# csUDPTxCsv
csharp based standalone win32 app proj</br>
import a csv file and transmit each row one by one through UDP of designated IP and port.</br>
developed with visual studio community, .NET 8.0 toolchain. </br>
These code is made with ChatGPT.


## csUDPSendCsv

usage : csUDPSendCsv.exe {data.csv} {dst ip address} {dst port}

it transfers data.csv file through UDP </br>
data.csv should contain byte columns </br>
destination IP address is like 127.0.0.1 </br>
dst port is like 5299
The input csv file should separated by comma.

## csCastSendCsv

usage : csCastUDPCsv.exe {data.csv} {dst ip address} {dst port}

it transfers data.csv file through UDP </br>
data.csv should contain data columns </br>
each column data type is defined in CastType.xml </br>
destination IP address is like 127.0.0.1 </br>
dst port is like 5299
The input csv file should separated by comma.

## pyUDPSend
usage : python pyUDPSend.py {data.csv} {host ip} {host port}

same purpose as csCastSendCsv.exe </br>

difference is that csCastSendCsv.exe uses castType.xml, but it utilizes castType.json format for variable type setting

## pyUDPhost
usage : python pyUDPhost.py {host ip} {host port}




