using System;
using System.Data.SqlClient;

public class RemoveVillain
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private const string NoSuchVillainMessage = "No such villain was found.";
    private const string DeletedVillainMessage = "{0} was deleted.";
    private const string ReleasedMinionsMessage = "{0} minions were released.";

    private static string GetVillainName(SqlConnection connection, int villainId)
    {
        SqlCommand getVilainNameCommand = new SqlCommand(
            $@"SELECT [Name] AS VillainName
                FROM Villains
               WHERE Id = {villainId}",
            connection);

        string villainName = (string) getVilainNameCommand.ExecuteScalar();

        return villainName;
    }

    private static int ReleaseMinions(SqlConnection connection, int villainId)
    {
        SqlCommand releaseMinionsCommand = new SqlCommand(
            $@"DELETE FROM MinionsVillains 
                WHERE VillainId = {villainId}",
            connection);

        int releasedMinionsCount = releaseMinionsCommand.ExecuteNonQuery();

        return releasedMinionsCount;
    }

    private static void DeleteVillain(SqlConnection connection, int villainId)
    {
        SqlCommand deleteVillainCommand = new SqlCommand(
            $@"DELETE FROM Villains
                WHERE Id = {villainId}",
            connection);

        deleteVillainCommand.ExecuteNonQuery();
    }

    public static void Main()
    {
        int villainId = int.Parse(Console.ReadLine());

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            string villainName = GetVillainName(connection, villainId);

            if (villainName == null)
            {
                Console.WriteLine(NoSuchVillainMessage);

                return;
            }

            int releasedMinionsCount = ReleaseMinions(connection, villainId);

            DeleteVillain(connection, villainId);

            Console.WriteLine(DeletedVillainMessage, villainName);
            Console.WriteLine(ReleasedMinionsMessage, releasedMinionsCount);
        }
    }
}