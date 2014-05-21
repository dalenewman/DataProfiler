DataProfiler
============
A .NET based library and command line tool for data profiling.  Use it to profile Excel, delimited text files, and database tables or views.

Released under GNU General Public License, version 3 (GPL-3.0).

<code>dp input (output)</code>

Input is either a file name, or a fully qualified database table or view name (e.g. Server.Database.Schema.Table).
Output is an optional file name.  If *.html, some HTML markup and style is added.  Otherwise, it's a delimited file.

* <code>dp c:\temp\data.txt c:\temp\data-profile.html</code>
* <code>dp localhost.Junk.dbo.Table</code>
* <code>dp [127.0.0.1].Junk.dbo.Table table-profile.csv</code>


The command line only takes 2 arguments: input, and output.

By default, database objects are assumed to by SqlServer.  You may configure dp to expect MySql, or PostgreSql as well.  MySql doesn't support schemas, so qualify the table without them (e.g. Server.Database.Table).  You can configure ports, usernames, and passwords as needed.


