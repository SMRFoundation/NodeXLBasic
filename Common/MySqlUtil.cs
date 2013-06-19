using System;
using System.Data;
using MySql.Data;
using MySql.Data.MySqlClient;
using System.Diagnostics;

namespace Smrf.AppLib
{
//*****************************************************************************
//  Class: MySqlUtil
//
/// <summary>
/// Utility methods for working with MySql databases.
/// </summary>
///
/// <remarks>
/// All methods are static.
/// </remarks>
//*****************************************************************************

public static class MySqlUtil
{
    //*************************************************************************
    //  Method: CallStoredProcedure()
    //
    /// <summary>
    /// Calls a MySql stored procedure.
    /// </summary>
    ///
    /// <param name="connectionString">
    /// Database connection string.
    /// </param>
    ///
    /// <param name="storedProcedureName">
    /// Name of the stored procedure.
    /// </param>
    ///
    /// <param name="nameValuePairs">
    /// Zero or stored procedure parameter name/value pairs.  Each parameter
    /// name must start with "@".  A parameter value can be null.
    /// </param>
    //*************************************************************************

    public static void
    CallStoredProcedure
    (
        String connectionString,
        String storedProcedureName,
        params Object [] nameValuePairs
    )
    {
        Debug.Assert( !String.IsNullOrEmpty(connectionString) );
        Debug.Assert( !String.IsNullOrEmpty(storedProcedureName) );
        Debug.Assert(nameValuePairs.Length % 2 == 0);

        using ( MySqlConnection connection =
            new MySqlConnection(connectionString) )
        {
            connection.Open();

            MySqlCommand command = new MySqlCommand(
                storedProcedureName, connection);

            command.CommandType = CommandType.StoredProcedure;

            MySqlParameterCollection parameters = command.Parameters;

            for (Int32 i = 0; i < nameValuePairs.Length; i += 2)
            {
                Debug.Assert(nameValuePairs[i + 0] is String);

                String name = (String)nameValuePairs[i + 0];

                Debug.Assert( !String.IsNullOrEmpty(name) );
                Debug.Assert(name[0] == '@');

                Object value = nameValuePairs[i + 1];

                if (value == null)
                {
                    // You cannot set a MySqlParameter to null.  It must be
                    // DBNull.Value.

                    value = DBNull.Value;
                }

                parameters.AddWithValue(name, value);
            }

            command.ExecuteNonQuery();
        }
    }
}

}
