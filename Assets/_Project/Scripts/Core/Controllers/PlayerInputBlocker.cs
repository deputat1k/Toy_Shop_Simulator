using System;
using Zenject;
using ToyShop.Core.Interfaces;

public class PlayerInputBlocker : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private readonly IPlayerController _player;

    public PlayerInputBlocker(SignalBus signalBus, IPlayerController player)
    {
        _signalBus = signalBus;
        _player = player;
    }

    public void Initialize() => _signalBus.Subscribe<TabletStateChangedSignal>(OnTabletStateChanged);
    public void Dispose() => _signalBus.Unsubscribe<TabletStateChangedSignal>(OnTabletStateChanged);

    private void OnTabletStateChanged(TabletStateChangedSignal signal)
    {
        if (signal.IsOpen) _player.DisableInput();
        else _player.EnableInput();
    }
}