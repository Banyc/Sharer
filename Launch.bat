mkdir "./uploads.user"
%SystemRoot%\explorer.exe "uploads.user"
start dotnet run

explorer "http://localhost/UploadPage/showqr"
