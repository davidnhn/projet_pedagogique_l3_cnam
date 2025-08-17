using Godot;
using System;

public partial class Character : CharacterBody2D
{
	// --- Tes propriétés d'origine ---
	[Export] public float Speed { get; protected set; } = 200.0f;
	[Export] public int MaxHealth { get; protected set; } = 100;
	[Export] public int Health { get; protected set; }
	[Export] public int Attack { get; protected set; } = 10;
	[Export] public int Defense { get; protected set; } = 5;

	public bool IsAlive { get; protected set; } = true;
	public string SpritePath { get; protected set; }

	// --- Ajouts pour l'interaction & UI ---
	[Export] public NodePath TileMapPath { get; set; }
	[Export] public int TileLayer { get; set; } = 0; 
	[Export] public float InteractDistance { get; set; } = 16f;      
	[Export] public string InteractAction { get; set; } = "interact";
	[Export] public string InventoryScenePath { get; set; } = "res://scenes/inventory.tscn";
	[Export] public NodePath UiLayerPath { get; set; }              

	private TileMap _tilemap;
	private CanvasLayer _uiLayer;
	private Control _inventoryInstance;

	private Vector2 _lookDir = Vector2.Down;

	public override void _Ready()
	{
		Health = MaxHealth;

		if (TileMapPath != null && !TileMapPath.IsEmpty)
			_tilemap = GetNodeOrNull<TileMap>(TileMapPath);

		if (UiLayerPath != null && !UiLayerPath.IsEmpty)
			_uiLayer = GetNodeOrNull<CanvasLayer>(UiLayerPath);
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive || GetTree().Paused)
		{
			Velocity = Vector2.Zero;
			MoveAndSlide();
			return;
		}

		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		if (inputDirection != Vector2.Zero)
			_lookDir = inputDirection.Normalized(); 

		Velocity = inputDirection.Normalized() * Speed;
		MoveAndSlide();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(InteractAction) && IsAlive && !GetTree().Paused)
			TryInteract();
	}


	private void TryInteract()
	{
		if (_tilemap != null)
		{
			Vector2 forward = GlobalPosition + (_lookDir == Vector2.Zero ? Vector2.Down : _lookDir) * InteractDistance;

			Vector2I cell = _tilemap.LocalToMap(_tilemap.ToLocal(forward));
			var tileData = _tilemap.GetCellTileData(TileLayer, cell);
			if (tileData != null)
			{
				var interactData = tileData.GetCustomData("interact");
				bool isInteract = interactData.VariantType != Variant.Type.Nil && interactData.AsBool();

				if (isInteract)
				{
					string kind = "";
					var kindData = tileData.GetCustomData("kind");
					if (kindData.VariantType != Variant.Type.Nil)
						kind = kindData.AsStringName();

					switch (kind)
					{
						case "computer":
							OpenInventory();
							return;

						default:
							GD.Print($"[Interact] Tuile interactive rencontrée, kind={kind}");
							return;
					}
				}
			}
		}
	}


	private void OpenInventory()
	{
		if (_inventoryInstance != null) return; 

		var packed = ResourceLoader.Load<PackedScene>(InventoryScenePath);
		if (packed == null)
		{
			GD.PushWarning($"Inventory scene introuvable: {InventoryScenePath}");
			return;
		}

		_inventoryInstance = packed.Instantiate<Control>();

		_inventoryInstance.ProcessMode = Node.ProcessModeEnum.Always;

		if (_uiLayer != null)
			_uiLayer.AddChild(_inventoryInstance);
		else
			AddChild(_inventoryInstance); 


		if (_inventoryInstance.HasSignal("closed"))
			_inventoryInstance.Connect("closed", new Callable(this, nameof(CloseInventory)));


		GetTree().Paused = true;
	}

	private void CloseInventory()
	{
		GetTree().Paused = false;

		if (_inventoryInstance != null && IsInstanceValid(_inventoryInstance))
			_inventoryInstance.QueueFree();

		_inventoryInstance = null;
	}

	public virtual void TakeDamage(int damage)
	{
		int actualDamage = Mathf.Max(0, damage - Defense);
		Health -= actualDamage;
		GD.Print($"{Name} took {actualDamage} damage. Health is now {Health}");

		if (Health <= 0)
		{
			Health = 0;
			IsAlive = false;
			Die();
		}
	}

	public virtual void AttackCharacter(Character target)
	{
		if (target != null && target.IsAlive)
		{
			GD.Print($"{Name} attacks {target.Name}.");
			target.TakeDamage(Attack);
		}
	}

	protected virtual void Die()
	{
		GD.Print($"{Name} has died.");
	}
}
