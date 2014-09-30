netmqSignalrMessageHub
======================

A netmq implementation of a SignalR IMessageHub/Backplane

Building

restore nuget packages:

nuget restore netmqSignalrMessageHub.sln 

mono:
XBuild netmqSignalrMessageHub.sln 

windows:
MSbuild netmqSignalrMessageHub.sln 


Running samples

Server

SampleServer.exe <http listening address> <netmq publishing socket> <netmq subscriber socket>


Client:

mono SampleClient.exe  <server http listening address>


Two servers and two clients

fire up 4 shells or dos prompts, cd to bin/Debug folder for server and client respectively

the two servers

mono SampleServer.exe http://localhost:8080 tcp://127.0.0.1:5002 tcp://127.0.0.1:5003

mono SampleServer.exe http://localhost:8081 tcp://127.0.0.1:5003 tcp://127.0.0.1:5002


the two clients

mono SampleClient.exe  http://localhost:8080

mono SampleClient.exe  http://localhost:8081
