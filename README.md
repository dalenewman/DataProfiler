DataProfiler
============

[Data Profiler](https://github.com/dalenewman/DataProfiler) is an open source 
.NET based command line tool (and library) for profiling delimited files, 
Excel (the 1st sheet), and database tables.

It is released under Apache 2.

---
#### Usage

```bash
dp

Data Profiler 0.1.0.0
Copyright 2013-2018 Dale Newman

  -s, --server             (Default: localhost) The server's name or ip address.

  -d, --database           (Default: ) The database name.

  -o, --schema owner       (Default: ) The schema or owner name.

  -t, --table              (Default: ) The table (or view) name.

  -u, --user               (Default: ) The user name.

  -p, --password           (Default: ) The password.

  -f, --file               (Default: ) The file.

  -c, --connection type    (Default: sqlserver) The connection type or provider
                           (i.e. sqlserver, mysql, postgresql, sqlite, file, or
                           excel.)

  -n, --port number        (Default: 0)

  -l, --limit              (Default: 15) To limit the Min Value and Max Value
                           text returned.

  --help                   Display this help screen.
```


#### Usage with File

```bash
dp -fc:\temp\Data\ff\years_2013_fantasy_fantasy.csv

Name,Type,Position,Count,Min Value,Max Value,Min Length,Max Length
Rk,short,1,741,"1","741",1,3
Name,string,2,738,"A.J. Green*","Zach Sudfeld",8,23
Tm,string,3,36,"2TM","WAS",3,3
Age,byte,4,19,"21","41",2,2
Games G,byte,5,17,"0","16",1,2
Games GS,string,6,18,"","9",0,2
Passing Cmp,short,7,58,"0","450",1,3
Passing Att,short,8,61,"0","659",1,3
Passing Yds,short,9,71,"0","5477",1,4
Passing TD,byte,10,30,"0","55",1,2
Passing Int,byte,11,21,"0","27",1,2
Rushing Att,short,12,109,"0","314",1,3
Rushing Yds,short,13,174,"-31","1607",1,4
Rushing YA,string,14,199,"","9.50",0,5
Rushing TD,byte,15,13,"0","12",1,2
Receiving Tgt,string,16,126,"","99",0,3
Receiving Rec,byte,17,92,"0","113",1,3
Receiving Yds,short,18,303,"-9","1646",1,4
Receiving YR,string,19,333,"","9.96",0,5
Receiving TD,byte,20,16,"0","16",1,2
Fantasy Pos,string,21,5,"","WR",0,2
Fantasy Pt,string,22,182,"","99",0,3
Fantasy DKPt,string,23,437,"","99.3",0,5
Fantasy FDPt,string,24,428,"","99.7",0,5
Fantasy VBD,string,25,56,"","97",0,3
Fantasy Pos Rk,short,26,287,"1","287",1,3
Fantasy Ov Rk,string,27,79,"","9",0,2
```

### Usage with File Piped to CSV and use Excel

```bash
dp -fc:\temp\Data\ff\years_2013_fantasy_fantasy.csv > output.csv && excel output.csv
```

![Excel](https://raw.githubusercontent.com/dalenewman/DataProfiler/master/Images/Excel.png)

**Note**: For this to work you have to have Excel.exe on your path.  You can use `start output.csv` to use your registered `.csv` editor.

### Usage with SQL Server Table

```bash
dp -dNorthWind -tCustomers

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
RowVersion,byte[],12,1,"","",0,0
```
