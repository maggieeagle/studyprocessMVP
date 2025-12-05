namespace App.Domain
{
    public class Group
    {
        public int Id { get; private set; }
        public string Name { get; private set; }

        public Group(string name)
        {
            Name = name;
        }
    }
}
