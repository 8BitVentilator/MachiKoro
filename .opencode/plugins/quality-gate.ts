import { spawn } from "node:child_process"
import { existsSync } from "node:fs"
import { join, relative, resolve } from "node:path"
import type { Plugin } from "@opencode-ai/plugin"

const PWSH_ARGUMENTS = ["-NoProfile", "-NonInteractive", "-ExecutionPolicy", "Bypass", "-File"]

type ScriptResult = {
  stdout: string
  stderr: string
  code: number | null
}

export const MachiKoroQualityGate: Plugin = async ({ client, worktree, directory }) => {
  const root = worktree || directory
  const hooks = join(root, ".claude", "hooks")
  const reportedSessions = new Set<string>()

  async function runScript(script: string, input: unknown): Promise<ScriptResult> {
    return new Promise((resolveResult) => {
      const child = spawn("pwsh", [...PWSH_ARGUMENTS, join(hooks, script)], {
        cwd: root,
        env: { ...process.env, CLAUDE_PROJECT_DIR: root, OPENCODE_PROJECT_DIR: root },
        stdio: ["pipe", "pipe", "pipe"],
      })

      const stdout: Buffer[] = []
      const stderr: Buffer[] = []
      child.stdout.on("data", (chunk) => stdout.push(Buffer.from(chunk)))
      child.stderr.on("data", (chunk) => stderr.push(Buffer.from(chunk)))
      child.on("error", (error) => {
        resolveResult({ stdout: "", stderr: error.message, code: 1 })
      })
      child.on("close", (code) => {
        resolveResult({
          stdout: Buffer.concat(stdout).toString("utf8"),
          stderr: Buffer.concat(stderr).toString("utf8"),
          code,
        })
      })
      child.stdin.end(JSON.stringify(input))
    })
  }

  function isProjectCSharpFile(file: unknown): file is string {
    if (typeof file !== "string" || !file.endsWith(".cs")) return false
    const absolute = resolve(root, file)
    return !relative(root, absolute).startsWith("..")
  }

  function changedCSharpFile(input: any, output: any): string | undefined {
    const candidates = [
      output?.args?.filePath,
      output?.args?.file_path,
      input?.args?.filePath,
      input?.args?.file_path,
      input?.tool_input?.file_path,
      input?.tool_response?.filePath,
    ]
    return candidates.find(isProjectCSharpFile)
  }

  function sessionId(event: any): string {
    return event?.properties?.sessionID || event?.properties?.sessionId || event?.sessionID || event?.sessionId || "opencode"
  }

  async function showQualityGateResult(result: ScriptResult, id: string): Promise<boolean> {
    if (result.stderr.trim()) {
      await client.app.log({
        body: { service: "machikoro-quality-gate", level: "warn", message: result.stderr.trim() },
      })
    }

    const text = result.stdout.trim()
    if (!text) return false

    let payload: any
    try {
      payload = JSON.parse(text)
    } catch {
      await client.app.log({
        body: { service: "machikoro-quality-gate", level: "warn", message: text },
      })
      return false
    }

    if (payload.systemMessage) {
      const isGreen = payload.systemMessage.startsWith("Quality Gate grün")
      await client.tui.showToast({
        body: { message: payload.systemMessage, variant: isGreen ? "success" : "warning" },
      })
      return !isGreen
    }

    if (payload.decision !== "block") return false

    const message = `Quality Gate fehlgeschlagen. ${payload.reason}`
    reportedSessions.add(id)
    await client.tui.showToast({ body: { message: "Quality Gate fehlgeschlagen", variant: "error" } })
    await client.session.prompt({
      path: { id },
      body: { noReply: true, parts: [{ type: "text", text: message }] },
    })
    throw new Error(message)
  }

  if (!existsSync(join(hooks, "format-file.ps1")) || !existsSync(join(hooks, "quality-gate.ps1"))) {
    await client.app.log({
      body: {
        service: "machikoro-quality-gate",
        level: "warn",
        message: "Claude hook scripts not found; opencode quality gate is disabled.",
      },
    })
    return {}
  }

  return {
    "tool.execute.after": async (input, output) => {
      const file = changedCSharpFile(input, output)
      if (!file) return

      reportedSessions.clear()
      await runScript("format-file.ps1", {
        tool_name: input.tool,
        tool_input: { file_path: file },
        tool_response: { filePath: file },
      })
    },
    event: async ({ event }) => {
      if (event.type !== "session.idle") return

      const id = sessionId(event)
      if (reportedSessions.has(id)) return

      const result = await runScript("quality-gate.ps1", { session_id: id })
      const shouldPauseUntilNextEdit = await showQualityGateResult(result, id)
      if (shouldPauseUntilNextEdit) reportedSessions.add(id)
    },
  }
}
