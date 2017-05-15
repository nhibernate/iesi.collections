if not exist build mkdir build
pushd src
dotnet restore
dotnet build -c release
dotnet test -f net461 -c release Iesi.Collections.Test
dotnet test -f netcoreapp1.1 -c release Iesi.Collections.Test
popd
copy src\Iesi.Collections\bin\Release\*.nupkg build
