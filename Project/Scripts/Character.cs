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
	[Export] public NodePath TileMapLayerPath { get; set; }
	[Export] public float InteractDistance { get; set; } = 16f;      
	[Export] public string InteractAction { get; set; } = "interact";
	[Export] public string InventoryScenePath { get; set; } = "res://scenes/inventory.tscn";
	[Export] public NodePath UiLayerPath { get; set; }              
	[Export] public NodePath AnimatedSpritePath { get; set; }
	[Export] public NodePath IconPath { get; set; }
	[Export] public NodePath AmountPath { get; set; }
	[Export] public NodePath QuestTrackerPath { get; set; }
	[Export] public NodePath TitlePath { get; set; }
	[Export] public NodePath ObjectivesPath { get; set; }

	private TileMapLayer _tilemapLayer;
	private CanvasLayer _uiLayer;
	private Control _inventoryInstance;
	private AnimatedSprite2D _animatedSprite;
	private RayCast2D _raycast;
	private Vector2 _lookDir = Vector2.Down;
	private TextureRect _icon;
	private Label _amount;
	private Control _questTracker;
	private Label _title;
	private VBoxContainer _objectives;
	private bool _canMove = true;

	public override void _Ready()
	{
		Health = MaxHealth;

		if (TileMapLayerPath != null && !TileMapLayerPath.IsEmpty)
			_tilemapLayer = GetNodeOrNull<TileMapLayer>(TileMapLayerPath);

		if (UiLayerPath != null && !UiLayerPath.IsEmpty)
			_uiLayer = GetNodeOrNull<CanvasLayer>(UiLayerPath);
			
		if (AnimatedSpritePath != null && !AnimatedSpritePath.IsEmpty)
			_animatedSprite = GetNodeOrNull<AnimatedSprite2D>(AnimatedSpritePath);

		_raycast = GetNodeOrNull<RayCast2D>("RayCast2D");

		if (IconPath != null && !IconPath.IsEmpty)
			_icon = GetNodeOrNull<TextureRect>(IconPath);

		if (AmountPath != null && !AmountPath.IsEmpty)
			_amount = GetNodeOrNull<Label>(AmountPath);

		if (QuestTrackerPath != null && !QuestTrackerPath.IsEmpty)
		{
			_questTracker = GetNodeOrNull<Control>(QuestTrackerPath);
			if (_questTracker != null)
				_questTracker.Visible = false;
		}

		if (TitlePath != null && !TitlePath.IsEmpty)
			_title = GetNodeOrNull<Label>(TitlePath);
			
		if (ObjectivesPath != null && !ObjectivesPath.IsEmpty)
			_objectives = GetNodeOrNull<VBoxContainer>(ObjectivesPath);

		Global.Player = this;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (!IsAlive || GetTree().Paused || !_canMove)
		{
			Velocity = Vector2.Zero;
		}
		else
		{
			GetInput();
		}
		
		MoveAndSlide();
		UpdateAnimation();
	}

	private void GetInput()
	{
		Vector2 inputDirection = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
		Velocity = inputDirection.Normalized() * Speed;

		if (Velocity != Vector2.Zero && _raycast != null)
		{
			_raycast.TargetPosition = Velocity.Normalized() * InteractDistance;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed(InteractAction) && IsAlive && !GetTree().Paused)
			TryInteract();
	}


	private void UpdateAnimation()
	{
		if (_animatedSprite == null) return;

		if (Velocity == Vector2.Zero)
		{
			_animatedSprite.Play("idle");
		}
		else
		{
			if (Mathf.Abs(Velocity.X) > Mathf.Abs(Velocity.Y))
			{
				if (Velocity.X > 0)
				{
					_animatedSprite.Play("walk_right");
				}
				else
				{
					_animatedSprite.Play("walk_left");
				}
			}
			else
			{
				if (Velocity.Y > 0)
				{
					_animatedSprite.Play("walk_down");
				}
				else
				{
					_animatedSprite.Play("walk_up");
				}
			}
		}
	}
	
	private void TryInteract()
	{
		// Raycast interaction logic
		if (_raycast != null && _raycast.IsColliding())
		{
			var collider = _raycast.GetCollider();

			switch (collider)
			{
				case Npc npc when npc.IsInGroup("NPC"):
					GD.Print($"Interacting with an NPC: {npc.npc_name}");
					// Future logic to start dialog can go here.
					return;

				case QuestItem questItem when questItem.IsInGroup("QuestItem"):
					GD.Print($"Interacting with an Item: {questItem.ItemId}");
					// Future logic to pick up item can go here.
					return;

				case Enemy enemy when enemy.IsInGroup("Enemy"):
					GD.Print($"Interacting with an Enemy: {enemy.Name}");
					// Future logic to start combat or talk can go here.
					return;
			}
		}
		
		// Fallback to tile-based interaction
		if (_tilemapLayer != null)
		{
			Vector2I cell = _tilemapLayer.LocalToMap(GlobalPosition);
			var tileData = _tilemapLayer.GetCellTileData(cell);
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
