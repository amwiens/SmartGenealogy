namespace SmartGenealogy.Data;

public class DatabaseInitializer
{
    private readonly PersonRepository _personRepository;

    public DatabaseInitializer(PersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public async Task LoadSeedDataAsync()
    {
        var person = new Person
        {
            Sex = 1,
            ParentID = 0
        };
        await _personRepository.SaveItemAsync(person);
    }
}