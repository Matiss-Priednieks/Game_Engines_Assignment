using Godot;
using System;

public class MainGame : Spatial
{
    AnimationPlayer transitionPlayer;
    PackedScene Lavalamp;

    int MinFreq = 20;
    int MaxFreq = 20000;
    int definition = 20;
    private AudioEffectSpectrumAnalyzerInstance fft;

    WorldEnvironment WE;

    public override void _Ready()
    {
        Lavalamp = (PackedScene)ResourceLoader.Load("res://scenes/LavaLamp.tscn");
        WE = GetNode<WorldEnvironment>("WorldEnvironment");

    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        var fft = (AudioEffectSpectrumAnalyzerInstance)AudioServer.GetBusEffectInstance(0, 0);
        var freq = MinFreq;
        var interval = (MaxFreq - MinFreq) / definition;
        var mag = fft.GetMagnitudeForFrequencyRange(freq, freq + interval);
        GD.Print(mag);
        if (mag.x >= 0.35)
        {

            WE.Environment.BackgroundEnergy = mag.x;
        }
        else
        {
            GD.Print(mag);
            WE.Environment.BackgroundEnergy = 0.05f;
        }

        if (Input.IsMouseButtonPressed(1))
        {
            // transitionPlayer = GetNode<AnimationPlayer>("%TransitionManager");
            // transitionPlayer.CurrentAnimation = "TransitionOut";
        }
        if (Input.IsActionJustReleased("side_cam"))
        {
            var cam = GetNode<Camera>("%SideCam");
            if (cam.Current == false)
            {
                cam.Current = true;
            }
        }
        else if (Input.IsActionJustReleased("topdown_cam"))
        {
            var cam = GetNode<Camera>("%TopDownCam");
            if (cam.Current == false)
            {
                cam.Current = true;
            }
        }
        else if (Input.IsActionJustReleased("player_cam"))
        {
            var cam = GetNode<Camera>("Player/PlayerCam");
            if (cam.Current == false)
            {
                cam.Current = true;
            }
        }
    }

    private void _on_TransitionManager_animation_finished(String anim)
    {
        if (anim == "TransitionOut")
        {
            GetTree().ChangeScene("res://scenes/TestScene.tscn"); //Buggy, creates a bunch of debugger errors.
        }
    }
}
