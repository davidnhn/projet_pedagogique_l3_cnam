using Godot;

/// <summary>
/// Classe singleton gérant l'état global du combat.
/// Cette classe était initialement prévue pour stocker les statistiques du joueur
/// mais a été simplifiée en faveur d'une approche directe dans BattleManager.
/// 
/// Note: Cette classe n'est plus utilisée dans le système de combat actuel.
/// Les stats du joueur sont maintenant gérées directement dans BattleManager.
/// </summary>
public partial class BattleState : Node
{
    /// <summary>
    /// Instance unique de BattleState (pattern Singleton).
    /// Permet d'accéder aux données de combat depuis n'importe où dans le jeu.
    /// </summary>
    public static BattleState Instance { get; private set; }
    
    // ===== STATISTIQUES DU JOUEUR =====
    
    /// <summary>
    /// Points de vie actuels du joueur pendant le combat.
    /// </summary>
    public int CurrentHealth { get; set; } = 35;
    
    /// <summary>
    /// Points de vie maximaux du joueur.
    /// </summary>
    public int MaxHealth { get; set; } = 35;
    
    /// <summary>
    /// Dégâts infligés par le joueur lors de ses attaques.
    /// </summary>
    public int Damage { get; set; } = 30;
    
    /// <summary>
    /// Appelé quand le nœud entre dans l'arbre de scène.
    /// Implémente le pattern Singleton en s'assurant qu'une seule instance existe.
    /// </summary>
    public override void _EnterTree()
    {
        if (Instance == null)
        {
            // Première instance, la conserver
            Instance = this;
        }
        else
        {
            // Instance déjà existante, supprimer celle-ci
            QueueFree();
        }
    }
}
