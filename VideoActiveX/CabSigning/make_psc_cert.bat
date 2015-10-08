rem makecert -sv psc.pvk -pe -n "CN=PSC Co" -ic "adeliya-ca.cer" -is LocalMachine "psc-co.cer" 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 -------------- 1.3.6.1.5.5.7.3.1,1.3.6.1.5.5.7.3.2
rem makecert -pe -n "CN=PSC Co" -sky signature -in "adeliya-CHESIE-CA" -is my -ir LocalMachine "psc-co.cer"
rem certutil.exe -privatekey -exportpfx "adeliya-CHESIE-CA" adeliya-CA.pfx
rem makecert -pe -n "CN=PSC Co" psc.cer
rem makecert -sv psc.pvk -pe -n "CN=PSC Co" -in "adeliya-CHESIE-CA" -is LocalMachine -ir LocalMachine -ip "Microsoft RSA SChannel Cryptographic Provider" -a sha1 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"
rem makecert -pe -n "CN=PSC Co" -in "adeliya-CHESIE-CA" -is LocalMachine -ir LocalMachine -ip "Microsoft RSA SChannel Cryptographic Provider" -a sha1 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"
rem makecert -pe -n "CN=PSC Co" -ic "adeliya-CA.pfx" -is LocalMachine -ip "Microsoft RSA SChannel Cryptographic Provider" -a sha1 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"

makecert -pe -n "CN=PSC Company" -ic "adeliya-CA2.cer" -is "Trusted Root Certification Authorities" -sr CurrentUser -ss My -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"
makecert -pe -n "CN=PSC Company" -in "adeliya-CA2" -is "Trusted Root Certification Authorities" -sr CurrentUser -ss My -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"
makecert -pe -n "CN=PSC Company" -ic "adeliya-CA2.cer" -iv "adeliya-CA2.pvk" -sr CurrentUser -ss My -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 "psc-co.cer"

rem makecert -sv adeliya-CA2.pvk -r -n "CN=adeliya-CA2" -cy authority  "adeliya-CA2.cer"
rem makecert -sv adeliya-CA2.pvk -r -n "CN=adeliya-CA2" -cy authority  "adeliya-CA2.cer"

rem makecert -pe -n "CN=PSC Company" -ic "adeliya-CA2.cer" -iv "adeliya-CA2.pvk" -sr CurrentUser -ss My "psc-co.cer"

PowerShell
Remove-Item -Path cert:\LocalMachine\MyCert -Recurse


makecert -r -pe -n "CN=PSC Company" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.5.5.7.3.3 -ss MyCert
makecert -r -pe -n "CN=PSC Company" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.4.1.311.10.3.13 -ss MyCert2
makecert -r -pe -n "CN=PSC Company" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 -ss MyCert3
makecert -pe -n "CN=PSC Company (local machine)" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 -sr LocalMachine -ss MyCert4
makecert -pe -n "CN=PSC Company (local machine2)" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 -sr LocalMachine -ss LocalMachine


makecert -r -n "CN=PSC Company Cert2" -b 01/01/2000 -e 01/01/2099 -eku 1.3.6.1.5.5.7.3.3,1.3.6.1.4.1.311.10.3.13 -sv psc-com.pvk psc-com.cer
cert2spc psc-com.cer psc-com.spc
pvkimprt -pfx psc-com.spc psc-com.pvk
pvk2pfx.exe -pvk psc-com.pvk -pi psc123 -spc psc-com.spc -pfx psc-com.pfx -po psc123

