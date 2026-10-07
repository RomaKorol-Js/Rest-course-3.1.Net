namespace Domain.Card
{
    public class Card
    {
        public Guid Id { get; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public int Attack { get; private set; }
        public int Cost { get; private set; }
        public int Health { get; private set; }
        public DateTime CreatedAt { get; }


        private Card(Guid id, string name, string description,int cost, int attack, int health, DateTime createdAt) 
        => (Id,Name,Description,Cost,Attack,Health,CreatedAt) = (id,name,description,cost,attack,health, createdAt);
 
        public static Card New(Guid id,string name,string description,int cost,int attack,int health) 
        => new(id,name,description,cost,attack,health, DateTime.UtcNow);



        public void UpdateDetails(string name, string description,int cost, int attack, int health)
         => (Name, Description, Cost, Attack, Health) = (name, description, cost, attack, health);
    }
}