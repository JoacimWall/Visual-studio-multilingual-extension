package se.walltec.multilingual

import com.intellij.notification.NotificationType
import com.intellij.openapi.actionSystem.ActionUpdateThread
import com.intellij.openapi.actionSystem.AnAction
import com.intellij.openapi.actionSystem.AnActionEvent
import com.intellij.openapi.actionSystem.CommonDataKeys
import com.intellij.openapi.fileChooser.FileChooser
import com.intellij.openapi.fileChooser.FileChooserDescriptorFactory
import com.intellij.openapi.fileEditor.FileEditorManager
import com.intellij.openapi.ui.Messages
import com.intellij.openapi.vfs.LocalFileSystem
import com.intellij.openapi.vfs.VirtualFile
import java.io.File

private val RESOURCE_EXTENSIONS = setOf("resx", "resw")
private val SOLUTION_EXTENSIONS = setOf("sln", "slnx")

// Same pattern as the shared code: AppResources.fr.resx or AppResources.fr-FR.resx is a language file.
private val LANGUAGE_FILE = Regex("""([.][a-zA-Z]{2}-[a-zA-Z]{2}\.resx$)|([.][a-zA-Z]{2}\.resx$)""")

private fun VirtualFile?.isResource() = this != null && !isDirectory && extension?.lowercase() in RESOURCE_EXTENSIONS
private fun VirtualFile?.isSolution() = this != null && !isDirectory && extension?.lowercase() in SOLUTION_EXTENSIONS
private fun VirtualFile?.isMasterResx() = this != null && isResource() && extension.equals("resx", true) && !LANGUAGE_FILE.containsMatchIn(name)

/** Visible only when [visibleFor] accepts the selected file. */
abstract class MultilingualAction(private val visibleFor: (VirtualFile?) -> Boolean) : AnAction() {
    override fun getActionUpdateThread() = ActionUpdateThread.BGT

    override fun update(e: AnActionEvent) {
        e.presentation.isEnabledAndVisible = e.project != null && visibleFor(e.getData(CommonDataKeys.VIRTUAL_FILE))
    }

    final override fun actionPerformed(e: AnActionEvent) {
        val project = e.project ?: return
        perform(e, project, e.getData(CommonDataKeys.VIRTUAL_FILE) ?: return)
    }

    abstract fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile)
}

abstract class FileCommandAction(private val command: String) : MultilingualAction({ it.isResource() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) =
        MultilingualRunner.run(project, command, File(file.path).path)
}

abstract class SolutionCommandAction(private val command: String) : MultilingualAction({ it.isSolution() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) =
        MultilingualRunner.run(project, command, File(MultilingualRunner.solutionDir(project)).path)
}

class SyncAction : FileCommandAction("sync")
class TranslateAction : FileCommandAction("translate")
class SyncTranslateAction : FileCommandAction("sync-translate")

class SyncAllAction : SolutionCommandAction("sync")
class TranslateAllAction : SolutionCommandAction("translate")
class SyncTranslateAllAction : SolutionCommandAction("sync-translate")

class NativeAction : MultilingualAction({ it.isMasterResx() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) =
        MultilingualRunner.run(project, "native", File(file.path).path)
}

class ExportAction : MultilingualAction({ it.isResource() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) {
        val labels = arrayOf("New", "Need review", "Final", "All")
        val values = arrayOf("New", "NeedReview", "Final", "All")
        val index = Messages.showChooseDialog(project, "Export rows with status:", "Export", null, labels, "All")
        if (index < 0) return
        MultilingualRunner.run(project, "export", File(file.path).path, listOf("--status", values[index]))
    }
}

class ImportAction : MultilingualAction({ it.isResource() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) {
        val descriptor = FileChooserDescriptorFactory.createSingleFileDescriptor()
            .withExtensionFilter("Spreadsheet", "xlsx", "csv")
        val chosen = FileChooser.chooseFile(descriptor, project, file.parent) ?: return
        MultilingualRunner.run(project, "import", File(file.path).path, listOf("--file", File(chosen.path).path))
    }
}

class OpenSettingsAction : MultilingualAction({ it.isSolution() }) {
    override fun perform(e: AnActionEvent, project: com.intellij.openapi.project.Project, file: VirtualFile) {
        val settings = LocalFileSystem.getInstance().refreshAndFindFileByPath(MultilingualRunner.solutionDir(project) + "/MultiLingualExtensionSettings.json")
        if (settings == null)
            MultilingualRunner.notify(project, "MultiLingualExtensionSettings.json is created the first time you run a Multilingual command.", NotificationType.INFORMATION)
        else
            FileEditorManager.getInstance(project).openFile(settings, true)
    }
}
