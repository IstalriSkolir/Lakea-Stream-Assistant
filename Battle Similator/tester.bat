set loop=0
:loop
"Battle Similator.exe" "DEBUG" "BOSSBATTLE" "1234" "Hellfire" 
set /a loop=%loop%+1
if "%loop%"=="100" goto next
goto loop

:next
echo Testing Complete