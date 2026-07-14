# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

Deeper reference material (CEF upgrade playbook, IPC wire formats, OSR workarounds, known quirks) lives in [INTERNALS.md](INTERNALS.md).

## What this is

OutSystems fork of Xilium.CefGlue — a .NET 8 binding for the Chromium Embedded Framework (CEF), shipping Avalonia (Windows/macOS/Linux) and WPF (Windows-only) browser controls as NuGet packages (`CefGlue.Common`, `CefGlue.Avalonia`, `CefGlue.WPF` + `.ARM64` variants). Version (`120.6099.211`) and the CEF redist pin (`120.1.8`) live in `Directory.Build.props` and track the CEF/Chromium version scheme; bumps happen in dedicated "Raise version" PRs. There is no CI in the repo (`.github/` has only CODEOWNERS) — builds, tests, and NuGet publishing are manual/external.

## Commands

**Always pass `-p:Platform=x64` (or `ARM64`).** There is no AnyCPU. Building a `.csproj` directly (`dotnet test` / `dotnet run`) without `-p:Platform` defaults to AnyCPU, so the `CefGlue.Packages.props` / `CefGlue.CopyLocal.props` conditions match nothing and CEF natives plus the browser subprocess are silently not copied — compiles fine, fails or hangs at runtime. Building the `.sln` without `-p:Platform` instead falls back to a real solution platform (ARM64 is listed first), which on an x64 machine yields wrong-architecture natives and `.ARM64` packages.

```powershell
# Build everything
dotnet build Xilium.CefGlue.sln -c Debug -p:Platform=x64

# Run all tests (NUnit; opens real windows — needs an interactive desktop session)
dotnet test CefGlue.Tests\CefGlue.Tests.csproj -c Debug -p:Platform=x64

# Run a single test (namespace root is CefGlue.Tests, not Xilium.*)
dotnet test CefGlue.Tests\CefGlue.Tests.csproj -c Debug -p:Platform=x64 --filter "FullyQualifiedName~CefGlue.Tests.Events.EventsTests.LoadStartIsFired"

# Demos
dotnet run --project CefGlue.Demo.Avalonia\CefGlue.Demo.Avalonia.csproj -c Debug -p:Platform=x64
dotnet run --project CefGlue.Demo.WPF\CefGlue.Demo.WPF.csproj -c Debug -p:Platform=x64   # Windows only

# Pack NuGet packages (GeneratePackageOnBuild on Common/Avalonia/WPF → Nuget\output)
dotnet build Xilium.CefGlue.sln -c Release -p:Platform=x64

# Regenerate the CEF interop layer (only on CEF upgrades; Python)
CefGlue.Interop.Gen\gen-cef3.cmd
```

Notes:
- Solution configurations: `Debug`, `Debug_WindowlessRender`, `Release`, `ReleaseWPFAvalonia`. The `<Configurations>` list in `Directory.Build.props` says `DebugWindowlessRender` — that is stale; the real name has the underscore. `ReleaseWPFAvalonia` is just `Release` with the two demo projects skipped, not a different compile flavor.
- `Debug_WindowlessRender` enables offscreen rendering (`WINDOWLESS` define) for the **demos only**; tests always run windowed regardless of configuration.
- `build-wpf.cmd` is broken — it calls a `build.cmd` deleted from the repo long ago. Build the solution directly.
- First build is slow: `CefGlue.BrowserProcess` self-publishes trimmed+self-contained for win/linux/osx whenever its assembly is recompiled (`PublishApp` target; skipped on no-change incremental builds), and `Nuget.config` redirects the NuGet cache into the repo-local `packages/` folder.
- Never glob/search `packages/` (repo-local NuGet cache with tens of thousands of files), `bin/`, `obj/`, or `Nuget/output`.

## Architecture

