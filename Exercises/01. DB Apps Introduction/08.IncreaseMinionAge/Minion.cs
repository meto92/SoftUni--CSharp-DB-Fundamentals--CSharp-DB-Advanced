public class Minion
{
    public Minion(string name, int age)
        : this(0, name, age)
    { }

    public Minion(int id, string name, int age)
    {
        this.Id = id;
        this.Name = name;
        this.Age = age;
    }

    public int Id { get; set; }

    public string Name { get; set; }

    public int Age { get; set; }

    public override string ToString()
    {
        return $"{this.Name} {this.Age}";
    }
}