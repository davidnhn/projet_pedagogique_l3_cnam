using Godot;

[Tool]
public partial class QuestItem : Area2D
{
	private Sprite2D _sprite2d;
	private Texture2D _itemIcon;

	[Export]
	public string ItemId { get; set; } = "";
	
	[Export]
	public int ItemQuantity { get; set; } = 1;

	[Export]
	public Texture2D ItemIcon
	{
		get => _itemIcon;
		set
		{
			_itemIcon = value;
			UpdateSpriteTexture();
		}
	}

	public override void _Ready()
	{
		// This runs when the game starts, ensuring the texture is set.
		UpdateSpriteTexture();
	}

	private void UpdateSpriteTexture()
	{
		// Lazily get the node in case this is called by the setter
		// before _Ready() has run in the editor.
		if (_sprite2d == null)
		{
			_sprite2d = GetNodeOrNull<Sprite2D>("Sprite2D");
		}
		
		if (_sprite2d != null)
		{
			_sprite2d.Texture = _itemIcon;
		}
	}

	private void start_interact(){
		GD.Print("Interacting with item: ", ItemId);
	}
}
