## Summary
- What changed and why?

## Checklist
- [ ] Code follows repo **Coding Instructions**
- [ ] Public APIs documented (XML)
- [ ] Unit tests added/updated
- [ ] No blocking async (`.Result` / `GetAwaiter().GetResult()`)
- [ ] `CancellationToken` respected
- [ ] Analyzers clean (`dotnet build` no warnings as errors)
- [ ] `dotnet format --verify-no-changes` passes

## Breaking changes?
- [ ] No
- [ ] Yes (explain impact & migration)