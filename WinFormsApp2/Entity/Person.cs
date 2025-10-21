namespace WinFormsApp2.Entity;

public class Person
{
    public int Id { get; set; }
    public string Name { get; set; }
    public int Age { get; set; }
    public DateTime Birthdate { get; set; }

    public Person(string name, DateTime birthdate)
    {
        Name = name;
        Birthdate = birthdate.Date;
        CalculateAge();
    }

    private void CalculateAge()
    {
        var today = DateTime.Today;
        var age = today.Year - Birthdate.Year;
        if (Birthdate.Date > today.AddYears(-age)) age--;
        Age = age;
    }
}