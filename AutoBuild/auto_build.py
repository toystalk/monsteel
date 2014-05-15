# -*- coding: utf8 -*-

"""
auto_build.py

Copyright (C) TOYS TALK - All Rights Reserved
Unauthorized copying of this file, via any medium is strictly prohibited
Proprietary and confidential

Auto build driver for the Monsteel Prototpye Project, can be run in a 
scheduled job.

Written by Rafael Batista <rafaelbatista@toystalk.com>, April 2014
"""

import os
from subprocess import call
import datetime
from shutil import copy, rmtree

UNITY_PATH_WINDOWS = "C:\Program Files (x86)\Unity\Editor\Unity.exe"

PROJECT_URL = "git@github.com:toystalk/monsteel-prototype.git"
BUILD_ROOT = "E:/monsteel-prototype-builds"
SOURCE_ROOT = os.path.join(BUILD_ROOT, "source")
LOCAL_BUILD_PATH = os.path.join(SOURCE_ROOT, "Builds")

ANDROID_BINARY_NAME = "monsteel.apk"

CLI_OPTIONS = (
	#"-batchmode",
	#"-quit",
	"-executeMethod", "AutoBuild.BuildAndroidPlayer",
	"-projectPath", SOURCE_ROOT
)

def main():
	todays_path = os.path.join(BUILD_ROOT, "daily", today())

	# Resets today's build for the script to work if it's run twice in
	# the same date (assuming something the last build failed and it was
	# quickly fixed)
	if (os.path.exists(todays_path)):
		rmtree(todays_path, onerror=try_to_give_write_permission)

	if (os.path.exists(SOURCE_ROOT)):
		rmtree(SOURCE_ROOT, onerror=try_to_give_write_permission)

	log("Downloading source code...")
	call(["git", "clone", "--depth", "1", PROJECT_URL, SOURCE_ROOT])

	log("Executing Android build...")
	call([UNITY_PATH_WINDOWS] + list(CLI_OPTIONS))

	log("Archiving build...")
	os.mkdir(todays_path)
	copy(os.path.join(LOCAL_BUILD_PATH, ANDROID_BINARY_NAME),
		 os.path.join(todays_path, ANDROID_BINARY_NAME))

def try_to_give_write_permission(func, path, exc_info):
    import stat
    if not os.access(path, os.W_OK):
        # Is the error an access error ?
        os.chmod(path, stat.S_IWUSR)
        func(path)
    else:
        raise

def log(msg):
	print "[{}] {}".format(now(), msg)

def now():
	return datetime.datetime.now().strftime("%Y-%m-%d %H:%M:%S")

def today():
	return datetime.datetime.now().strftime("%Y-%m-%d")

if __name__ == '__main__':
	main()
