function moveAssets($path, $destination) {
  $path = Join-Path $PSScriptRoot -ChildPath $path
  $destination = Join-Path $PSScriptRoot -ChildPath $destination		
  if(Test-Path $path) { cp $path -Exclude *.pdb -Destination $destination }
}

moveAssets .\bin\Release\net461\win7-x64\publish\* ..\..\GlobalInstall\Windows\
moveAssets .\bin\Release\netcoreapp1.0\publish\* ..\..\GlobalInstall\Linux-Mac\netcoreapp1.0\
moveAssets .\bin\Release\netcoreapp1.1\publish\* ..\..\GlobalInstall\Linux-Mac\netcoreapp1.1\
