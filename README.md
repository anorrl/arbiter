# .arbiter

also known as ANotherRccServiceArbiterLol (ANRSAL)

This is a forked repo from [Aria](https://github.com/unc-nnected/ANotherRccServiceArbiterLol).

Please check their original project out! This project is meant to build on top of it and lean it more for ANORRL support!

```
--dir "path" | path to ACCService directory, this one is required (DUH!)

lua scripts:
should be obvious, uses --gscript, rscript, rascript, rmscript, rmmscript "path"

server config:
--port "number"
--cores "number"
--baseurl "url"
--name "name" | rccservice name

authentication from arbiter to site:
--secret "key" | api key
--accesskey "key" | gameserver key

misc:
--debug | verbose logging
--experimental | experimental features

example launch: ANRSAL.exe --dir "C:\ACCService" --port 8124 --cores 12 --secret "key" --gscript "C:\ACCService\gameserver.txt" --rscript "C:\ACCService\render.lua" --debug```
