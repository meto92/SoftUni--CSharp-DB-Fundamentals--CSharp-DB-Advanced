using System.Data.SqlClient;

public class InitialSetup
{
    private const string InitialConnectionString = 
        "Server=(LocalDB)\\MSSQLLocalDB;" +
        "Database=master;" +
        "Integrated Security=true";
    private const string ConnectionString =
        "Server=(LocalDB)\\MSSQLLocalDB;" +
        "Database=MinionsDB;" +
        "Integrated Security=true";

    private static void CreateTables(SqlConnection connection)
    {
        SqlCommand createCountriesTableCommand = new SqlCommand(
            "CREATE TABLE Countries (" +
            "    Id     INT PRIMARY KEY IDENTITY," +
            "    [Name] VARCHAR(50)" +
            ")",
            connection);
        SqlCommand createTownsTableCommand = new SqlCommand(
            "CREATE TABLE Towns (" +
            "    Id          INT PRIMARY KEY IDENTITY," +
            "    [Name]      VARCHAR(50)," +
            "    CountryCode INT FOREIGN KEY REFERENCES Countries(Id)" +
            ")",
            connection);
        SqlCommand createMinionsTableCommand = new SqlCommand(
            "CREATE TABLE Minions (" +
            "    Id     INT PRIMARY KEY IDENTITY," +
            "    [Name] VARCHAR(30)," +
            "    Age    INT," +
            "    TownId INT FOREIGN KEY REFERENCES Towns(Id)" +
            ")",
            connection);
        SqlCommand createEvilnessFactorsTableCommand = new SqlCommand(
           "CREATE TABLE EvilnessFactors (" +
           "    Id     INT PRIMARY KEY IDENTITY," +
           "    [Name] VARCHAR(50)" +
           ")",
           connection);
        SqlCommand createVillainsTableCommand = new SqlCommand(
           "CREATE TABLE Villains (" +
           "    Id     INT PRIMARY KEY IDENTITY," +
           "    [Name] VARCHAR(50)," +
           "    EvilnessFactorId INT FOREIGN KEY REFERENCES EvilnessFactors(Id)" +
           ")",
           connection);
        SqlCommand createMinionsVillainsTableCommand = new SqlCommand(
           "CREATE TABLE MinionsVillains (" +
           "    MinionId  INT FOREIGN KEY REFERENCES Minions(Id)," +
           "    VillainId INT FOREIGN KEY REFERENCES Villains(Id)," +
           "    CONSTRAINT PK_MinionsVillains PRIMARY KEY (MinionId, VillainId)" +
           ")",
           connection);

        createCountriesTableCommand.ExecuteNonQuery();
        createTownsTableCommand.ExecuteNonQuery();
        createMinionsTableCommand.ExecuteNonQuery();
        createEvilnessFactorsTableCommand.ExecuteNonQuery();
        createVillainsTableCommand.ExecuteNonQuery();
        createMinionsVillainsTableCommand.ExecuteNonQuery();
    }

    private static void InsertRecords(SqlConnection connection)
    {
        SqlCommand insertIntoCountriesCommand = new SqlCommand(
            @"INSERT INTO Countries
                     ([Name])
              VALUES ('Bulgaria'),
                     ('England'),
                     ('Cyprus'),
                     ('Germany'),
                     ('Norway')",
            connection);
        SqlCommand insertIntoTownsCommand = new SqlCommand(
            @"INSERT INTO Towns
                     ([Name], CountryCode)
              VALUES ('Plovdiv', 1),
                     ('Varna', 1),
                     ('Burgas', 1),
                     ('Sofia', 1),
                     ('London', 2),
                     ('Southampton', 2),
                     ('Bath', 2),
                     ('Liverpool', 2),
                     ('Berlin', 3),
                     ('Frankfurt', 3),
                     ('Oslo', 4)",
            connection);
        SqlCommand insertIntoMinionsCommand = new SqlCommand(
            @"INSERT INTO Minions
                     ([Name], Age, TownId)
              VALUES ('Bob', 42, 3),
                     ('Kevin', 1, 1),
                     ('Bob', 32, 6),
                     ('Simon', 45, 3),
                     ('Cathleen', 11, 2),
                     ('Carry', 50, 10),
                     ('Becky', 125, 5),
                     ('Mars', 21, 1),
                     ('Misho', 5, 10),
                     ('Zoe', 125, 5),
                     ('Json', 21, 1)",
            connection);
        SqlCommand insertIntoEvilnessFactorsCommand = new SqlCommand(
           @"INSERT INTO EvilnessFactors
                    ([Name])
             VALUES ('Super good'),
                    ('Good'),
                    ('Bad'),
                    ('Evil'),
                    ('Super evil')",
           connection);
        SqlCommand insertIntoVillainsCommand = new SqlCommand(
            @"INSERT INTO Villains
                     ([Name], EvilnessFactorId)
              VALUES ('Gru', 2),
                     ('Victor', 1),
                     ('Jilly', 3),
                     ('Miro', 4),
                     ('Rosen', 5),
                     ('Dimityr', 1),
                     ('Dobromir', 2)",
           connection);
        SqlCommand insertIntoMinionsVillainsCommand = new SqlCommand(
           @"INSERT INTO MinionsVillains
                    (MinionId, VillainId)
             VALUES (4, 2),
                    (1, 1),
                    (5, 7),
                    (3, 5),
                    (2, 6),
                    (11, 5),
                    (8, 4),
                    (9, 7),
                    (7, 1),
                    (1, 3),
                    (7, 3),
                    (5, 3),
                    (4, 3),
                    (1, 2),
                    (2, 1),
                    (2, 7)",
           connection);

        insertIntoCountriesCommand.ExecuteNonQuery();
        insertIntoTownsCommand.ExecuteNonQuery();
        insertIntoMinionsCommand.ExecuteNonQuery();
        insertIntoEvilnessFactorsCommand.ExecuteNonQuery();
        insertIntoVillainsCommand.ExecuteNonQuery();
        insertIntoMinionsVillainsCommand.ExecuteNonQuery();
    }

    public static void Main()
    {
        using (SqlConnection initialConnection = new SqlConnection(InitialConnectionString))
        {
            initialConnection.Open();

            SqlCommand createDbCommand = new SqlCommand(
                "CREATE DATABASE MinionsDB",
                initialConnection);

            createDbCommand.ExecuteNonQuery();
        }

        using (SqlConnection connection = new SqlConnection(ConnectionString))
        {
            connection.Open();

            CreateTables(connection);
            InsertRecords(connection);
        }
    }
}