using Godot;

public partial class MusicManager : AudioStreamPlayer
{
    public override void _Ready()
    {
        // Démarre la musique seulement si elle n'est pas déjà en cours
        if (!Playing)
            Play();
    }
} 