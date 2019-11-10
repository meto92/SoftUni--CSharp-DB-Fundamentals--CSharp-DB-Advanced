using System;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class IncreaseMinionAge
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private static string GetTitleCasing(string str)
    {
        StringBuilder result = new StringBuilder(str.ToLower());

        Regex regex = new Regex(@"\p{L}");

        result[0] = char.ToUpper(result[0]);

        for (int i = 1; i < result.Length; i++)
        {
            if (!regex.IsMatch(result[i - 1].ToString()))
            {
                result[i] = char.ToUpper(result[i]);
            }
        }

        return result.ToString();
    }

    private static List<Minion> GetMinions(SqlConnection connection, string joinedMinionIds)
    {
        List<Minion> minions = new List<Minion>();

        SqlCommand getMinionsCommand = new SqlCommand(
                $@"SELECT Id,
                   	      [Name],
                   	      Age
                     FROM Minions
                    WHERE Id IN ({joinedMinionIds})",
                connection);

        SqlDataReader reader = getMinionsCommand.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                int id = (int) reader["Id"];
                string name = (string) reader["Name"];
                int age = (int) reader["Age"];

                Minion minion = new Minion(id, name, age);

                minions.Add(minion);
            }
        }

        return minions;
    }

    private static void UpdateMinions(SqlConnection connection, List<Minion> minions)
    {
        foreach (Minion minion in minions)
        {
            SqlCommand updateMinionsCommand = new SqlCommand(
                $@"UPDATE Minions
                     SET Age += 1,
                  	     [Name] = '{GetTitleCasing(minion.Name)}'
                   WHERE Id = {minion.Id}",
                connection);

            updateMinionsCommand.ExecuteNonQuery();
        }
    }

    private static List<Minion> GetAllMinions(SqlConnection connection)
    {
        SqlCommand getMinionsCommand = new SqlCommand(
            @"SELECT Id,
               	     [Name],
               	     Age
                FROM Minions",
            connection);

        List<Minion> minions = new List<Minion>();

        SqlDataReader reader = getMinionsCommand.ExecuteReader();
        
        using (reader)
        {
            while (reader.Read())
            {
                string name = (string) reader["Name"];
                int age = (int) reader["Age"];

                Minion minion = new Minion(name, age);

                minions.Add(minion);
            }
        }

        return minions;
    }

    public static void Main()
    {
        int[] minionIds = Console.ReadLine()
            .Split()
            .Select(int.Parse)
            .ToArray();

        string joinedMinionIds = string.Join(", ", minionIds);

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            List<Minion> minions = GetMinions(connection, joinedMinionIds);

            UpdateMinions(connection, minions);

            List<Minion> allMinions = GetAllMinions(connection);

            Console.WriteLine(string.Join(Environment.NewLine, allMinions));
        }
    }
}