### Process model
CEF is multi-process. The host app runs the browser process; `CefGlue.BrowserProcess` builds `Xilium.CefGlue.BrowserProcess.exe`, which CEF spawns for renderer/GPU/utility processes. It is deployed in a `CefGlueBrowserProcess/` subfolder next to the app (probed by `CefRuntimeLoader.GetSubProcessPaths`, which also accepts a flat layout as fallback); a `ResolvingUnmanagedDll` hook (`NativeLibsLoader`) resolves `libcef` from the subprocess's parent directory, which is what lets it live in the subfolder while the CEF natives stay in the app directory.

### Project layering (bottom-up)
1. **`CefGlue`** (assembly `Xilium.CefGlue`) — low-level unsafe P/Invoke binding over the CEF C API. Three layers: `Interop/` (DllImports on `libcef` + native struct mirrors), `Classes.g/` (generated lifetime halves of partial classes, roles PROXY = wraps native object / HANDLER = managed object called by native), and hand-written public halves in `Classes.Proxies/`, `Classes.Handlers/`, `Structs/`, `Enums/`. Entry point: static `CefRuntime`. Platform differences are runtime-detected (`CefRuntime.Platform`) — never add `WINDOWS`/`OSX`/`LINUX` compile-time defines here. Native libcef version is enforced by API-hash check (`CefVersionMismatchException`).
2. **`CefGlue.Common.Shared`** — the contract shared between host and render process: `RendererProcessCommunication/Messages.cs` (all IPC message structs), the custom JSON `Serializer`/`Deserializer`, named-pipe transport, `CustomScheme`, `CommonCefApp`. Nearly everything is `internal`, exposed via `InternalsVisibleTo`.
3. **`CefGlue.Common`** — the platform-agnostic browser control layer: `BaseCefBrowser` (public control facade), `CefRuntimeLoader` (deferred CEF initialization — `Initialize()` only stores a lambda; the real init runs on first browser construction), `CommonBrowserAdapter` (windowed) / `CommonOffscreenBrowserAdapter` (OSR), internal CEF handler implementations, and the host side of JS object binding (`ObjectBinding/`). Windowed vs OSR is a **process-wide** decision made at init from `CefSettings.WindowlessRenderingEnabled`, not per-control.
4. **`CefGlue.BrowserProcess`** — render-process side: `RenderProcessHandler` wires JS evaluation and object binding into V8, using `CefGlueGlobalScript.js` (embedded resource) registered as a V8 extension named `cefglue`.
5. **`CefGlue.Avalonia` / `CefGlue.WPF`** — thin UI adapters exposing `AvaloniaCefBrowser` / `WpfCefBrowser`. They override three factory methods (`CreateControl`, `CreateOffScreenControlHost`, `CreatePopupHost`) and implement input forwarding, render surfaces (`WriteableBitmap`), popups, tooltips, and context menus per toolkit.

### The `BaseCefBrowser.cs` shared-source trick
`CefGlue.Common\BaseCefBrowser.cs` is `<Compile Remove>`'d from CefGlue.Common and instead source-linked into **both** UI projects, each supplying a partial-class declaration that fixes the UI base type (Avalonia `Control`, WPF `ContentControl`). Editing that file affects both UI assemblies, and it is not in `Xilium.CefGlue.Common.dll` at all.

