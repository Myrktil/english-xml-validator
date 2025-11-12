# EnglishXMLValidator

This programm searches a BG3 english.xml file for duplicate UUIDs.  
Simply place the english.xml file in the same folder as the programm and run it.

While the programm handles missing linebreaks, it expects the xml syntax to be upheld and won't work otherwise. This means entries need to follow this pattern:  

\<content contentuid="123" version="123">text</content\>