using System;
using UnityEngine;
using Zenject;

public class CursorController : IInitializable, IDisposable
{
    private readonly SignalBus _signalBus;

    public CursorController(SignalBus signalBus) => _signalBus = signalBus;

    public void Initialize() => _signalBus.Subscribe<TabletStateChangedSignal>(OnTabletStateChanged);
    public void Dispose() => _signalBus.Unsubscribe<TabletStateChangedSignal>(OnTabletStateChanged);

    private void OnTabletStateChanged(TabletStateChangedSignal signal)
    {
        Cursor.visible = signal.IsOpen;
        Cursor.lockState = signal.IsOpen ? CursorLockMode.None : CursorLockMode.Locked;
    }
}