### JS ↔ .NET interop (spans three projects)
- **Registration**: `BaseCefBrowser.RegisterJavascriptObject` → `NativeObjectRegistry` reflects public instance methods (names camelCased) → `NativeObjectRegistrationRequest` message → renderer creates a `window.<name>` JS Proxy with one function per method.
- **Call**: the JS Proxy serializes ALL arguments into a single JSON string → `NativeObjectCallRequest` (correlated by CallId) → host deserializes against the .NET parameter types, invokes via reflection (Tasks awaited), replies `NativeObjectCallResult` → a V8 Promise resolves in JS.
- **Evaluation**: `EvaluateJavaScript<T>` → `JsEvaluationRequest` (TaskId) → renderer evals inside `cefglue.evaluateScript(...)` → JSON result deserialized to `T`.
- **Serialization is NOT plain JSON**: strings/DateTimes/byte[] carry one-char type-marker prefixes (`S`/`D`/`B`) and object graphs use `$id`/`$ref` (cycle support). The custom `Serializer`, `Deserializer`, and the JS-side stringifier/reviver in `CefGlueGlobalScript.js` must stay in sync — never replace one side with a stock JSON serializer.
- Bound objects are only injected into the **main frame's** V8 context; iframes don't get them.
- Besides process messages there is a fallback crash channel: a named pipe whose GUID name is passed via browser `extraInfo["CrashPipeName"]`; renderer .NET crashes surface as the `UnhandledException` browser event.

### Generated code — do not hand-edit
254 committed files carry the header `DO NOT MODIFY! THIS IS AUTOGENERATED FILE!`: `CefGlue\Classes.g\*.g.cs`, `CefGlue\Interop\Classes.g\*.g.cs`, `Interop\libcef.g.cs`, `Interop\version.g.cs`. They are regenerated **offline** (no MSBuild codegen step) by the Python generator in `CefGlue.Interop.Gen/` from vendored CEF headers, only when upgrading CEF. Behavior changes belong in the hand-written partial halves (`Classes.Proxies/`, `Classes.Handlers/`) or in the generator templates (`make_interop.py`). Caveat: `Interop\Structs`, `Interop\Base`, and `Enums` are hand-written mirrors of CEF headers that the generator does NOT update — layout drift there causes memory corruption, not compile errors. New CEF classes need a role entry in `schema_cef3.py` before regeneration. `CefGlue\Wrapper\**` is excluded from compilation entirely.

### Packaging layout (unusual)
`CefGlue.Common` is a fat package: `Xilium.CefGlue.dll` and `Xilium.CefGlue.Common.Shared.dll` are embedded into it (those two projects are deliberately not packaged as standalone NuGets — no `GeneratePackageOnBuild`/`PackageId`; don't add packaging to them), plus the self-contained BrowserProcess publish output under `bin/<rid>/`, plus consumer-side `build/CefGlue.Common.props/.targets` that copy the subprocess and CEF natives into consuming apps' output. The Avalonia package does **not** declare a direct Avalonia dependency (that `PackageReference` is `PrivateAssets=all`), but it does declare `Avalonia.ReactiveUI` at the pinned 11.0.9, which pulls Avalonia in transitively — consumers can override with their own Avalonia version compatible with 11.0.9.

## Testing gotchas

- Tests bootstrap one real CEF runtime + one Avalonia UI thread per process (`TestBase.cs`) and reuse a real 1×1 window; CEF can only initialize once per process, so tests **must stay sequential** — never add NUnit parallelization.
- Browser-based fixtures derive from `TestBase`; pure unit tests (`SerializationTests`, object-binding reflection tests) don't.
- The 30 s per-test timeout applies only to non-DEBUG builds; Debug runs hang forever on a stuck test.

## Other gotchas

- `CefRuntimeLoader` contains a DEBUG-only guard that throws `"Remove this fix block after CEF upgrade"` if the CEF major version ≠ 120 (tied to the `disable-features=FirstPartySets` workaround) — it must be removed/updated when bumping CEF.
- C# files are CRLF + UTF-8 BOM (enforced by `.editorconfig` and `normalize-line-endings.cmd`); Allman braces, 4-space indent.
- Both `origin` and `upstream` remotes point to `OutSystems/CefGlue` — upstream is not the original Xilium repo. Work is Jira-driven (`RDEV-*`/`RDERE-*` prefixes in PR titles); code owner is `@OutSystems/ide-team`.
- Linux ARM64 has a known libcef.so loading failure (static TLS); see `LINUX.md` for `LD_PRELOAD`/`patchelf` workarounds.
