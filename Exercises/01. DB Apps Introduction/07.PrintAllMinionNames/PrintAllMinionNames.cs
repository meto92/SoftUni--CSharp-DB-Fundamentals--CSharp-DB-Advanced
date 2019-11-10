using System;
using System.Data.SqlClient;
using System.Collections.Generic;

public class PrintAllMinionNames
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private static List<string> GetMinionNames(SqlConnection connection)
    {
        List<string> minionNames = new List<string>();

        SqlCommand getMinionNamesCommand = new SqlCommand(
            @"SELECT [Name] AS MinionName
                FROM Minions",
            connection);

        SqlDataReader reader = getMinionNamesCommand.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                string minionName = (string) reader["MinionName"];

                minionNames.Add(minionName);
            }
        }

        return minionNames;
    }

    public static void Main()
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            List<string> minionNames = GetMinionNames(connection);


            for (int i = 0; i < minionNames.Count / 2; i++)
            {
                Console.WriteLine(minionNames[i]);
                Console.WriteLine(minionNames[minionNames.Count - 1 - i]);
            }

            if (minionNames.Count % 2 == 1)
            {
                Console.WriteLine(minionNames[minionNames.Count / 2]);
            }
        }
    }
}