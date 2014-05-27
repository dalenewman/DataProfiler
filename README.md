DataProfiler
============
A .NET based command line tool (and library) for profiling delimited files, Excel (the 1st sheet), and database tables.

Released under GNU General Public License, version 3 (GPL-3.0).

The command line tool is dp.exe.  It takes 1 to 3 arguments:

1. **Input**: A file name or fully qualified database table name.  The fully qualified name must indicate the Server, the Database, the Schema, and the Table's name delimited by dots.  For example, `Server.Database.Schema.Table` is correct.
  * When your server name contains dots, surround it with brackets (e.g. `[Server.com]`).
  * If your provider doesn't support schemas, use `Server.Database.Table`.
2. **Output** (optional): An optional file name.  By default, the file is comma delimited.  So, you may choose a .txt or .csv extension and it will open in NotePad or Excel respectively.  However, if you use an .html extension, the output is is an HTML table with Bootstrap styling.
3. **Sample Percentage** (optional): If you don't need a perfect data profile, use an integer between 1 and 99 to sample a percentage of the data.  This can really speed things up.

By default, database objects assume a provider of SqlServer.  You may configure DP to expect MySql, or PostgreSql as well.  You may configure ports, usernames, and passwords as needed.  This is done in dp.exe.config.

```xml
  <transformalize>
    <processes>
      <!-- JunkDrawer process is for profiling files -->
      <add name="JunkDrawer">
        <connections>
          <add name="input" provider="file" file="*"/>
          <add name="output" provider="internal"/>
        </connections>
      </add>
      <add name="DataProfiler">
        <connections>
          <!-- by default, provider is sqlserver -->
          <add name="input" />
          
          <!-- mysql 
          <add name="input" provider="mysql" port="*" user="*" password="*" />
          -->
          
          <!-- postgresql
          <add name="input" provider="postgresql" port="*" user="*" password="*" />
          -->
          
          <!-- default output is data-profile.html, wherever your executable is -->
          <add name="output" provider="html" file="data-profile.html" />
        </connections>
        <actions>
          <!-- remove this if you don't want data-profile.html to open after profile is complete -->
          <add action="open" connection="output" />
        </actions>
        <entities>
          <!-- you can control the default sample size here, and whether or not nolock hint is used -->
          <add name="*" sample="100" no-lock="true" />
        </entities>
      </add>
    </processes>
  </transformalize>
```


