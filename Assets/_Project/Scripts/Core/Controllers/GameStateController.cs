using System;
using Zenject;

public class GameStateController : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;
    private bool _isTabletOpen = false; // ЄДИНЕ місце, де зберігається цей стан

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
        _isTabletOpen = !_isTabletOpen; // Змінюємо стан

        // Сповіщаємо всю гру про зміну стану
        _signalBus.Fire(new TabletStateChangedSignal(_isTabletOpen));
    }
}