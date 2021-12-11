echo.服务启动......  
echo off  
sc create Consul binpath="D:\Consul\consul.exe agent -server -ui -node=10.0.194.104 -bind 10.0.194.104 -client=0.0.0.0 -bootstrap-expect 1 -data-dir D:\Consul\data -config-file D:\Consul\conf"
net start Consul
sc config Consul start= AUTO  
echo.Consul启动完毕！ 
pause