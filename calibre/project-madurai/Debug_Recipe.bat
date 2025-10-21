SET PATH=C:\Program Files\Calibre2;%PATH%
call Delete_Your_Recipe.bat
ebook-convert Your_Recipe.recipe .epub -vvv --debug-pipeline debug > Your_Recipe.log