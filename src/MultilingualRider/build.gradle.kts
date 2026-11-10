plugins {
    id("java")
    id("org.jetbrains.kotlin.jvm") version "2.4.21"
    id("org.jetbrains.intellij.platform") version "2.20.1"
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
        rider("2026.2")
        bundledModule("intellij.rider.rdclient.dotnet") // solutionDirectory lives here
    }
}

intellijPlatform {
    pluginConfiguration {
        ideaVersion {
            sinceBuild = "262"
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
