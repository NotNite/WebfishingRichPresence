using GDWeave;

namespace WebfishingRichPresence;

public class Mod : IMod {
    public Mod(IModInterface modInterface) {
        modInterface.RegisterScriptMod(new LibRpcFetcher());
        modInterface.RegisterScriptMod(new RichPresenceForwarder());
    }

    public void Dispose() { }
}
