# Html.Rt
##### Fast, Simple Html Parsing Library 
---
## Install
#### Nuget
install from nuget
```sh
Install-Package Html.Rt
```
## Usage

```cs
 var file = System.IO.File.ReadAllText('path of file'); 
 var document = new Html.Rt.Document(file);
 var markedElement = document.GetElementById("markedElement");
```


