DataProfiler
============
Profile Excel, delimited text files, and database tables\views.

Released under GNU General Public License, version 3 (GPL-3.0).

Usage: dp <input> (<output>)

<input> is either a file name, or a fully qualified database table \ view name.
<output> is an optional file name.  If *.html, some HTML markup and style is added.  Otherwise, it's a delimited file.

dp c:\temp\data.txt c:\temp\data-profile.html
dp localhost.Junk.dbo.Table
dp [127.0.0.1].Junk.dbo.Table table-profile.csv

The command line only takes <input> and <output>.  More settings are available in dp.exe.config. See wiki for more information.


