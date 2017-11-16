# About NodeXL Plug-Ins

The NodeXL Excel template has built-in support for importing network graph data from a variety of sources, including edge list workbooks, matrix workbooks, UCINet files, Pajek files, GraphML files, and so on.  You can import from these sources using the Data, Import menu in the NodeXL tab in Excel’s ribbon.

If you know C#, VB.NET, or another .NET language, you can add a custom item to the Data, Import menu that will import from a data source not directly supported by the template, such as a corporate network directory, a public social network API, or an obscure graph file format.  You do this by creating a “graph data provider” plug-in, which is a .NET assembly containing a class that implements a NodeXL interface called IGraphDataProvider.  You then put this assembly into the NodeXL template’s plug-in folder.

When the user opens the Data, Import menu and selects your custom menu item, the text of which you specify via a property on your class, the template calls a method on your class named TryGetGraphData().  This method typically opens a custom dialog box that collects graph data parameters from the user, gets the specified graph data from the data source, and returns the graph data to the template in GraphML format.  GraphML is a standard, extensible, XML-based format for storing network graph data.  The template uses the GraphML data to populate the Excel workbook.

To get started with creating a graph data provider plug-in for the NodeXL template, do the following:

* Download the latest version of the NodeXL Class Libraries from the [Downloads](http://nodexl.codeplex.com/Release/ProjectReleases.aspx) tab.

* Open the NodeXLApi.chm help file.  (Important Note: You may see nothing but empty topics when you attempt to view the NodeXLApi.chm file. To fix this problem, which is due to a security restriction in Internet Explorer 7, right-click the chm file in Windows Explorer, select Properties, and click the Unblock button on the General tab.)

* Read the "IGraphDataProvider Interface" topic.