using Godot;

/// <summary>
/// Classe de ressource définissant les propriétés d'un ennemi dans le système de combat.
/// Cette classe est utilisée pour créer des ressources (.tres) configurables dans l'éditeur Godot.
/// </summary>
[GlobalClass]
public partial class BaseEnemy : Resource
{
    /// <summary>
    /// Nom de l'ennemi affiché dans l'interface et les messages de combat.
    /// </summary>
    [Export]
    public string EnemyName { get; set; } = "Enemy";
    
    /// <summary>
    /// Texture/sprite de l'ennemi affiché à l'écran pendant le combat.
    /// Doit être assignée dans l'éditeur Godot pour que l'ennemi soit visible.
    /// </summary>
    [Export]
    public Texture2D EnemyTexture { get; set; } = null;
    
    /// <summary>
    /// Points de vie maximaux de l'ennemi.
    /// Détermine la longueur de la barre de vie et la difficulté du combat.
    /// </summary>
    [Export]
    public int Health { get; set; } = 30;
    
    /// <summary>
    /// Dégâts infligés par l'ennemi au joueur lors de son tour d'attaque.
    /// Plus cette valeur est élevée, plus le combat est difficile.
    /// </summary>
    [Export]
    public int Damage { get; set; } = 20;
}
