using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InteractingComponent : Node2D
{
	private Label InteractLabel;
	private List<Area2D> CurrentInteractions = [];
	private bool CanInteract = true;

	public override void _Ready()
	{
		InteractLabel = GetNode<Label>("InteractLabel");
	}

    public override void _Input(InputEvent @event)
    {
        if(@event.IsActionPressed("interact") && CanInteract && CurrentInteractions.Count != 0)
		{
			InteractLabel.Hide();

			var CurrentInteraction = CurrentInteractions.First() as Interactable;
			CurrentInteraction.Interact.Invoke();
		}
    }

	public override void _Process(double delta)
	{
		if (CurrentInteractions.Any() && CanInteract)
		{
			//sort interactions areas by their distance
			CurrentInteractions.Sort(
				(Area2D area1, Area2D area2) =>
				{
					float area1Dist = GlobalPosition.DistanceTo(area1.GlobalPosition);
					float area2Dist = GlobalPosition.DistanceTo(area2.GlobalPosition);

					return area1Dist.CompareTo(area2Dist);
				}
			);

			var FirstInteraction = CurrentInteractions.First() as Interactable;


			if (FirstInteraction.IsInteractable)
			{
				InteractLabel.Text = FirstInteraction.InteractName;
				InteractLabel.Show();
			}
		}
		else
			InteractLabel.Hide();
	}

	private void OnAreaEntered(Area2D area)
	{
		CurrentInteractions.Add(area);
	}

	private void OnAreaExited(Area2D area) 
	{
		CurrentInteractions.Remove(area);
	}

}
