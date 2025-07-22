using Godot;

public partial class InventoryOverlay : Panel
{
    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey keyEvent && keyEvent.Pressed && keyEvent.Keycode == Key.Escape)
        {
            QueueFree(); // Ferme l'overlay
            GetViewport().SetInputAsHandled();
        }
    }

    public override void _Ready()
    {
        // Capture l'input même si le jeu est en pause
        SetProcessInput(true);
    }
} 