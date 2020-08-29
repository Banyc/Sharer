mkdir "./uploads.user"
%SystemRoot%\explorer.exe "uploads.user"
start dotnet run

explorer "https://localhost/UploadPage/showqr"
