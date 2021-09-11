namespace HamTestWasmHosted.Server.Domain
{
    public class Topic
    { 
        public string Name { get; init; }

        public Topic(string name)
        {
            Name = name;
        }
    }
}