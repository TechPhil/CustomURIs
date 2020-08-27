# CustomURIs
An app which registers and allows editing of a custom URI schema (goto://)

CustomURIs utilises URI schemas (such as mailto:, ftp://, etc.) to make a custom shortlink system for your Windows PC!



<b>Before running the application, move the .exe file to a suitable place. The exe cannot be moved once the application has been run the first time</b>

When first running the application, you will need to run as Administrator - This is so that the schema can be registered in the Windows Registry.
From that point onwards, type `goto:edit` or `goto://edit` in the run box to open up `Links.xml` - the super hi tech database that holds the links.


XML Structure
```xml
<root>
  <url>
    <shortlink></shortlink>
    <fullurl></fullurl>
  </url>
</root>
```
Set `shortlink` to the thing you want after `goto:` or `goto://`.<br>
`fullurl` should be the link to open, or the command to run

### Example

If you wanted to set a shortlink of `goto:google` (or `goto://google`) to redirect you to www.google.co.uk, then your XML should look like this - 
```xml
<root>
  <url>
    <shortlink>google</shortlink>
    <fullurl>https://www.google.com</fullurl>
  </url>
</root>
```
You can then run `goto:google` or `goto://google` in the Run dialog box, or even in your web browser!
