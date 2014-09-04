
Overview
--------
NodeXL is an Excel 2007/2010/2013 template for displaying and analyzing network
graphs, along with an API for incorporating graphs in other applications.  To
build NodeXL, see .\HowToBuildNodeXL.txt.

The following sections describe the subdirectories within this directory,
listed in alphabetical order.


Adapters
--------
Class library project.  Contains graph adapters that read and write graphs to
various file formats.


Algorithms
----------
Class library project.  Contains classes that implement graph algorithms.


ApplicationUtil
---------------
Class library project.  Contains classes useful to graphing applications,
including ExcelTemplate.


BuildTools
----------
Tools needed by the build process, but not needed on client machines.


Common
------
Classes that are used by two or more projects but not compiled into a shared
assembly.  The classes are linked to the projects via the Add as Link option in
Visual Studio's Add Existing Item dialog.


Core
----
Class library project.  Contains core classes representing graphs, vertices,
and edges.


Documents
---------
NodeXL documentation.


ExcelTemplate
-------------
VSTO-based Excel 2007/2010/2013 template project that displays a graph within
an Excel workbook.


ExcelTemplatePlugIns
--------------------
Class library project.  Contains interface definitions for plug-ins used by
ExcelTemplate.


ExcelTemplatePostDeploymentAction
---------------------------------
Class library project.  Contains an action that gets run on the client computer
right after the ExcelTemplate project is deployed.


GraphDataProviders
------------------
Class library project.  Contains several classes that implement the
Microsoft.NodeXL.ExcelTemplatePlugIns.IGraphDataProvider interface.   These
classes are plugins for the ExcelTemplate project.  They import graph data into
the template from data sources that the template doesn't know about, such as
Twitter, YouTube, and Flickr.


GraphMLFileProcessor
--------------------
Windows Forms project.  Continuously checks for new GraphML files created by
the Network Server console application, and creates and automates a NodeXL
workbook for each such file, one at a time.  This was implemented after it was
discovered that simultaneous Excel instances created by multiple instances of
the NetworkServer program do not work properly.  GraphMLFileProcessor never
instantiates more than one Excel instance at a time.


GraphMLFileProcessorStarter
---------------------------
Console project.  Starts the GraphMLFileProcessor program.  The
GraphMLFileProcessor program is buried in a folder within the ClickOnce cache
and the folder changes with every NodeXL ClickOnce update, so starting the
program is difficult.  GraphMLFileProcessorStarter looks in the Windows Start
menu for the NodeXL Excel Template shortcut, gets NodeXL's ClickOnce path from
the shortcut, and starts the GraphMLFileProcessor program using that path.


Layouts
-------------
Class library project.  Contains classes that lay out graphs.


NetworkServer
-------------
Console project.  Gets a network using the classes in GraphDataProviders and
stores it in a GraphML file.  This is a console-based alternative to getting
the same graph data from within the ExcelTemplate.


NetworkServerStarter
--------------------
Console project.  Starts the NetworkServer program.  The NetworkServer program
is buried in a folder within the ClickOnce cache and the folder changes with
every NodeXL ClickOnce update, so starting the program is difficult.
NetworkServerStarter looks in the Windows Start menu for the NodeXL Excel
Template shortcut, gets NodeXL's ClickOnce path from the shortcut, and starts
the NetworkServer program using that path.


TestGraphDataProviders
----------------------
Windows Forms project.  Tests the IGraphDataProvider classes in
GraphDataProviders.


TestWpfNodeXLControl
--------------------
Windows Forms project.  Tests the NodeXLControl.


UnitTests
---------
Unit test project.  Tests classes in the other projects.


Util
----
Class library project.  Contains classes used by two or more of the other
projects.


WpfControl
----------
WPF custom control library project.  Contains NodeXLControl, a WPF control that
draws graphs.


WpfVisualization
----------------
Class library project.  Contains classes used to draw graphs in the WPF
environment.


