using System;
using System.Data.SqlClient;
using System.Collections.Generic;

public class ChangeTownNamesCasing
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private const string NoTownsAffectedMessage = "No town names were affected.";

    private static List<int> GetTownIds(SqlConnection connection, int countryId)
    {
        List<int> townIds = new List<int>();

        SqlCommand getTownIdsCommand = new SqlCommand(
            $@"SELECT Id
                 FROM Towns
                WHERE CountryCode = {countryId}",
            connection);

        SqlDataReader reader = getTownIdsCommand.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                int townId = (int) reader["Id"];

                townIds.Add(townId);
            }
        }

        return townIds;
    }

    private static int GetCountryId(SqlConnection connection, string countryName)
    {
        SqlCommand getCountryIdCommand = new SqlCommand(
            $@"SELECT Id
                 FROM Countries
                WHERE [Name] = '{countryName}'",
            connection);

        object idObj = getCountryIdCommand.ExecuteScalar();

        if (idObj != null)
        {
            return (int) idObj;
        }

        return 0;
    }

    private static void ChangeTownsCasing(SqlConnection connection, string joinedIds)
    {
        SqlCommand changeTownsCasingCommand = new SqlCommand(
            $@"UPDATE Towns
                  SET [Name] = UPPER([Name])
                WHERE Id IN ({joinedIds})",
            connection);

        changeTownsCasingCommand.ExecuteNonQuery();
    }

    private static List<string> GetTownNames(SqlConnection connection, string joinedIds)
    {
        List<string> townNames = new List<string>();

        SqlCommand getTownNamesCommand = new SqlCommand(
            $@"SELECT [Name] AS TownName
                 FROM Towns
                WHERE Id IN ({joinedIds})",
            connection);

        SqlDataReader reader = getTownNamesCommand.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                string townName = (string) reader["TownName"];

                townNames.Add(townName);
            }
        }

        return townNames;
    }

    private static void PrintChangedTownNames(SqlConnection connection, List<int> townIds)
    {
        string joinedIds = string.Join(", ", townIds);

        ChangeTownsCasing(connection, joinedIds);

        List<string> townNames = GetTownNames(connection, joinedIds);

        Console.WriteLine($"{townNames.Count} town names were affected.");
        Console.WriteLine($"[{string.Join(", ", townNames)}]");
    }

    public static void Main()
    {
        string countryName = Console.ReadLine();

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            int countryId = GetCountryId(connection, countryName);

            List<int> townIds = GetTownIds(connection, countryId);

            if (countryId == 0 || townIds.Count == 0)
            {
                Console.WriteLine(NoTownsAffectedMessage);
            }
            else
            {
                PrintChangedTownNames(connection, townIds);
            }            
        }
    }
}