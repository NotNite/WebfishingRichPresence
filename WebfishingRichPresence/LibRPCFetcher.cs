using System.Reflection;
using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace WebfishingRichPresence;

public class LibRpcFetcher : IScriptMod {
    public bool ShouldRun(string path) => path == "res://mods/WebfishingRichPresence/main.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        foreach (var token in tokens) {
            if (token is ConstantToken {Value: StringVariant {Value: "%LIBRPCPATH%"} str}) {
                str.Value = Path.Combine(
                    Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!,
                    "librpc.dll"
                );
            }

            yield return token;
        }
    }
}
