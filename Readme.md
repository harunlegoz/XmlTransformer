# Rahl.XmlTransformer
A library to help ease the pain of transforming XML into JSON and CSV using XPath but not XSLT.

## What is it?
The library originated from a proof-of-concept where the objective was to use Azure Data Factory v2 to transform an XML file and push the records into an Azure SQL Database. ADFv2 is a powerful tool but it doesn't support XML as an input type, so only thing possible with XML on ADFv2 is to copy the file as binary.

One option was to use Durable Azure Functions to transform XML but that required writing a custom function for every file type. Or, I could create a generic function that accepts the mapping through REST message body as JSON and do the transformation accordingly. This is the outcome of that PoC.

## Usage
XmlTransformer is specifically built to transform array-structured XML files and them alone (at least for now). The supported XML structure should start with a root element and then an item repeating within the root element. It treats each element as an array item/record and transforms it to target structure (JSON or CSV) record with or without a mapping.

The root element's attributes are also included within each target record, since the target is always treated as tabular. The same attributes will appear on each target record, allowing a consistent tabular structure.

Here's a sample valid XML file:
```xml
<items>
    <item>
        <prop1>A</prop1>
        <prop2>
            <prop21>B</prop21>
            <prop22>C</prop22>
        </prop2>
        <prop3>
            <child>D</child>
            <child>E</child>
            <child>F</child>
            <child>G</child>
        </prop3>
    </item>
    <item>
        <prop1>A</prop1>
        <prop2>
            <prop21>B</prop21>
            <prop22>C</prop22>
        </prop2>
        <prop3>
            <child>D</child>
            <child>E</child>
        </prop3>
    </item>
</items>
```

### Azure Function Example
It doesn't have NuGet packages yet, but I intend to add and release them shortly. At the moment, feel free to copy the source, edit the examples and deploy them anywhere you want.

There's a function with HttpTrigger to do on-the-fly transformation with small files. I'll be adding the durable function example, so you can transform large files as well.

Here's a sample JSON request:

```json
{
	"format": "json",
	"mapping": {
		"attr1": "$/@attr1",
		"attr2": "$/@attr2",
		"prop1": "/prop1/text()",
		"prop2": "/prop2/prop21/text()"
	},
	"data": "<items attr1=\"A1\" attr2=\"A2\"><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item><item><prop1>A</prop1><prop2><prop21>B</prop21><prop22>C</prop22></prop2><prop3><child>D</child><child>E</child><child>F</child><child>G</child></prop3></item></items>"
}
```

The library currently can transform XML into JSON and CSV. It has two modes:

### Direct Usage
Transforms entire XML into JSON and CSV, protecting its hierarchy. With JSON it's quite straightforward, but with CSV it's tricky. It combines the inner elements with "=" character to allow parsing the values later. You do direct transformation by _not_ providing mapping rules:

```csharp
var input = "<items><item>A</item></items>";
var transformer = new XmlToJsonTransformer();
var output =  await transformer.TransformAsync(input);
```
### Mapping Usage
Transforms XML into JSON and CSV with the provided mapping values. This mode _always_ creates a tabular data format, even with JSON. You define your data type in your mapping like this:
```csharp
var input = "<items attr1=\"A1\" attr2=\"A2\"><item>A</item><item>B</item><item>C</item><item>D</item><item>E</item></items>";

// Act
var config = new TransformerConfiguration()
{
    Mapping = new[]
    {
        new Map("attr1", "$/@attr1"),
        new Map("attr2", "$/@attr2"),
        new Map("value", "/text()")
    }
};
var transformer = new XmlToJsonTransformer(config);
var output =  await transformer.TransformAsync(input);
```
The output json file will look like below:
```json
[
    {
        "attr1": "A1",
        "attr2": "A2",
        "value": "A"
    },
    {
        "attr1": "A1",
        "attr2": "A2",
        "value": "B"
    },
    {
        "attr1": "A1",
        "attr2": "A2",
        "value": "C"
    },
    {
        "attr1": "A1",
        "attr2": "A2",
        "value": "D"
    },
    {
        "attr1": "A1",
        "attr2": "A2",
        "value": "E"
    }
]
```

### Mapping Rules
Mapping is defined with field based XPath queries, with an additional option to define data type to make a type-safe transformation. Following mapping values are all valid:

```csharp
var config = new TransformerConfiguration()
{
    Mapping = new[]
    {
        new Map("attr1", "$/@attr1"), // Root element's attr1 attribute. This is a special notation, not a valid XPath query.
        new Map("value1", "/prop1/text()"), // Reads prop1 subelement's inner text as string and assigns it as value1.
        new Map("value2", "/prop1/text() | /prop2/text()"), // This is an OR statement of XPath query, checking if prop1 exists and gets it text, otherwise checks prop2 and gets its text
        new Map("value3", "int -> /prop3/text()"), //Reads prop3's inner text as integer to assign as value3
        new Map("value4", "decimal -> /prop4/text()"), //Reads prop4's inner text as decimal to assign as value4
        new Map("value5", "long -> /prop5/text()"), //Reads prop5's inner text as long to assign as value5
        new Map("value6", "bool -> /prop6/text()"), //Reads prop6's inner text as boolean to assign as value6
        new Map("value7", "string -> /prop7/text()"), //Reads prop7's inner text as string (default data type) to assign as value7
        new Map("value8", "datetime -> /prop8/text()"), //Reads prop8's inner text as datetime using default format provider (which is CurrentCulture) to assign as value8
        new Map("value9", "datetime(yyyy/MM/dd) -> /prop9/text()"), //Reads prop9's inner text as datetime using the provided format (which is yyyy/MM/dd) to assign as value9
    }
};
```

Notes:
- Supported data types: string, int, long, decimal, bool, datetime
- If data type isn't provided, string will be used as default data type.
- Format is only valid for datetime data type, for others, it'll be ignored.

## To-do List
- Add Azure Data Factory example to call http function
- Publish to NuGet feed