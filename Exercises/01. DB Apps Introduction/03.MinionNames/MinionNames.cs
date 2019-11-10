using System;
using System.Text;
using System.Linq;
using System.Data.SqlClient;
using System.Collections.Generic;

public class MinionNames
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private const string NoVillainMessage = "No villain with ID {0} exists in the database.";
    private const string VillainInfoPattern = "Villain: {0}";
    private const string NoMinionsMessage = "(no minions)";

    private static string GetVillainName(SqlConnection connection, int villainId)
    {
        SqlCommand getVillainNameCommand = new SqlCommand(
            $@"SELECT [Name] AS VillainName
                 FROM Villains
                WHERE Id = {villainId}",
            connection);

        string villainName = (string) getVillainNameCommand.ExecuteScalar();

        return villainName;
    }

    private static void AppendMinionsInfo(SqlConnection connection, int villainId, StringBuilder result)
    {
        SqlCommand getMinionsCommand = new SqlCommand(
            $@"SELECT m.[Name] AS MinionName,
               	      m.Age AS MinionAge
                 FROM Minions AS m
               	      JOIN MinionsVillains AS mv
               	      ON mv.MinionId = m.Id
                WHERE mv.VillainId = {villainId}
                ORDER BY MinionName",
            connection);

        List<Minion> minions = new List<Minion>();

        SqlDataReader reader = getMinionsCommand.ExecuteReader();

        using (reader)
        {
            while (reader.Read())
            {
                string minionName = (string) reader["MinionName"];
                int minionage = (int) reader["Minionage"];

                Minion minion = new Minion(minionName, minionage);

                minions.Add(minion);
            }
        }

        if (minions.Count == 0)
        {
            result.AppendLine(NoMinionsMessage);
        }
        else
        {
            minions.Select((minion, index) => $"{index + 1}. {minion}")
                .ToList()
                .ForEach(m => result.AppendLine(m));
        }
    }

    public static void Main()
    {
        int villainId = int.Parse(Console.ReadLine());

        StringBuilder result = new StringBuilder();

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string villainName = GetVillainName(connection, villainId);

            if (villainName == null)
            {
                Console.WriteLine(NoVillainMessage, villainId);

                return;
            }

            result.AppendFormat(VillainInfoPattern, villainName);
            result.AppendLine();

            AppendMinionsInfo(connection, villainId, result);
        }

        Console.Write(result);
    }
}