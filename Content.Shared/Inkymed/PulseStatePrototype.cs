using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using Robust.Shared.Utility;

namespace Content.Shared.Inkymed;

[Prototype]
public sealed partial class PulseStatePrototype : IPrototype // todo inkymec
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public SpriteSpecifier Sprite { get; private set; } = default!;
}
