
using System;
using System.IO;
using System.Reflection;
using System.Collections.Generic;
using System.Diagnostics;
using Smrf.NodeXL.Core;
using Smrf.NodeXL.Adapters;
using Microsoft.NodeXL.ExcelTemplatePlugIns;

namespace Smrf.NodeXL.ExcelTemplate
{
//*****************************************************************************
//  Class: PlugInManager
//
/// <summary>
/// Provides access to plug-in classes.
/// </summary>
///
/// <remarks>
/// Call <see cref="GetGraphDataProviders" /> to get an array plug-in classes
/// that implement <see cref="IGraphDataProvider" />.
///
/// <para>
/// All methods are static.
/// </para>
///
/// </remarks>
//*****************************************************************************

public static class PlugInManager
{
    //*************************************************************************
    //  Method: GetGraphDataProviders()
    //
    /// <summary>
    /// Gets an array of plug-in classes that implement either the newer <see
    /// cref="IGraphDataProvider2" /> interface or the older <see
    /// cref="IGraphDataProvider" /> interface.
    /// </summary>
    ///
    /// <returns>
    /// An array of zero or more <see cref="IGraphDataProvider2" /> or <see
    /// cref="IGraphDataProvider" /> implementations.
    /// </returns>
    ///
    /// <remarks>
    /// The <see cref="IGraphDataProvider2" /> and <see
    /// cref="IGraphDataProvider" /> interfaces allow developers to
    /// create plug-in classes that import graph data into the NodeXL Excel
    /// Template without having to modify the ExcelTemplate's source code.  See
    /// <see cref="IGraphDataProvider2" /> for more information.
    /// </remarks>
    //*************************************************************************

    public static Object []
    GetGraphDataProviders()
    {
        List<Object> oGraphDataProviders = new List<Object>();
        IEnumerable<String> oFileNames;

        if ( TryGetFilesInPlugInFolder(out oFileNames) )
        {
            foreach (String sFileName in oFileNames)
            {
                // The techniques for checking types for a specified interface
                // and instantiating instances of those types are from the
                // article "Let Users Add Functionality to Your .NET
                // Applications with Macros and Plug-Ins" in the October 2003
                // issue of MSDN Magazine.

                Type [] aoTypes;

                if ( !TryGetTypesFromFile(sFileName, out aoTypes) )
                {
                    continue;
                }

                foreach (Type oType in aoTypes)
                {
                    if (!oType.IsAbstract)
                    {
                        if (
                            typeof(IGraphDataProvider2).IsAssignableFrom(oType)
                            ||
                            typeof(IGraphDataProvider).IsAssignableFrom(oType)
                            )
                        {
                            // TODO: This is to prevent the inclusion of the
                            // Graph Server graph data provider, which as of
                            // June 2013 is not fully implemented.

                            if (oType.Name !=
                                "GraphServerNetworkGraphDataProvider")

                            oGraphDataProviders.Add(
                                Activator.CreateInstance(oType) );
                        }
                    }
                }
            }
        }

        oGraphDataProviders.Sort(
            delegate
            (
                Object oGraphDataProviderA,
                Object oGraphDataProviderB
            )
            {
                Debug.Assert(oGraphDataProviderA != null);
                Debug.Assert(oGraphDataProviderB != null);

                return ( GetGraphDataProviderName(oGraphDataProviderA).
                    CompareTo( GetGraphDataProviderName(oGraphDataProviderB) )
                    );
            }
            );

        return ( oGraphDataProviders.ToArray() );
    }

    //*************************************************************************
    //  Method: GetGraphDataProviderName()
    //
    /// <summary>
    /// Gets the name of a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <returns>
    /// The name of the graph data provider.
    /// </returns>
    //*************************************************************************

    public static String
    GetGraphDataProviderName
    (
        Object graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        return ( (graphDataProvider is IGraphDataProvider2) ?
            ( (IGraphDataProvider2)graphDataProvider ).Name
            :
            ( (IGraphDataProvider)graphDataProvider ).Name
            );
    }

    //*************************************************************************
    //  Method: GetGraphDataProviderDescription()
    //
    /// <summary>
    /// Gets the description of a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <returns>
    /// The description of the graph data provider.
    /// </returns>
    //*************************************************************************

