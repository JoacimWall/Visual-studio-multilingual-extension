package se.walltec.multilingual

import com.intellij.execution.configurations.GeneralCommandLine
import com.intellij.execution.executors.DefaultRunExecutor
import com.intellij.execution.filters.TextConsoleBuilderFactory
import com.intellij.execution.process.OSProcessHandler
import com.intellij.execution.process.ProcessAdapter
import com.intellij.execution.process.ProcessEvent
import com.intellij.execution.process.ProcessOutputTypes
import com.intellij.execution.ui.RunContentDescriptor
import com.intellij.execution.ui.RunContentManager
import com.intellij.notification.NotificationGroupManager
import com.intellij.notification.NotificationType
import com.intellij.openapi.extensions.PluginId
import com.intellij.openapi.fileEditor.FileDocumentManager
import com.intellij.openapi.project.Project
import com.intellij.openapi.util.Key
import com.intellij.openapi.util.SystemInfo
import com.intellij.openapi.vfs.LocalFileSystem
import com.intellij.openapi.vfs.VfsUtil
import com.intellij.ide.plugins.PluginManagerCore
import com.intellij.util.EnvironmentUtil
import com.intellij.execution.configurations.PathEnvironmentVariableUtil
import com.jetbrains.rider.projectView.solutionDirectory
import java.io.File
import java.nio.charset.StandardCharsets

object MultilingualRunner {
    private const val PLUGIN_ID = "se.walltec.multilingual"
    private const val GROUP = "Multilingual Extension"

    fun solutionDir(project: Project): String =
        runCatching { project.solutionDirectory.path }.getOrNull() ?: project.basePath ?: error("No solution directory")

    /** Saves documents, runs `dotnet multilingual.dll <command> <path> <extra> --solution-dir <dir>` in a Run console tab. */
    fun run(project: Project, command: String, path: String, extra: List<String> = emptyList()) {
        FileDocumentManager.getInstance().saveAllDocuments()
        val dotnet = findDotnet()
        val dll = PluginManagerCore.getPlugin(PluginId.getId(PLUGIN_ID))?.pluginPath?.resolve("cli/multilingual.dll")?.toFile()
        if (dotnet == null) return notify(project, "The .NET 10 runtime is required but no 'dotnet' was found. Install it from https://dot.net.", NotificationType.ERROR)
        if (dll == null || !dll.exists()) return notify(project, "The bundled multilingual CLI was not found in the plugin.", NotificationType.ERROR)

        val dir = solutionDir(project)
        val commandLine = GeneralCommandLine(dotnet, dll.path, command, path)
            .withParameters(extra)
            .withParameters("--solution-dir", dir)
            .withWorkDirectory(dir)
            .withParentEnvironmentType(GeneralCommandLine.ParentEnvironmentType.CONSOLE)
            .withCharset(StandardCharsets.UTF_8)

        val handler = try {
            OSProcessHandler(commandLine)
        } catch (e: Exception) {
            return notify(project, "Could not start dotnet: ${e.message}", NotificationType.ERROR)
        }
        val console = TextConsoleBuilderFactory.getInstance().createBuilder(project).console
        console.attachToProcess(handler)
        val descriptor = RunContentDescriptor(console, handler, console.component, "Multilingual: $command")
        RunContentManager.getInstance(project).showRunContent(DefaultRunExecutor.getRunExecutorInstance(), descriptor)

        val errors = ArrayDeque<String>()
        handler.addProcessListener(object : ProcessAdapter() {
            override fun onTextAvailable(event: ProcessEvent, outputType: Key<*>) {
                if (outputType != ProcessOutputTypes.STDERR) return
                event.text.lines().filter { it.isNotBlank() }.forEach {
                    errors.addLast(it.trim())
                    if (errors.size > 5) errors.removeFirst()
                }
            }

            override fun processTerminated(event: ProcessEvent) {
                LocalFileSystem.getInstance().refreshAndFindFileByPath(dir)?.let { VfsUtil.markDirtyAndRefresh(true, true, true, it) }
                if (event.exitCode == 0) notify(project, "$command done", NotificationType.INFORMATION)
                else notify(project, errors.joinToString("\n").ifEmpty { "$command failed (exit code ${event.exitCode})" }, NotificationType.ERROR)
            }
        })
        handler.startNotify()
    }

    fun notify(project: Project, message: String, type: NotificationType) {
        NotificationGroupManager.getInstance().getNotificationGroup(GROUP)
            .createNotification(message, type).notify(project)
    }

    private fun findDotnet(): String? {
        val exe = if (SystemInfo.isWindows) "dotnet.exe" else "dotnet"
        PathEnvironmentVariableUtil.findInPath("dotnet", EnvironmentUtil.getValue("PATH"), null)?.let { return it.path }
        val roots = listOfNotNull(
            System.getenv("DOTNET_ROOT"),
            if (SystemInfo.isWindows) "C:\\Program Files\\dotnet" else null,
            if (SystemInfo.isMac) "/usr/local/share/dotnet" else null,
            if (SystemInfo.isLinux) "/usr/share/dotnet" else null,
            if (SystemInfo.isLinux) "/usr/lib/dotnet" else null,
            System.getProperty("user.home") + "/.dotnet",
        )
        return roots.map { File(it, exe) }.firstOrNull { it.canExecute() }?.path
    }
}
