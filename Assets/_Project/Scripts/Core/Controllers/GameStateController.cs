using System;
using Zenject;

public class GameStateController : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private bool _isTabletOpen = false; // this state is maintained

    public GameStateController(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }

    public void Initialize()
    {
        _signalBus.Subscribe<InputTabletToggleSignal>(OnTabletToggleRequested);
    }

    public void Dispose()
    {
        _signalBus.Unsubscribe<InputTabletToggleSignal>(OnTabletToggleRequested);
    }

    private void OnTabletToggleRequested()
    {
        _isTabletOpen = !_isTabletOpen; // Changing the state

        // Notifying the entire game about a state change
        _signalBus.Fire(new TabletStateChangedSignal(_isTabletOpen));
    }
}