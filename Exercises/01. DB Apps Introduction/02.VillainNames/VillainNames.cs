using System;
using System.Data.SqlClient;

public class VillainNames
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    public static void Main()
    {
        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            SqlCommand createCountriesTableCommand = new SqlCommand(
                @"SELECT v.[Name] AS VillainName,
                         COUNT(mv.MinionId) AS MinionsCount
                    FROM Villains AS v
                         JOIN MinionsVillains AS mv
                         ON mv.VillainId = v.Id
                   GROUP BY v.Id,
                         V.[Name]
                  HAVING COUNT(mv.MinionId) > 3
                   ORDER BY MinionsCount DESC",
                connection);

            SqlDataReader reader = createCountriesTableCommand.ExecuteReader();

            using (reader)
            {
                while (reader.Read())
                {
                    string villainName = (string) reader["VillainName"];
                    int minionsCount = (int) reader["MinionsCount"];

                    Console.WriteLine($"{villainName} - {minionsCount}");
                }
            }
        }
    }
}