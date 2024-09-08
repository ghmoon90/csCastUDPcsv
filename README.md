# csUDPTxCsv
csharp based standalone win32 app proj
import a csv file and transmit each row one by one through UDP of designated IP and port.
developed with visual studio community, .NET 8.0 toolchain.


## csUDPSendCsv

usage : csUDPSendCsv.exe {data.csv} {dst ip address} {dst port}

it transfers data.csv file through UDP 
data.csv should contain byte columns 
destination IP address is like 127.0.0.1 
dst port is like 5299

## csCastSendCsv

usage : csCastUDPCsv.exe {data.csv} {dst ip address} {dst port}

it transfers data.csv file through UDP 
data.csv should contain data columns 
each column data type is defined in CastType.xml
destination IP address is like 127.0.0.1 
dst port is like 5299
