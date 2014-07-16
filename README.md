DataProfiler
============

[Data Profiler](https://github.com/dalenewman/DataProfiler) is an open source .NET based command line tool (and library) for profiling delimited files, Excel (the 1st sheet), and database tables.  It uses [Transformalize](https://github.com/dalenewman/Transformalize) and [Junk Drawer](https://github.com/dalenewman/JunkDrawer) to do most of it's work.

It is released under GNU General Public License, version 3 (GPL-3.0).

The command line tool is `dp.exe`.  It takes 1 to 3 arguments:

---

####Argument 1, Input:
Input should be a file name or fully qualified database table name.

_file:_
```bash
dp c:\temp\fantasy\fantasy2013.txt
```
---
<table class="table table-striped table-condensed table-hover">
		<thead>
            <th>Field</th><th>Type</th><th>Index</th><th>Distinct Count</th><th>Min Value</th><th>Max Value</th><th>Min Length</th><th>Max Length</th>
		</thead>
		<tbody>
			<tr><td>Rank</td><td>int16</td><td>1</td><td>598</td><td>1</td><td>598</td><td>1</td><td>3</td></tr>
			<tr><td>Name</td><td>string</td><td>2</td><td>597</td><td>A.J. Green*</td><td>Zach Sudfeld</td><td>8</td><td>23</td></tr>
			<tr><td>Team</td><td>string</td><td>3</td><td>35</td><td>2TM</td><td>WAS</td><td>3</td><td>3</td></tr>
			<tr><td>Age</td><td>byte</td><td>4</td><td>18</td><td>21</td><td>38</td><td>2</td><td>2</td></tr>
			<tr><td>Games</td><td>byte</td><td>5</td><td>16</td><td>1</td><td>16</td><td>1</td><td>2</td></tr>
			<tr><td>GamesStarted</td><td>byte</td><td>6</td><td>17</td><td>0</td><td>16</td><td>1</td><td>2</td></tr>
			<tr><td>PassingCmp</td><td>int16</td><td>7</td><td>58</td><td>0</td><td>450</td><td>1</td><td>3</td></tr>
			<tr><td>PassingAtt</td><td>int16</td><td>8</td><td>61</td><td>0</td><td>659</td><td>1</td><td>3</td></tr>
			<tr><td>PassingYds</td><td>int16</td><td>9</td><td>71</td><td>0</td><td>5477</td><td>1</td><td>4</td></tr>
			<tr><td>PassingTD</td><td>byte</td><td>10</td><td>30</td><td>0</td><td>55</td><td>1</td><td>2</td></tr>
			<tr><td>PassingInt</td><td>byte</td><td>11</td><td>21</td><td>0</td><td>27</td><td>1</td><td>2</td></tr>
			<tr><td>RushingAtt</td><td>int16</td><td>12</td><td>109</td><td>0</td><td>314</td><td>1</td><td>3</td></tr>
			<tr><td>RushingYds</td><td>int16</td><td>13</td><td>174</td><td>-31</td><td>1607</td><td>1</td><td>4</td></tr>
			<tr><td>RushingYA</td><td>single</td><td>14</td><td>198</td><td>-9</td><td>45</td><td>1</td><td>5</td></tr>
			<tr><td>RushingTD</td><td>byte</td><td>15</td><td>13</td><td>0</td><td>12</td><td>1</td><td>2</td></tr>
			<tr><td>ReceivingRec</td><td>byte</td><td>16</td><td>92</td><td>0</td><td>113</td><td>1</td><td>3</td></tr>
			<tr><td>ReceivingYds</td><td>int16</td><td>17</td><td>304</td><td>-9</td><td>1646</td><td>1</td><td>4</td></tr>
			<tr><td>ReceivingYR</td><td>single</td><td>18</td><td>334</td><td>-5</td><td>57</td><td>1</td><td>5</td></tr>
			<tr><td>ReceivingTD</td><td>byte</td><td>19</td><td>16</td><td>0</td><td>16</td><td>1</td><td>2</td></tr>
			<tr><td>FantPos</td><td>string</td><td>20</td><td>4</td><td>QB</td><td>WR</td><td>2</td><td>2</td></tr>
			<tr><td>FantPoints</td><td>int16</td><td>21</td><td>176</td><td>-3</td><td>410</td><td>1</td><td>3</td></tr>
			<tr><td>VBD</td><td>byte</td><td>22</td><td>56</td><td>0</td><td>182</td><td>1</td><td>3</td></tr>
			<tr><td>PosRank</td><td>byte</td><td>23</td><td>222</td><td>1</td><td>223</td><td>1</td><td>3</td></tr>
			<tr><td>OvRank</td><td>byte</td><td>24</td><td>79</td><td>0</td><td>78</td><td>1</td><td>2</td></tr>
      </tbody>
	</table>

_table:_

```bash
dp localhost.NorthWind.dbo.Customers
```
---
 <table class="table table-striped table-condensed table-hover">
	<thead>
        <th>Field</th><th>Type</th><th>Index</th><th>Distinct Count</th><th>Min Value</th><th>Max Value</th><th>Min Length</th><th>Max Length</th>
	</thead>
	<tbody>
		<tr><td>CustomerID</td><td>string</td><td>1</td><td>91</td><td>ALFKI</td><td>WOLZA</td><td>5</td><td>5</td></tr>
		<tr><td>CompanyName</td><td>string</td><td>2</td><td>91</td><td>Alfreds Futterkiste</td><td>Wolski  Zajazd</td><td>8</td><td>36</td></tr>
		<tr><td>ContactName</td><td>string</td><td>3</td><td>91</td><td>Alejandra Camino</td><td>Zbyszek Piestrzeniewicz</td><td>8</td><td>23</td></tr>
		<tr><td>ContactTitle</td><td>string</td><td>4</td><td>12</td><td>Accounting Manager</td><td>Sales Representative</td><td>5</td><td>30</td></tr>
		<tr><td>Address</td><td>string</td><td>5</td><td>91</td><td>1 rue Alsace-Lorraine</td><td>Walserweg 21</td><td>11</td><td>46</td></tr>
		<tr><td>City</td><td>string</td><td>6</td><td>69</td><td>Aachen</td><td>Warszawa</td><td>4</td><td>15</td></tr>
		<tr><td>Region</td><td>string</td><td>7</td><td>19</td><td></td><td>WY</td><td>0</td><td>13</td></tr>
		<tr><td>PostalCode</td><td>string</td><td>8</td><td>87</td><td></td><td>WX3 6FW</td><td>0</td><td>9</td></tr>
		<tr><td>Country</td><td>string</td><td>9</td><td>21</td><td>Argentina</td><td>Venezuela</td><td>2</td><td>11</td></tr>
		<tr><td>Phone</td><td>string</td><td>10</td><td>91</td><td>(02) 201 24 67</td><td>981-443655</td><td>8</td><td>17</td></tr>
		<tr><td>Fax</td><td>string</td><td>11</td><td>70</td><td></td><td>981-443655</td><td>0</td><td>17</td></tr>
		<tr><td>RowVersion</td><td>byte[]</td><td>12</td><td>91</td><td>System.Byte[]</td><td>System.Byte[]</td><td>13</td><td>13</td></tr>
  </tbody>
</table>


The fully qualified database table name must indicate the **Server**, the **Database**, the **Schema**, and the **Table** name.  It must be delimited by dots.  For example, `Server.Database.Schema.Table` is correct.  When your server name contains dots, surround it with brackets (e.g. `[Server.com]`).  If your provider doesn't support schema, use `Server.Database.Table`.

By default, database objects assume a provider of `sqlserver`.  You may configure it to expect `mysql`, or `postgresql` as well.  If you're not using SQL Server with trusted authentication, you may configure ports, user names, and passwords as needed in *dp.exe.config*.

---

####Argument 2, Output:

Output is optional.  If you don't provide it, it will be whatever is defined as output in your DataProfiler process (see *dp.exe.config*). It is usually *data-profile.html*. If your output file has an .html extension, it is rendered and opened as an HTML table with Bootstrap styling.  You may also choose a .txt or .csv extension and it will open in NotePad or Excel respectively.

---

####Argument 3, Sample Percentage:

Sample Percentage is optional.  If you don't need a perfect data profile, use an integer between 1 and 99 to sample a percentage of the data.  This can really speed things up on bigger files (at the cost of accuracy though).

---

####dp.exe.config

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

				<!-- for other database providers -->
				<!-- <add name="input" provider="mysql" port="*" user="*" password="*" /> -->
				<!-- <add name="input" provider="postgresql" port="*" user="*" password="*" /> -->

				<!-- default output is data-profile.html, in the same folder your dp.exe is in -->
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
