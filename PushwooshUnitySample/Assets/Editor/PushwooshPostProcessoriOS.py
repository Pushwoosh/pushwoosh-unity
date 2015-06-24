#!/usr/bin/env python

import sys, os.path
import shutil

install_path = sys.argv[1]
target_platform = sys.argv[2]
app_id = sys.argv[3]

print "PushwooshPostProcessor called with install path: {}, platform: {}, app_id: {}".format(install_path, target_platform, app_id)

if target_platform != "iPhone" and target_platform != "iOS":
	print "Error: unrecognized platform!"
	sys.exit()

path = os.path.join(install_path, 'Info.plist')

file = open(path, 'r')
fileData = file.read()
file.close()

replaceText = '<dict>\n\t<key>Pushwoosh_APPID</key>\n\t<string>' + app_id + '</string>\n'
fileData = fileData.replace('<dict>', replaceText, 1)

file = open(path, "w")
file.write(fileData)
file.close()

print "Info.plist sucessfully patched"
