DataProfiler
============

[Data Profiler](https://github.com/dalenewman/DataProfiler) is an open source 
.net core based command line tool (and library) for profiling delimited files, 
excel (the 1st sheet), and database tables.

It is released under Apache 2.

---
#### Usage

```bash
Data Profiler 0.2.0
Copyright 2013-2025 Dale Newman

  -c, --connection-type    Required. (Default: ) The connection type or provider (i.e. sqlserver, mysql, postgresql, sqlite, file, or excel.)
  -s, --server             (Default: ) The server's name or ip address.
  -d, --database           (Default: ) The database name.
  -o, --schema             (Default: ) The schema name.
  -t, --table              (Default: ) The table (or view) name.
  -u, --user               (Default: ) The user name.
  -p, --password           (Default: ) The password.
  -f, --file               (Default: ) The file.
  -n, --port-number        (Default: 0)
  -l, --limit              (Default: 15) To limit the Min Value and Max Value text returned.
  -m, --in-memory          (Default: true) Load everything into memory and profile, else try reducing memory footprint.
  --help                   Display this help screen.
  --version                Display version information.
```

#### Usage with File

```bash
dp.cli.exe -c file  -f SacramentoCrimeJanuary2006.csv 

Name,Type,Position,Count,Min Value,Max Value,Min Length,Max Length
cdatetime,datetime,1,5094,"1/1/2006 12:00:00 AM","1/31/2006 11:50:00 PM",19,21
address,string,2,5492,"1 CLAUSS CT","YREKA AVE / CAN...",8,40
district,byte,3,6,"1","6",1,1
beat,string,4,20,"          ","6C        ",10,10
grid,short,5,539,"102","1661",3,4
crimedescr,string,6,304,"10.48.150 E FAL...","WARRANT SERVED ...",9,30
ucr_ncic_code,short,7,88,"909","8102",3,4
latitude,single,8,4973,"38.438","38.68379",6,9
longitude,single,9,4460,"-121.55583","-121.365234",7,11
```

### Usage with File Piped to CSV and use Excel

```bash
dp.cli.exe -c file -f c:\temp\Data\ff\years_2013_fantasy_fantasy.csv > output.csv && excel output.csv
```

![Excel](https://raw.githubusercontent.com/dalenewman/DataProfiler/master/Images/Excel.png)

**Note**: For this to work you have to have Excel.exe on your path.  You can use `start output.csv` to use your registered `.csv` editor.

### Usage with SQL Server Table

```bash
dp.cli.exe -c sqlserver -s localhost -u username -p password -d NorthWind -t Customers

Name,Type,Position,Count,Min Value,Max Value,Min Length,Max Length
CustomerID,string,1,91,"ALFKI","WOLZA",5,5
CompanyName,string,2,91,"Alfreds Futterk...","Wolski  Zajazd",8,36
ContactName,string,3,91,"Alejandra Camin...","Zbyszek Piestrz...",8,23
ContactTitle,string,4,12,"Accounting Mana...","Sales Represent...",5,30
Address,string,5,91,"1 rue Alsace-Lo...","Walserweg 21",11,46
City,string,6,69,"Aachen","Warszawa",4,15
Region,string,7,19,"","WY",0,13
PostalCode,string,8,87,"","WX3 6FW",0,9
Country,string,9,21,"Argentina","Venezuela",2,11
Phone,string,10,91,"(02) 201 24 67","981-443655",8,17
Fax,string,11,70,"","981-443655",0,17
```