    public static String
    GetGraphDataProviderDescription
    (
        Object graphDataProvider
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        return ( (graphDataProvider is IGraphDataProvider2) ?
            ( (IGraphDataProvider2)graphDataProvider ).Description
            :
            ( (IGraphDataProvider)graphDataProvider ).Description
            );
    }

    //*************************************************************************
    //  Method: TryGetGraphFromGraphDataProvider()
    //
    /// <summary>
    /// Attempts to get a graph from a graph data provider.
    /// </summary>
    ///
    /// <param name="graphDataProvider">
    /// An object that implements either <see cref="IGraphDataProvider2" /> or
    /// <see cref="IGraphDataProvider" />.
    /// </param>
    ///
    /// <param name="graph">
    /// Where the graph gets stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the graph was obtained.
    /// </returns>
    //*************************************************************************

    public static Boolean
    TryGetGraphFromGraphDataProvider
    (
        Object graphDataProvider,
        out IGraph graph
    )
    {
        Debug.Assert(graphDataProvider != null);

        Debug.Assert(graphDataProvider is IGraphDataProvider2 ||
            graphDataProvider is IGraphDataProvider);

        graph = null;
        GraphMLGraphAdapter oGraphMLGraphAdapter = new GraphMLGraphAdapter();

        if (graphDataProvider is IGraphDataProvider2)
        {
            String sPathToTemporaryFile = null;

            if ( !( (IGraphDataProvider2)graphDataProvider )
                .TryGetGraphDataAsTemporaryFile(out sPathToTemporaryFile) )
            {
                return (false);
            }

            try
            {
                graph = oGraphMLGraphAdapter.LoadGraphFromFile(
                    sPathToTemporaryFile);
            }
            finally
            {
                File.Delete(sPathToTemporaryFile);
            }
        }
        else
        {
            String sGraphDataAsGraphML;

            if ( !( (IGraphDataProvider)graphDataProvider ).TryGetGraphData(
                out sGraphDataAsGraphML) )
            {
                return (false);
            }

            graph = oGraphMLGraphAdapter.LoadGraphFromString(
                sGraphDataAsGraphML);
        }

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetFilesInPlugInFolder()
    //
    /// <summary>
    /// Attempts to get the full paths to the files in the folder where plug-in
    /// assemblies are stored.
    /// </summary>
    ///
    /// <param name="oFileNames">
    /// Where the full paths get stored if true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the full paths were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetFilesInPlugInFolder
    (
        out IEnumerable<String> oFileNames
    )
    {
        oFileNames = null;

        String sPlugInFolder = ApplicationUtil.GetPlugInFolder();

        if ( !Directory.Exists(sPlugInFolder) )
        {
            return (false);
        }

        List<String> oFileNameList = new List<String>();

        foreach ( String sSearchPattern in new String[] {"*.dll", "*.exe"} )
        {
            oFileNameList.AddRange( Directory.GetFiles(
                sPlugInFolder, sSearchPattern) );
        }

        oFileNames = oFileNameList;

        return (true);
    }

    //*************************************************************************
    //  Method: TryGetTypesFromFile()
    //
    /// <summary>
    /// Attempts to get an array of types implemented by an assembly.
    /// </summary>
    ///
    /// <param name="sPath">
    /// Full path to a file that might be an assembly.
    /// </param>
    ///
    /// <param name="aoTypes">
    /// Where the array of types implemented by the assembly gets stored if
    /// true is returned.
    /// </param>
    ///
    /// <returns>
    /// true if the types were obtained.
    /// </returns>
    //*************************************************************************

    private static Boolean
    TryGetTypesFromFile
    (
        String sPath,
        out Type [] aoTypes
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(sPath) );

        aoTypes = null;

        Assembly oAssembly;

        try
        {
            oAssembly = Assembly.LoadFile(sPath);
        }
        catch (FileLoadException)
        {
            return (false);
        }
        catch (BadImageFormatException)
        {
            return (false);
        }

        try
        {
            aoTypes = oAssembly.GetTypes();
        }
        catch (ReflectionTypeLoadException)
        {
            // This occurs when the loaded assembly has dependencies in an
            // assembly that hasn't been loaded.

            return (false);
        }

        return (true);
    }
}
}
