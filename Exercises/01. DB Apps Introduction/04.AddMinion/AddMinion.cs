using System;
using System.Linq;
using System.Data.SqlClient;

public class AddMinion
{
    private const string ConnectionString =
       "Server=(LocalDB)\\MSSQLLocalDB;" +
       "Database=MinionsDB;" +
       "Integrated Security=true";

    private const string AddedNewTownMessage = "Town {0} was added to the database.";
    private const string AddedNewVillainMessage = "Villain {0} was added to the database.";
    private const string AddedMinionAsServantMessage = "Successfully added {0} to be minion of {1}.";

    private static void InsertTown(SqlConnection connection, SqlTransaction transaction, string townName)
    {
        SqlCommand insertTownCommand = new SqlCommand(
            $@"INSERT INTO Towns
                      ([Name])
               VALUES ('{townName}')", 
            connection,
            transaction);

        insertTownCommand.ExecuteNonQuery();

        Console.WriteLine(AddedNewTownMessage, townName);
    }

    private static int GetTownId(SqlConnection connection, SqlTransaction transaction, string townName)
    {
        SqlCommand getTownIdCommand = new SqlCommand(
            $@"SELECT Id
                 FROM Towns
                WHERE [Name] = '{townName}'",
            connection,
            transaction);

        int id = 0;

        try
        {
            id = (int) getTownIdCommand.ExecuteScalar();
        }
        catch (NullReferenceException)
        {
            InsertTown(connection, transaction, townName);

            id = (int) getTownIdCommand.ExecuteScalar();
        }

        return id;
    }
    
    private static void InsertVillain(SqlConnection connection, SqlTransaction transaction, string villainName)
    {
        SqlCommand insertVillainCommand = new SqlCommand(
            $@"INSERT INTO Villains
                      ([Name], EvilnessFactorId)
               VALUES ('{villainName}', (SELECT Id FROM EvilnessFactors WHERE [Name] = 'Evil'))",
            connection,
            transaction);

        insertVillainCommand.ExecuteNonQuery();

        Console.WriteLine(AddedNewVillainMessage, villainName);
    }

    private static int GetVillainId(SqlConnection connection, SqlTransaction transaction, string villainName)
    {
        SqlCommand getVillainIdCommand = new SqlCommand(
            $@"SELECT Id
                 FROM Villains
                WHERE [Name] = '{villainName}'",
            connection,
            transaction);

        int id = 0;

        try
        {
            id = (int) getVillainIdCommand.ExecuteScalar();
        }
        catch (NullReferenceException)
        {
            InsertVillain(connection, transaction, villainName);

            id = (int) getVillainIdCommand.ExecuteScalar();
        }

        return id;
    }

    private static void InsertMinion(SqlConnection connection, SqlTransaction transaction, string name, int age, int townId)
    {
        SqlCommand insertMinionCommand = new SqlCommand(
            $@"INSERT INTO Minions
               	      ([Name], Age, TownId)
               VALUES ('{name}', {age}, {townId})",
            connection,
            transaction);

        insertMinionCommand.ExecuteNonQuery();
    }

    private static int GetMinionId(SqlConnection connection, SqlTransaction transaction, string minionName)
    {
        SqlCommand getMinionIdCommand = new SqlCommand(
            $@"SELECT Id
                 FROM Minions
                WHERE [Name] = '{minionName}'
                ORDER BY Id DESC",
            connection,
            transaction);

        int id = (int) getMinionIdCommand.ExecuteScalar();

        return id;
    }

    private static void AddServant(SqlConnection connection, SqlTransaction transaction, int villainId, int minionId)
    {
        SqlCommand addServantCommand = new SqlCommand(
            $@"INSERT INTO MinionsVillains
               	      (MinionId, VillainId)
               VALUES ({minionId}, {villainId})",
            connection,
            transaction);

        addServantCommand.ExecuteNonQuery();
    }

    public static void Main()
    {
        string[] minionInfo = Console.ReadLine()
            .Split(new[] { ':', ' ' }, StringSplitOptions.RemoveEmptyEntries)
            .Skip(1)
            .ToArray();
        string villainName = Console.ReadLine().Split(": ")[1];

        string minionName = minionInfo[0];
        int minionAge = int.Parse(minionInfo[1]);
        string minionTownName = minionInfo[2];

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            SqlTransaction transaction = connection.BeginTransaction();

            try
            {
                int townId = GetTownId(connection, transaction, minionTownName);
                int villainId = GetVillainId(connection, transaction, villainName);

                InsertMinion(connection, transaction, minionName, minionAge, townId);

                int minionId = GetMinionId(connection, transaction, minionName);

                AddServant(connection, transaction, villainId, minionId);

                Console.WriteLine(AddedMinionAsServantMessage, minionName, villainName);

                transaction.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);

                transaction.Rollback();
            }
        }
    }
}