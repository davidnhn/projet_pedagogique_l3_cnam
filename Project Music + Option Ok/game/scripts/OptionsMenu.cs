using Godot;
using System;

public partial class OptionsMenu : Control
{
    private Slider volumeSlider;
    private CheckBox muteCheckBox;
    private float lastVolumeDb = 0;
    private Button returnButton;

    public override void _Ready()
    {
        // Récupère le slider Volume
        volumeSlider = GetNode<Slider>("MarginContainer/VBoxContainer/Volume");
        if (volumeSlider != null)
        {
            volumeSlider.ValueChanged += OnVolumeValueChanged;

            // Synchronise le slider avec le volume actuel du bus principal
            float currentDb = AudioServer.GetBusVolumeDb(0);
            float sliderValue;
            if (currentDb <= -40)
                sliderValue = 0;
            else
                sliderValue = (float)(Math.Pow(10, (currentDb / 40.0)) * 100.0);
            volumeSlider.Value = sliderValue;

            // Récupère la CheckBox Mute
            muteCheckBox = GetNode<CheckBox>("MarginContainer/VBoxContainer/CheckBox");
            if (muteCheckBox != null)
            {
                muteCheckBox.Toggled += OnMuteToggled;
                // Synchronise l'état de la case à cocher avec le volume
                muteCheckBox.ButtonPressed = (currentDb <= -79);
            }
            else
            {
                GD.PrintErr("Mute CheckBox not found!");
            }
        }
        else
        {
            GD.PrintErr("Volume slider not found!");
        }

        returnButton = GetNode<Button>("MarginContainer/VBoxContainer/Return_Button");
        if (returnButton != null)
        {
            returnButton.Pressed += OnReturnButtonPressed;
            // Définit une taille minimale personnalisée pour le bouton Return
            returnButton.CustomMinimumSize = new Vector2(200, 40);
        }
        else
        {
            GD.PrintErr("Return_Button not found!");
        }
    }

    private void OnVolumeValueChanged(double value)
    {
        // Valeur du slider entre 0 et 100
        float linearValue = (float)value / 100f;
        // On évite le log(0) qui donne -inf
        if (linearValue < 0.01f)
            AudioServer.SetBusVolumeDb(0, -80);
        else
            AudioServer.SetBusVolumeDb(0, Mathf.Lerp(-40, 0, (float)Math.Log10(linearValue) + 1));
    }

    private void OnMuteToggled(bool buttonPressed)
    {
        if (buttonPressed)
        {
            // Mémorise le volume actuel et coupe le son
            lastVolumeDb = AudioServer.GetBusVolumeDb(0);
            AudioServer.SetBusVolumeDb(0, -80);
        }
        else
        {
            // Restaure le volume précédent
            AudioServer.SetBusVolumeDb(0, lastVolumeDb);
        }
    }

    private void OnReturnButtonPressed()
    {
        GetTree().ChangeSceneToFile("res://scenes/main_menu.tscn");
    }
}
