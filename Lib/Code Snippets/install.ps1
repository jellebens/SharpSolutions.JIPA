$SourceDir = Split-Path -Parent $PSCommandPath
$Mydocuments = [environment]::getfolderpath('mydocuments')

$Source = [io.path]::combine($SourceDir, "*.snippet")

$DestinationPath = [io.path]::combine($Mydocuments, "Visual Studio 2015\Code Snippets\Visual C#\My Code Snippets")

Copy-Item -Path $Source -Destination $DestinationPath
