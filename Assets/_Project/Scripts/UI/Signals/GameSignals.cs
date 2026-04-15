// Сигнал, який просто кричить "Натиснуто Tab!"
public struct InputTabletToggleSignal { }

// Сигнал, який каже всій грі "Планшет тепер відкритий (або закритий)"
public struct TabletStateChangedSignal
{
    public bool IsOpen { get; }
    public TabletStateChangedSignal(bool isOpen) { IsOpen = isOpen; }
}

// Сигнал для майбутнього HUD (гроші)
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