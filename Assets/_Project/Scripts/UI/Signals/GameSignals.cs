// The signal shouts "Tab pressed!"
public struct InputTabletToggleSignal { }

// A signal that tells the entire game "The tablet is now open (or closed)"
public struct TabletStateChangedSignal
{
    public bool IsOpen { get; }
    public TabletStateChangedSignal(bool isOpen) { IsOpen = isOpen; }
}

// A signal for the HUD (money)
public struct BalanceChangedSignal
{
    public int NewBalance { get; }
    public BalanceChangedSignal(int newBalance) { NewBalance = newBalance; }
}

public struct PurchaseResultSignal
{
    public string ToyId { get; }
    public bool Success { get; }
    public PurchaseResultSignal(string toyId, bool success)
    {
        ToyId = toyId;
        Success = success;
    }
}