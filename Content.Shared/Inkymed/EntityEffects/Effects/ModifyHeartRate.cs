using Content.Shared.Inkymed;
using Content.Shared._Shitmed.Body.Organ;
using Content.Shared.Body.Components;
using Content.Shared.EntityEffects;
using Robust.Shared.Prototypes;

namespace Content.Shared.Inkymed.EntityEffects.Effects;

/// <inheritdoc cref="EntityEffect"/>
public sealed partial class ModifyHeartRate : EntityEffect
{
    [DataField(required: true)]
    public float Amount;

    [DataField]
    public bool HeartRestart;

    [DataField]
    public bool AutoStabilisation;

    [DataField]
    public float? LowerCap;

    [DataField]
    public float? HigherCap;

    // gaslokalka forever!
    public override void Effect(EntityEffectBaseArgs args)
    {
        var entMan = args.EntityManager;
        var target = args.TargetEntity;

        // 1. i can breathe?
        if (!entMan.TrySystem<HeartRateSystem>(out var heartRateSystem))
            return;

        // 2. the big israel port
        var query = entMan.AllEntityQueryEnumerator<HeartComponent>();
        while (query.MoveNext(out var heartUid, out var heart))
        {
            if (entMan.GetComponent<TransformComponent>(heartUid).ParentUid == target)
            {
                // 3. jarvis, find me a heart
                var sign = (heart.CurrentRate > heart.NormalRate) && AutoStabilisation ? -1 : 1;
                var delta = sign * Amount;

                // 4. burger_boss
                heartRateSystem.UpdateRate(heartUid, heart, delta, HeartRestart, LowerCap, HigherCap);
                return;
            }
        }
    }

    // goida idk
    protected override string? ReagentEffectGuidebookText(IPrototypeManager prototype, IEntitySystemManager entSys)
    {
        var lines = new List<string>();

        if (AutoStabilisation)
        {
            if (Amount >= 0)
            {
                lines.Add(Loc.GetString("entity-effect-guidebook-modify-heart-rate-stabilise-increase",
                    ("amount", Math.Abs(Amount)),
                    ("highCap", HigherCap ?? 80)));
                lines.Add(Loc.GetString("entity-effect-guidebook-modify-heart-rate-stabilise-decrease",
                    ("amount", Math.Abs(Amount)),
                    ("lowCap", LowerCap ?? 80)));
            }
        }
        else
        {
            var key = Amount >= 0
                ? "entity-effect-guidebook-modify-heart-rate-increase"
                : "entity-effect-guidebook-modify-heart-rate-decrease";
            lines.Add(Loc.GetString(key, ("amount", Math.Abs(Amount))));
        }

        if (HeartRestart)
            lines.Add(Loc.GetString("entity-effect-guidebook-modify-heart-rate-restart"));

        return string.Join("\n", lines);
    }
}
