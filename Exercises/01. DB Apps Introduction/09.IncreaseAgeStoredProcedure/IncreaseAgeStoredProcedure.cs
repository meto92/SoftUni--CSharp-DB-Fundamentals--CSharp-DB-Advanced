using System;
using System.Data.SqlClient;

public class IncreaseAgeStoredProcedure
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private const string MinionInfoFormat = "{0} – {1} years old";
    private const string NoSuchMinionMessage = "No such minion was found.";

    public static void Main()
    {
        int minionId = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            SqlCommand increaseAgeCommand = new SqlCommand(
                $"EXEC usp_GetOlder {minionId}",
                connection);

            increaseAgeCommand.ExecuteNonQuery();

            SqlCommand getMinionNameCommand = new SqlCommand(
                $@"SELECT [Name]
                     FROM Minions
                    WHERE Id = {minionId}",
                connection);
            SqlCommand getMinionAgeCommand = new SqlCommand(
                $@"SELECT Age
                     FROM Minions
                    WHERE Id = {minionId}",
                connection);


            try
            {
                string minionName = (string) getMinionNameCommand.ExecuteScalar();
                int minionAge = (int) getMinionAgeCommand.ExecuteScalar();

                Console.WriteLine(MinionInfoFormat, minionName, minionAge);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine(NoSuchMinionMessage);
            }
        }
    }
}