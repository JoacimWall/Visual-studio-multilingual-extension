plugins {
    id("java")
    id("org.jetbrains.kotlin.jvm") version "2.1.20"
    id("org.jetbrains.intellij.platform") version "2.5.0"
}

group = "se.walltec"
version = "1.0.0"

repositories {
    mavenCentral()
    intellijPlatform {
        defaultRepositories()
    }
}

kotlin {
    jvmToolchain(21)
}

dependencies {
    intellijPlatform {
        rider("2025.2")
    }
}

intellijPlatform {
    pluginConfiguration {
        ideaVersion {
            sinceBuild = "252"
            untilBuild = provider { null }
        }
    }
}

// Publishes the CLI (needs the .NET 10 SDK) so it ends up in the plugin's cli/ folder.
val publishCli by tasks.registering(Exec::class) {
    val outDir = layout.buildDirectory.dir("cli")
    inputs.files(fileTree("cli/Multilingual.Cli") { exclude("bin/**", "obj/**") })
    inputs.dir("../MultilingualExtension/MultilingualExtension.SharedCode")
    outputs.dir(outDir)
    commandLine("dotnet", "publish", "cli/Multilingual.Cli/Multilingual.Cli.csproj", "-c", "Release", "-o", outDir.get().asFile.absolutePath)
}

tasks.prepareSandbox {
    dependsOn(publishCli)
    from(layout.buildDirectory.dir("cli")) {
        into(intellijPlatform.projectName.map { "$it/cli" })
    }
}
