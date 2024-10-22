using GDWeave.Godot;
using GDWeave.Godot.Variants;
using GDWeave.Modding;

namespace WebfishingRichPresence;

public class RichPresenceForwarder : IScriptMod {
    public bool ShouldRun(string path) => path == "res://Scenes/Singletons/SteamNetwork.gdc";

    public IEnumerable<Token> Modify(string path, IEnumerable<Token> tokens) {
        var setRichPresenceWaiter = new MultiTokenWaiter([
            t => t.Type is TokenType.PrFunction,
            t => t is IdentifierToken {Name: "set_rich_presence"}
        ]);
        var emitMembersUpdatedWaiter = new MultiTokenWaiter([
            t => t is IdentifierToken {Name: "emit_signal"},
            t => t.Type is TokenType.ParenthesisOpen,
            t => t is ConstantToken {Value: StringVariant {Value: "_members_updated"}},
            t => t.Type is TokenType.ParenthesisClose
        ]);
        var newlineWaiter = new TokenWaiter(t => t.Type is TokenType.Newline, waitForReady: true);
        var settingStatus = false;

        foreach (var token in tokens) {
            yield return token;

            if (setRichPresenceWaiter.Check(token)) {
                settingStatus = true;
                newlineWaiter.SetReady();
            } else if (emitMembersUpdatedWaiter.Check(token)) {
                settingStatus = false;
                newlineWaiter.SetReady();
            } else if (newlineWaiter.Check(token)) {
                const string node = "/root/WebfishingRichPresence";
                yield return new Token(TokenType.CfIf);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant(node));
                yield return new Token(TokenType.Colon);
                yield return new Token(TokenType.Dollar);
                yield return new ConstantToken(new StringVariant(node));
                yield return new Token(TokenType.Period);
                if (settingStatus) {
                    yield return new IdentifierToken("set_status");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("token");
                    yield return new Token(TokenType.ParenthesisClose);
                } else {
                    yield return new IdentifierToken("set_num_players");
                    yield return new Token(TokenType.ParenthesisOpen);
                    yield return new IdentifierToken("MEMBERS");
                    yield return new Token(TokenType.ParenthesisClose);
                }
                yield return token;
                newlineWaiter.Reset();
            }
        }
    }
}
