namespace FilmAI.Windows.Models;

public class CrewMember
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Role { get; set; } = string.Empty; // Yönetmen, Senarist, Müzik, Görüntü Yönetmeni...
}
